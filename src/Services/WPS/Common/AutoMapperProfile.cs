using System.Collections.Generic;
using AutoMapper;
using VDS.WPS.Data.Entities;
using VDS.WPS.Models.Response;

namespace wps.Common
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<WorkPlace, WorkPlaceResponseModel>();
        }

    }
}