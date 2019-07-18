using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDS.BlobService.Adapters;
using VDS.Core.EventBus.Abstractions;
using VDS.IntegrationEvents.Events;
using VDS.Logging;

namespace VDS.BlobService.IntegrationEvents.Eventhandling
{
    public class WorkPlaceCreatedIntegrationEventHandler : IIntegrationEventHandler<WorkPlaceCreatedIntegrationEvent>
    {
        private readonly IAppLogger<WorkPlaceCreatedIntegrationEventHandler> _logger;
        private readonly IBlobAdapter _blobAdapter;
        public WorkPlaceCreatedIntegrationEventHandler(
            IAppLogger<WorkPlaceCreatedIntegrationEventHandler> logger,
            IBlobAdapter blobAdapter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _blobAdapter = blobAdapter ?? throw new ArgumentNullException(nameof(blobAdapter));
        }

        public async Task Handle(WorkPlaceCreatedIntegrationEvent @event)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at ({@IntegrationEvent})", @event.Id, @event);

            await _blobAdapter.CreateContainer(@event.WorkPlaceId);
        }
    }
}
