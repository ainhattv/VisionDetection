using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VDS.UMS.Interfaces;
using VDS.UMS.Entities.RequestModels;
using VDS.UMS.Entities.ResponseModels;
using VDS.UMS.Logging;

namespace VDS.UMS.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly LoggerAdapter<IdentityController> _logger;

        public IdentityController(
            IIdentityService identityService,
            LoggerAdapter<IdentityController> logger)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateUserResponse>> Create([FromBody] CreateUserRequest requestModel)
        {
            string host = Request.Host.Value;

            var result = await _identityService.Create(requestModel.FirstName, requestModel.LastName, requestModel.Email, requestModel.PassWord, host);

            return Ok(result);
        }

        [Route("ConfirmEmail")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateUserResponse>> ConfirmEmail([FromBody] EmailConfirmationRequest requestModel)
        {
            var result = await _identityService.ConfirmEmail(requestModel.UserId, requestModel.Code);

            return Ok(result);
        }

        [Route("Login")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest requestModel)
        {
            var result = await _identityService.Login(requestModel.UserName, requestModel.PassWord);

            return Ok(result);
        }
    }
}