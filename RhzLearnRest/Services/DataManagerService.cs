using AutoMapper;
using RhzLearnRest.Domains.Interfaces;
using RhzLearnRest.Domains.Models.Dtos;
using System;
using System.Collections.Generic;

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
    }
}
