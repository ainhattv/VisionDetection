using System;
using System.Collections.Generic;
using VDS.BlobService.Data.Entities;

namespace VDS.BlobService.Data.Entities
{
    public class BlobContainer : BaseEntities
    {
        public Guid WorkPlaceId { get; set; }

        public string Name { get; set; }

        public ICollection<BlobFolder> BlobFolders { get; set; }

        public string GetDirectory()
        {
            return this.Id.ToString();
        }
    }
}