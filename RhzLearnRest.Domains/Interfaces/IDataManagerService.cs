using Microsoft.AspNetCore.JsonPatch;
using RhzLearnRest.Domains.Models;
using RhzLearnRest.Domains.Models.Dtos;
using RhzLearnRest.Domains.Models.Helpers;
using RhzLearnRest.Domains.Models.ResourceParameters;
using System;
using System.Collections.Generic;

namespace RhzLearnRest.Domains.Interfaces
{
    public interface IDataManagerService
    {
        public object Controller { get; set; }
        IEnumerable<AuthorDto> GetAuthors();
        PagedList<Author> GetAuthors(AuthorResourceParameters authorResourceParameters);
        IEnumerable<AuthorDto> GetAuthors(IEnumerable<Guid> authorIds);
        Object GetAuthor(Guid authId, bool full = false);
        bool DeleteAuthor(Guid authorId);
        IEnumerable<CourseDto> GetCoursesForAuthor(Guid authId);
        CourseDto GetCourseForAuthor(Guid authId, Guid courseId);
        AuthorDto AddAuthor(NewAuthorDto author);
        CourseDto AddCourseForAuthor(Guid authorId, NewCourseDto course);
        IEnumerable<AuthorDto> AddAuthorCollection(IEnumerable<NewAuthorDto> authors);
        bool UpdateCourseForAuthor(Guid authorId, Guid courseId, UpdateCourseDto course);
        bool DeleteCourseForAuthor(Guid authorId, Guid courseId);
        object PatchCourseForAuthor(Guid authorId, Guid courseId, JsonPatchDocument<UpdateCourseDto> patchDoc);
        string CreateAuthorsResourceUri(AuthorResourceParameters authorResourceParameters, ResourceUriType type);
    }
}
