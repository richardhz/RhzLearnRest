using RhzLearnRest.Domains.Models.Dtos;
using System;
using System.Collections.Generic;

namespace RhzLearnRest.Domains.Interfaces
{
    public interface IDataManagerService
    {
        IEnumerable<AuthorDto> GetAuthors();
        AuthorDto GetAuthor(Guid authId);
        IEnumerable<CourseDto> GetCoursesForAuthor(Guid authId);
        CourseDto GetCourseForAuthor(Guid authId, Guid courseId);
    }
}
