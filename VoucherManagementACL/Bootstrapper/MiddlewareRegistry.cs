﻿namespace VoucherManagementACL.Bootstrapper
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net.Http;
    using System.Reflection;
    using Common;
    using Lamar;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Shared.Extensions;
    using Shared.General;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class MiddlewareRegistry : ServiceRegistry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MiddlewareRegistry"/> class.
        /// </summary>
        public MiddlewareRegistry()
        {
            this.AddHealthChecks().AddSecurityService(this.ApiEndpointHttpHandler).AddTransactionProcessorService();

            this.AddSwaggerGen(c =>
                               {
                                   c.SwaggerDoc("v1",
                                                new OpenApiInfo
                                                {
                                                    Title = "Voucher Management ACL",
                                                    Version = "1.0",
                                                    Description = "A REST Api to provide and Anti Corruption Layer for the Voucher Mobile Application",
                                                    Contact = new OpenApiContact
                                                              {
                                                                  Name = "Stuart Ferguson",
                                                                  Email = "golfhandicapping@btinternet.com"
                                                              }
                                                });
                                   // add a custom operation filter which sets default values
                                   c.OperationFilter<SwaggerDefaultValues>();
                                   c.ExampleFilters();

                                   //Locate the XML files being generated by ASP.NET...
                                   var directory = new DirectoryInfo(AppContext.BaseDirectory);
                                   var xmlFiles = directory.GetFiles("*.xml");

                                   //... and tell Swagger to use those XML comments.
                                   foreach (FileInfo fileInfo in xmlFiles)
                                   {
                                       c.IncludeXmlComments(fileInfo.FullName);
                                   }
                               });

            this.AddSwaggerExamplesFromAssemblyOf<SwaggerJsonConverter>();

            this.AddAuthentication(options =>
                                   {
                                       options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                       options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                       options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                                   }).AddJwtBearer(options =>
                                                   {
                                                       options.BackchannelHttpHandler = new HttpClientHandler
                                                                                        {
                                                                                            ServerCertificateCustomValidationCallback = (message,
                                                                                                certificate,
                                                                                                chain,
                                                                                                sslPolicyErrors) => true
                                                                                        };
                                                       options.Authority = ConfigurationReader.GetValue("SecurityConfiguration", "Authority");
                                                       options.Audience = ConfigurationReader.GetValue("SecurityConfiguration", "ApiName");

                                                       options.TokenValidationParameters = new TokenValidationParameters
                                                                                           {
                                                                                               ValidateAudience = false,
                                                                                               ValidAudience =
                                                                                                   ConfigurationReader.GetValue("SecurityConfiguration", "ApiName"),
                                                                                               ValidIssuer =
                                                                                                   ConfigurationReader.GetValue("SecurityConfiguration", "Authority"),
                                                                                           };
                                                       options.IncludeErrorDetails = true;
                                                   });

            this.AddControllers().AddNewtonsoftJson(options =>
                                                    {
                                                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                                                        options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                                                        options.SerializerSettings.Formatting = Formatting.Indented;
                                                        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                                                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                                                    });

            Assembly assembly = this.GetType().GetTypeInfo().Assembly;
            this.AddMvcCore().AddApplicationPart(assembly).AddControllersAsServices();
        }

        #endregion

        #region Methods

        /// <summary>
        /// APIs the endpoint HTTP handler.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        private HttpClientHandler ApiEndpointHttpHandler(IServiceProvider serviceProvider)
        {
            return new HttpClientHandler
                   {
                       ServerCertificateCustomValidationCallback = (message,
                                                                    cert,
                                                                    chain,
                                                                    errors) =>
                                                                   {
                                                                       return true;
                                                                   }
                   };
        }

        #endregion
    }
}