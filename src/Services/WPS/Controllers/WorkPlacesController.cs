using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VDS.Logging;
using VDS.WPS.Interfaces;
using VDS.WPS.Models.Request;
using VDS.WPS.Models.Response;

namespace VDS.WPS.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WorkPlacesController : ControllerBase
    {
        private readonly LoggerAdapter<WorkPlacesController> _logger;
        private readonly IWorkPlaceService _workPlaceService;

        public WorkPlacesController(
            LoggerAdapter<WorkPlacesController> logger,
            IWorkPlaceService workPlaceService)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _workPlaceService = workPlaceService ?? throw new System.ArgumentNullException(nameof(workPlaceService));
        }

        [HttpGet("{userid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<WorkPlaceResponseModel> Get(Guid userid)
        {
            _logger.LogInformation($"Start get WorkPlace userid: {0}", userid);
            return await _workPlaceService.GetWorkPlaceByUserId(userid);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async void Post([FromBody] WorkPlaceRequestModel requestModel)
        {
            _logger.LogInformation($"Start Create WorkPlace requestModel: {0}", requestModel);
            await _workPlaceService.CreateWorkPlace(requestModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async void Put(Guid id, [FromBody] WorkPlaceRequestModel requestModel)
        {
            _logger.LogInformation($"Start Update WorkPlace requestModel: {0}", requestModel);
            await _workPlaceService.UpdateWorkPlace(id, requestModel);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async void Delete(Guid id)
        {
            _logger.LogInformation($"Start Delete WorkPlace id: {0}", id);
            await _workPlaceService.DeleteWorkPlace(id);
        }
    }
}