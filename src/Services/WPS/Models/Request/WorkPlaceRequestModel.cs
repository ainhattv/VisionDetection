using System;
using System.ComponentModel.DataAnnotations;

namespace VDS.WPS.Models.Request
{
    public class WorkPlaceRequestModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public Guid AuthorId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string AuthorEmail { get; set; }

        [StringLength(100)]
        public string AuthorName { get; set; }
    }
}