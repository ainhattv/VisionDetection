using System;

namespace VDS.WPS.Models.Response
{
    public class WorkPlaceResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid AuthorId { get; set; }

        public string AuthorEmail { get; set; }

        public string AuthorName { get; set; }
    }
}