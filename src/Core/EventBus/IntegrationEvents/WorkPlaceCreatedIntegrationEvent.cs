using System;
using System.Collections.Generic;
using System.Text;
using VDS.Core.EventBus.Events;

namespace VDS.IntegrationEvents.Events
{
    public class WorkPlaceCreatedIntegrationEvent : IntegrationEvent
    {
        public WorkPlaceCreatedIntegrationEvent(Guid workPlaceId, Guid ownerId)
        {
            OwnerId = ownerId;
            WorkPlaceId = workPlaceId;
        }

        public Guid WorkPlaceId { get; private set; }

        public Guid OwnerId { get; private set; }
    }
}
