﻿namespace VoucherManagementACL.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Common;
    using BusinessLogic.RequestHandlers;
    using BusinessLogic.Requests;
    using Common;
    using Common.Examples;
    using DataTransferObjects.Responses;
    using Factories;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Newtonsoft.Json;
    using Shared.Logger;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ExcludeFromCodeCoverage]
    [Route(VoucherController.ControllerRoute)]
    [ApiController]
    //[Authorize]
    public class VoucherController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The mediator
        /// </summary>
        private readonly IMediator Mediator;

        /// <summary>
        /// The model factory
        /// </summary>
        private readonly IModelFactory ModelFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="modelFactory">The model factory.</param>
        public VoucherController(IMediator mediator,
                                     IModelFactory modelFactory)
        {
            this.Mediator = mediator;
            this.ModelFactory = modelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs the transaction.
        /// </summary>
        /// <param name="voucherCode">The voucher code.</param>
        /// <param name="applicationVersion">The application version.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [SwaggerResponse(200, type:typeof(GetVoucherResponseMessage))]
        [SwaggerResponseExample(200, typeof(GetVoucherResponseMessageExample))]
        public async Task<IActionResult> GetVoucher([FromQuery] String voucherCode,
                                                    [FromQuery] String applicationVersion,
                                                    CancellationToken cancellationToken)
        {
            if (ClaimsHelper.IsPasswordToken(this.User) == false)
            {
                return this.Forbid();
            }

            // Do the software version check
            try
            {
                VersionCheckRequest versionCheckRequest = VersionCheckRequest.Create(applicationVersion);
                await this.Mediator.Send(versionCheckRequest, cancellationToken);
            }
            catch(VersionIncompatibleException vex)
            {
                Logger.LogError(vex);
                return this.StatusCode(505);
            }

            Guid estateId = Guid.Parse(ClaimsHelper.GetUserClaim(this.User, "estateId").Value);
            Guid contractId = Guid.Parse(ClaimsHelper.GetUserClaim(this.User, "contractId").Value);

            // Now do the GET
            GetVoucherRequest request = GetVoucherRequest.Create(estateId, contractId, voucherCode);

            GetVoucherResponse response = await this.Mediator.Send(request, cancellationToken);

            return this.Ok(this.ModelFactory.ConvertFrom(response));
        }

        /// <summary>
        /// Redeems the voucher.
        /// </summary>
        /// <param name="voucherCode">The voucher code.</param>
        /// <param name="applicationVersion">The application version.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        [SwaggerResponse(200, type: typeof(RedeemVoucherResponseMessage))]
        [SwaggerResponseExample(200, typeof(RedeemVoucherResponseMessageExample))]
        public async Task<IActionResult> RedeemVoucher([FromQuery] String voucherCode,
                                                    [FromQuery] String applicationVersion,
                                                    CancellationToken cancellationToken)
        {
            if (ClaimsHelper.IsPasswordToken(this.User) == false)
            {
                return this.Forbid();
            }

            // Do the software version check
            try
            {
                VersionCheckRequest versionCheckRequest = VersionCheckRequest.Create(applicationVersion);
                await this.Mediator.Send(versionCheckRequest, cancellationToken);
            }
            catch (VersionIncompatibleException vex)
            {
                Logger.LogError(vex);
                return this.StatusCode(505);
            }

            Guid estateId = Guid.Parse(ClaimsHelper.GetUserClaim(this.User, "estateId").Value);
            Guid contractId = Guid.Parse(ClaimsHelper.GetUserClaim(this.User, "contractId").Value);

            // Now do the GET
            RedeemVoucherRequest request = RedeemVoucherRequest.Create(estateId, contractId, voucherCode);

            RedeemVoucherResponse response = await this.Mediator.Send(request, cancellationToken);

            return this.Ok(this.ModelFactory.ConvertFrom(response));
        }


        #endregion

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "vouchers";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/" + VoucherController.ControllerName;

        #endregion
    }
}