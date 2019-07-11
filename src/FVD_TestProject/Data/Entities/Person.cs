using System;

namespace FVD.Data.Entities
{
    public class Person
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }
    }
}