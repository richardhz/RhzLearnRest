﻿using RhzLearnRest.Domains.Models;
using RhzLearnRest.Domains.Models.Helpers;
using RhzLearnRest.Domains.Models.ResourceParameters;
using System;
using System.Collections.Generic;

namespace RhzLearnRest.Domains.Interfaces
{
    public interface ICourseLibraryRepository
    {
        IEnumerable<Course> GetCourses(Guid authorId);
        Course GetCourse(Guid authorId, Guid courseId);
        void AddCourse(Guid authorId, Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(Course course);
        IEnumerable<Author> GetAuthors();
        PagedList<Author> GetAuthors(AuthorResourceParameters authorResourceParameters);
        Author GetAuthor(Guid authorId);
        IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds);
        void AddAuthor(Author author);
        void DeleteAuthor(Author author);
        void UpdateAuthor(Author author);
        bool AuthorExists(Guid authorId);
        bool Save();
    }
}
