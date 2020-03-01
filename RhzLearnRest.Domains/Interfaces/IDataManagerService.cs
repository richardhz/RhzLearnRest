using Microsoft.AspNetCore.JsonPatch;
using RhzLearnRest.Domains.Models.Dtos;
using RhzLearnRest.Domains.Models.ResourceParameters;
using System;
using System.Collections.Generic;

namespace RhzLearnRest.Domains.Interfaces
{
    public interface IDataManagerService
    {
        public object Controller { get; set; }
        IEnumerable<AuthorDto> GetAuthors();
        IEnumerable<AuthorDto> GetAuthors(AuthorResourceParameters authorResourceParameters);
        IEnumerable<AuthorDto> GetAuthors(IEnumerable<Guid> authorIds);
        AuthorDto GetAuthor(Guid authId);
        IEnumerable<CourseDto> GetCoursesForAuthor(Guid authId);
        CourseDto GetCourseForAuthor(Guid authId, Guid courseId);
        AuthorDto AddAuthor(NewAuthorDto author);
        CourseDto AddCourseForAuthor(Guid authorId, NewCourseDto course);
        IEnumerable<AuthorDto> AddAuthorCollection(IEnumerable<NewAuthorDto> authors);
        bool UpdateCourseForAuthor(Guid authorId, Guid courseId, UpdateCourseDto course);
        object PatchCourseForAuthor(Guid authorId, Guid courseId, JsonPatchDocument<UpdateCourseDto> patchDoc);
    }
}
