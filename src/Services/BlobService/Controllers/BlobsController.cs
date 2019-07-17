using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VDS.BlobService.Interfaces;
using VDS.Logging;

namespace BlobService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BlobsController : ControllerBase
    {
        private readonly IContainerService _containerService;
        private readonly IAppLogger<BlobsController> _logger;

        public BlobsController(
            IContainerService containerService,
            IAppLogger<BlobsController> logger)
        {
            _containerService = containerService ?? throw new System.ArgumentNullException(nameof(containerService));
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IEnumerable<Uri>> Get([FromQuery]Guid wpId, [FromQuery]Guid userId)
        {
            _logger.LogInformation($"Start get blobs userid: {userId} && wpId: {wpId}");
            return await _containerService.GetBlobs(wpId, userId);
        }

        [HttpPost("Upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Uri> Post([FromQuery]Guid wpId, [FromQuery]Guid userId, [FromBody]IFormFile file)
        {
            _logger.LogInformation($"Start upload blob userid: {wpId} && userId: {userId}");
            return await _containerService.UploadContainerBlob(wpId, userId, file);
        }
    }
}