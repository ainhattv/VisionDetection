using System;
using VDS.BlobService.Data.Entities;

namespace VDS.BlobService.Data.Entities
{
    public class BlobFolder : BaseEntities
    {
        public Guid UserId { get; set; }

        public Guid BlobContainerId { get; set; }

        public BlobContainer BlobContainer { get; set; }
    }
}