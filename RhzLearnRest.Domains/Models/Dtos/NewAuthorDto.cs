using System;
using System.Collections.Generic;
using System.Text;

namespace RhzLearnRest.Domains.Models.Dtos
{
    public class NewAuthorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string MainCategory { get; set; }
        public ICollection<NewCourseDto> Courses { get; set; } = new List<NewCourseDto>();
    }
}
