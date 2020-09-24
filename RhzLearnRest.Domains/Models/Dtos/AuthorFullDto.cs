using System;
using System.Collections.Generic;
using System.Text;

namespace RhzLearnRest.Domains.Models.Dtos
{
    public class AuthorFullDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string MainCategory { get; set; }
    }
}
