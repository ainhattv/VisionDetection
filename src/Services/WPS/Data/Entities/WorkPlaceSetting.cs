using System;
using VDS.WPS.Common.Enums;

namespace VDS.WPS.Data.Entities
{
    public class WorkPlaceSetting : BaseEntities
    {
        public Guid WorkPlaceId { get; set; }

        public WorkPlace WorkPlace { get; set; }

        public WorkPlaceSize WorkPlaceSize { get; set; }

        public WorkPlaceLevel WorkPlaceLevel { get; set; }
    }
}