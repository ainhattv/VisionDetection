using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VDS.UMS.Entities.ResponseModels
{
    public class CreateUserResponse : IdentityResult
    {
        public string Token { get; set; }

        public new IEnumerable<IdentityError> Errors { get; set; }
    }
}
