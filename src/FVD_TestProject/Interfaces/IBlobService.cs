using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace FVD.Interfaces
{
    public interface IBlobService
    {
        Task<bool> UpLoadBlobs(Guid userId, List<IFormFile> files);

        Task<IEnumerable<string>> GetBlobs(string userid);

        Task<IList<IdentifyResult>> IdentityFace(IFormFile file);
    }
}