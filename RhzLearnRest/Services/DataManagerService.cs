using AutoMapper;
using RhzLearnRest.Domains.Interfaces;
using RhzLearnRest.Domains.Models;
using RhzLearnRest.Domains.Models.Dtos;
using RhzLearnRest.Domains.Models.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RhzLearnRest.Services
{
    public class DataManagerService : IDataManagerService
    {
        private readonly ICourseLibraryRepository _repo;
        private readonly IMapper _mapper;
        public DataManagerService(ICourseLibraryRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public AuthorDto AddAuthor(NewAuthorDto author)
        {
            var authorEntity = _mapper.Map<Author>(author);
            _repo.AddAuthor(authorEntity);
            _repo.Save();
            return _mapper.Map<AuthorDto>(authorEntity);
            

        }

        public AuthorDto GetAuthor(Guid authId)
        {
            var x = _repo.GetAuthor(authId);
            return _mapper.Map<AuthorDto>(x);
        }

        public IEnumerable<AuthorDto> GetAuthors()
        {
            var x = _repo.GetAuthors();
            return _mapper.Map<IEnumerable<AuthorDto>>(x);
        }

        public IEnumerable<AuthorDto> GetAuthors(AuthorResourceParameters authorResourceParameters)
        {

            var x = _repo.GetAuthors(authorResourceParameters );
            return _mapper.Map<IEnumerable<AuthorDto>>(x);
        }

        public IEnumerable<AuthorDto> GetAuthors(IEnumerable<Guid> authorIds)
        {
            var x = _repo.GetAuthors(authorIds);
            if (authorIds.Count() != x.Count())
            {
                return null;
            }

            return _mapper.Map<IEnumerable<AuthorDto>>(x);
        }

    

        public IEnumerable<CourseDto> GetCoursesForAuthor(Guid authId)
        {
            if (!_repo.AuthorExists(authId))
            {
                return null;
            }
            var x = _repo.GetCourses(authId);
            return _mapper.Map<IEnumerable<CourseDto>>(x);
        }

        public CourseDto GetCourseForAuthor(Guid authId, Guid courseId)
        {
            if (!_repo.AuthorExists(authId))
            {
                return null;
            }
            var x = _repo.GetCourse(authId, courseId);
            if (x == null)
            {
                return null;
            }
            return _mapper.Map<CourseDto>(x);
        }


        public CourseDto AddCourseForAuthor(Guid authorId, NewCourseDto course)
        {
            if (!_repo.AuthorExists(authorId))
            {
                return null;
            }
            var courseEntity = _mapper.Map<Course>(course);
            _repo.AddCourse(authorId,courseEntity);
            _repo.Save();
            return _mapper.Map<CourseDto>(courseEntity);
        }

        public IEnumerable<AuthorDto> AddAuthorCollection(IEnumerable<NewAuthorDto> authors)
        {
            var authorEntities = _mapper.Map<IEnumerable<Author>>(authors);
            foreach(var author in authorEntities)
            {
                _repo.AddAuthor(author);
            }
            _repo.Save();
            return _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
        }

        public bool UpdateCourseForAuthor(Guid authorId, Guid courseId, UpdateCourseDto course)
        {
            if (!_repo.AuthorExists(authorId))
            {
                return false;
            }

            var courseToUpdate = _repo.GetCourse(authorId, courseId);

            if (courseToUpdate == null)
            {
                return false;
            }

            // EF is tracking "courseToUpdate" so by executing the next line _mapper.Map the entity has changed 
            // to a modified state so executing the Save will write the changes to the database. 
            // We could just omit the Update statement, however this would be incorrect, in the future something 
            // may change which would require some functionality in the update process. 
            // The user of the repository does not need to know this.
            _mapper.Map(course, courseToUpdate);
            _repo.UpdateCourse(courseToUpdate);
            _repo.Save();
            return true;
        }
    }
}
