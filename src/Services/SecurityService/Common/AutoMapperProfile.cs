using AutoMapper;
using Microsoft.AspNetCore.Identity;
using VDS.UMS.Entities;
using VDS.UMS.Entities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VDS.UMS.Common
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IdentityResult, CreateUserResponse>();
            CreateMap<SignInResult, LoginResponse>();
        }

    }
}