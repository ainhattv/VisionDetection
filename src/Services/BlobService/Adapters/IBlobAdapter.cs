using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VDS.BlobService.Adapters
{
    public interface IBlobAdapter
    {
        Task<IEnumerable<Uri>> GetBlobs(Guid containerId, Guid folderId);
         Task<Guid> CreateContainer();
         Task DeleteContainer(Guid containerId);
         Task<Uri> UploadContainerBlob(Guid containerId, Guid folderId, IFormFile file);
         Task DeleteBlob(Guid containerId, string blobPath);
    }
}