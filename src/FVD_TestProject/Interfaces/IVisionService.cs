using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace FVD.Interfaces
{
    public interface IVisionService
    {
        Task<bool> AddPersonFaceByUserId(Guid userId, string filePath);

        Task TrainExecute();

        Task<IList<IdentifyResult>> FaceIdentity(string filePath);
    }
}