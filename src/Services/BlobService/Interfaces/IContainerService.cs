using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VDS.BlobService.Interfaces
{
    public interface IContainerService
    {
        Task<IEnumerable<Uri>> GetBlobs(Guid wpId, Guid userId);
        Task CreateContainer(Guid wpId);
        Task DeleteContainer(Guid wpId);
        Task<Uri> UploadContainerBlob(Guid wpId, Guid userId, IFormFile file);
        Task DeleteBlob(Guid containerId, string blobPath);
    }
}