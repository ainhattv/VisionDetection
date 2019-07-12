using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VDS.UMS.Entities.RequestModels
{
    public class LoginRequest
    {
        public string UserName { get; set; }

        public string PassWord { get; set; }
    }
}
