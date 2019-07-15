using System;

namespace VDS.BlobService.Data.Entities
{
    public abstract class BaseEntities
    {
        public Guid Id { get; set; }

        public bool IsActive { get; set; }
    }
}