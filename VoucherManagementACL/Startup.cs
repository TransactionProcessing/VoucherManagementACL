namespace VoucherManagementACL
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net.Http;
    using System.Reflection;
    using BusinessLogic.RequestHandlers;
    using BusinessLogic.Requests;
    using BusinessLogic.Services;
    using Common;
    using Factories;
    using HealthChecks.UI.Client;
    using MediatR;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NLog.Extensions.Logging;
    using SecurityService.Client;
    using Shared.Extensions;
    using Shared.General;
    using Shared.Logger;
    using Swashbuckle.AspNetCore.Filters;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using VoucherManagement.Client;
    using ILogger = Microsoft.Extensions.Logging.ILogger;

    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IWebHostEnvironment webHostEnvironment)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(webHostEnvironment.ContentRootPath)
                                                                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                                      .AddJsonFile($"appsettings.{webHostEnvironment.EnvironmentName}.json", optional: true).AddEnvironmentVariables();

            Startup.Configuration = builder.Build();
            Startup.WebHostEnvironment = webHostEnvironment;
        }

        public static IConfigurationRoot Configuration { get; set; }

        public static IWebHostEnvironment WebHostEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigurationReader.Initialise(Startup.Configuration);

            this.ConfigureMiddlewareServices(services);

            services.AddTransient<IMediator, Mediator>();

            services.AddTransient<ServiceFactory>(context =>
                                                  {
                                                      return t => context.GetService(t);
                                                  });
            services.AddSingleton<IModelFactory, ModelFactory>();
            services.AddSingleton<IRequestHandler<VersionCheckRequest,Unit>, VersionCheckRequestHandler>();
            services.AddSingleton<IRequestHandler<GetVoucherRequest, GetVoucherResponse>, VoucherRequestHandler>();
            services.AddSingleton<IVoucherManagementACLApplicationService, VoucherManagementACLApplicationService>();
            services.AddSingleton<ISecurityServiceClient, SecurityServiceClient>();
            services.AddSingleton<IVoucherManagementClient, VoucherManagementClient>();
            services.AddSingleton<Func<String, String>>(container => (serviceName) =>
                                                                     {
                                                                         return ConfigurationReader.GetBaseServerUri(serviceName).OriginalString;
                                                                     });
            services.AddSingleton<HttpClient>();
        }

        private void ConfigureMiddlewareServices(IServiceCollection services)
        {
            services.AddHealthChecks().AddUrlGroup(new Uri($"{ConfigurationReader.GetValue("SecurityConfiguration", "Authority")}/health"),
                                                   name:"Security Service",
                                                   httpMethod:HttpMethod.Get,
                                                   failureStatus:HealthStatus.Unhealthy,
                                                   tags:new string[] {"security", "authorisation"});
                    //.AddUrlGroup(new Uri($"{ConfigurationReader.GetValue("AppSettings", "TransactionProcessorApi")}/health"),
                    //         name: "Transaction Processor Service",
                    //         httpMethod: HttpMethod.Get,
                    //         failureStatus: HealthStatus.Unhealthy,
                    //         tags: new string[] { "application", "transactionprocessing" });

            services.AddApiVersioning(
                                      options =>
                                      {
                                          // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                                          options.ReportApiVersions = true;
                                          options.DefaultApiVersion = new ApiVersion(1, 0);
                                          options.AssumeDefaultVersionWhenUnspecified = true;
                                          options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                                      });

            services.AddVersionedApiExplorer(
                                             options =>
                                             {
                                                 // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                                                 // note: the specified format code will format the version as "'v'major[.minor][-status]"
                                                 options.GroupNameFormat = "'v'VVV";

                                                 // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                                                 // can also be used to control the format of the API version in route templates
                                                 options.SubstituteApiVersionInUrl = true;
                                             });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(c =>
            {
                // add a custom operation filter which sets default values
                c.OperationFilter<SwaggerDefaultValues>();
                c.ExampleFilters();
            });

            services.AddSwaggerExamplesFromAssemblyOf<SwaggerJsonConverter>();

            services.AddAuthentication(options =>
                                       {
                                           options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                           options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                           options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                                       })
                    .AddJwtBearer(options =>
                    {
                       //options.SaveToken = true;
                       options.Authority = ConfigurationReader.GetValue("SecurityConfiguration", "Authority");
                        options.Audience = ConfigurationReader.GetValue("SecurityConfiguration", "ApiName");
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidAudience = ConfigurationReader.GetValue("SecurityConfiguration", "ApiName"),
                            ValidIssuer = ConfigurationReader.GetValue("SecurityConfiguration", "Authority"),
                        };
                        options.IncludeErrorDetails = true;
                    });

            services.AddControllers();
                
            Assembly assembly = this.GetType().GetTypeInfo().Assembly;
            services.AddMvcCore().AddApplicationPart(assembly).AddControllersAsServices();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory,
                              IApiVersionDescriptionProvider provider)
        {
            String nlogConfigFilename = "nlog.config";

            if (env.IsDevelopment())
            {
                nlogConfigFilename = $"nlog.{env.EnvironmentName}.config";
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.ConfigureNLog(Path.Combine(env.ContentRootPath, nlogConfigFilename));
            loggerFactory.AddNLog();

            ILogger logger = loggerFactory.CreateLogger("TransactionProcessor");

            Logger.Initialise(logger);

            app.AddRequestLogging();
            app.AddResponseLogging();
            app.AddExceptionHandler();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapControllers();
                                 endpoints.MapHealthChecks("health", new HealthCheckOptions()
                                                                     {
                                                                         Predicate = _ => true,
                                                                         ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                                                                     });
                             });

            app.UseSwagger();

            app.UseSwaggerUI(
                             options =>
                             {
                                 // build a swagger endpoint for each discovered API version
                                 foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                                 {
                                     options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                                 }
                             });
        }
    }
}
