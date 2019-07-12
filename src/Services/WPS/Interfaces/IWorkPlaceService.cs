using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDS.WPS.Models.Request;
using VDS.WPS.Models.Response;

namespace VDS.WPS.Interfaces
{
    public interface IWorkPlaceService
    {
        Task<WorkPlaceResponseModel> GetWorkPlaceByUserId(Guid userId);
        Task CreateWorkPlace(WorkPlaceRequestModel workPlace);
        Task UpdateWorkPlace(Guid id, WorkPlaceRequestModel workPlace);
        Task DeleteWorkPlace(Guid id);
    }
}