using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VDS.UMS.Entities.RequestModels
{
    public class CreateUserRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string PassWord { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
