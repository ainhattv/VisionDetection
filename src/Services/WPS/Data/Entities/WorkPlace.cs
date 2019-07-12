using System;
using System.ComponentModel.DataAnnotations;

namespace VDS.WPS.Data.Entities
{
    public class WorkPlace : BaseEntities
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public Guid AuthorId { get; set; }
        [Required]
        public string AuthorEmail { get; set; }
        [Required]
        public string AuthorName { get; set; }

        public WorkPlaceSetting WorkPlaceSetting { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
    }
}