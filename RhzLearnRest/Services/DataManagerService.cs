using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RhzLearnRest.Domains.Interfaces;
using RhzLearnRest.Domains.Models;
using RhzLearnRest.Domains.Models.Dtos;
using RhzLearnRest.Domains.Models.Helpers;
using RhzLearnRest.Domains.Models.PropertyMapping;
using RhzLearnRest.Domains.Models.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace RhzLearnRest.Services
{
    public class DataManagerService : IDataManagerService
    {
        private readonly ICourseLibraryRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly IPropertyMappingService _propertyMapper;
        private Dictionary<string, PropertyMappingValue> _authorPropertyMapping;

        // The DataManager service needs to use the controllers validation functionality so we must find a way to get the controller 
        // into the service.
        public object Controller { get; set; }

        public DataManagerService(ICourseLibraryRepository repo, IMapper mapper, IUrlHelper urlHelper, IPropertyMappingService propertyMapper )
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _urlHelper = urlHelper;
            _propertyMapper = propertyMapper;

            _authorPropertyMapping = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string> {"Id"}) },
                {"MainCategory", new PropertyMappingValue(new List<string> {"MainCategory"})},
                {"Age", new PropertyMappingValue(new List<string> {"DateOfBirth"})},
                {"Name", new PropertyMappingValue(new List<string> {"FirstName","LastName"})}
            };

            _propertyMapper.Init<AuthorDto, Author>(_authorPropertyMapping);

    }

        public AuthorDto AddAuthor(NewAuthorDto author)
        {
            var authorEntity = _mapper.Map<Author>(author);
            _repo.AddAuthor(authorEntity);
            _repo.Save();
            return _mapper.Map<AuthorDto>(authorEntity);
            

        }


        public bool DeleteAuthor(Guid authorId)
        {
            var x = _repo.GetAuthor(authorId);
            if (x == null)
            {
                return false;
            }
            _repo.DeleteAuthor(x);
            _repo.Save();
            return true;

        }

        public object GetAuthor(Guid authId, bool full = false)
        {
            var x = _repo.GetAuthor(authId);
            if (full)
            {
                return _mapper.Map<AuthorFullDto>(x);
            }
            return _mapper.Map<AuthorDto>(x);
        }

        public IEnumerable<AuthorDto> GetAuthors()
        {
            var x = _repo.GetAuthors();
            return _mapper.Map<IEnumerable<AuthorDto>>(x);
        }

        //public IEnumerable<AuthorDto> GetAuthors(AuthorResourceParameters authorResourceParameters)
        public PagedList<Author> GetAuthors(AuthorResourceParameters authorResourceParameters)
        {
            if(!string.IsNullOrWhiteSpace(authorResourceParameters.OrderBy))
            {
                if (!_propertyMapper.ValidMappingExistsFor<AuthorDto, Author>(authorResourceParameters.OrderBy))
                {
                    return null;
                }
            }
            

            var x = _repo.GetAuthors(authorResourceParameters );

            //var previousPageLink = x.HasPrevious ? CreateAuthorsResourceUri(authorResourceParameters, ResourceUriType.PreviousPage) : null;
            //var nextPageLink = x.HasNext ? CreateAuthorsResourceUri(authorResourceParameters, ResourceUriType.NextPage) : null;

            var pageMetaData = new
            {
                totalCount = x.TotalCount,
                pageSize = x.PageSize,
                currentPage = x.CurrentPage,
                totalPages = x.TotalPages
            };

            _urlHelper.ActionContext.HttpContext.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pageMetaData));


            return x;// _mapper.Map<IEnumerable<AuthorDto>>(x);
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


        public bool DeleteCourseForAuthor(Guid authorId, Guid courseId)
        {

            if (!_repo.AuthorExists(authorId))
            {
                return false;
            }

            var courseToDelete = _repo.GetCourse(authorId, courseId);

            if (courseToDelete == null)
            {
                return false;
            }

            _repo.DeleteCourse(courseToDelete);
            _repo.Save();
            return true;
        }


        public object PatchCourseForAuthor(Guid authorId, Guid courseId, JsonPatchDocument<UpdateCourseDto> patchDoc)
        {
            if (!_repo.AuthorExists(authorId))
            {
                return new NotFoundResult();
            }

            var courseToUpdate = _repo.GetCourse(authorId, courseId);

            if (courseToUpdate == null)
            {
                return new NotFoundResult();
            }

            var courseToPatch = _mapper.Map<UpdateCourseDto>(courseToUpdate);
            patchDoc.ApplyTo(courseToPatch);

            if (!((ControllerBase)Controller).TryValidateModel(courseToPatch))
            {
                return ((ControllerBase)Controller).ValidationProblem(((ControllerBase)Controller).ModelState);
            }

            _mapper.Map(courseToPatch, courseToUpdate);
            _repo.UpdateCourse(courseToUpdate);
            _repo.Save();
            return new NoContentResult();

        }


        public string CreateAuthorsResourceUri(
           AuthorResourceParameters authorResourceParameters,
           ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetAuthors",
                        new
                        {
                            fields = authorResourceParameters.Fields,
                            orderBy = authorResourceParameters.OrderBy,
                            pageNumber = authorResourceParameters.PageNumber - 1,
                            pageSize = authorResourceParameters.PageSize,
                            mainCategory = authorResourceParameters.MainCategory,
                            searchQuery = authorResourceParameters.SearchQuery
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetAuthors",
                        new
                        {
                            fields = authorResourceParameters.Fields,
                            orderBy = authorResourceParameters.OrderBy,
                            pageNumber = authorResourceParameters.PageNumber + 1,
                            pageSize = authorResourceParameters.PageSize,
                            mainCategory = authorResourceParameters.MainCategory,
                            searchQuery = authorResourceParameters.SearchQuery
                        });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetAuthors",
                        new
                        {
                            fields = authorResourceParameters.Fields,
                            orderBy = authorResourceParameters.OrderBy,
                            pageNumber = authorResourceParameters.PageNumber,
                            pageSize = authorResourceParameters.PageSize,
                            mainCategory = authorResourceParameters.MainCategory,
                            searchQuery = authorResourceParameters.SearchQuery
                        });
            }
        }

    }
}
