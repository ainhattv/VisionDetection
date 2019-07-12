using Microsoft.AspNetCore.Identity;
using VDS.UMS.Entities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VDS.UMS.Interfaces
{
    public interface IIdentityService
    {
        Task<CreateUserResponse> Create(string firstName, string lastName, string email, string password, string hostname);

        Task<LoginResponse> Login(string userName, string password);

        Task<bool> ConfirmEmail(string userId, string code);
    }
}
