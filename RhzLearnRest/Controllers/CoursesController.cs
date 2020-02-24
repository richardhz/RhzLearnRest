using Microsoft.AspNetCore.Mvc;
using RhzLearnRest.Domains.Interfaces;
using RhzLearnRest.Domains.Models.Dtos;
using System;
using System.Collections.Generic;

namespace RhzLearnRest.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly IDataManagerService _manager;
        public CoursesController(IDataManagerService manager)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
        }

        [HttpGet()]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId)
        {
            IEnumerable<CourseDto> x = _manager.GetCoursesForAuthor(authorId);
            return x == null ? NotFound() : (ActionResult<IEnumerable<CourseDto>>)Ok(x);
        }

        [HttpGet("{courseId}")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            var x = _manager.GetCourseForAuthor(authorId, courseId);
            return x == null ? NotFound() : (ActionResult<CourseDto>)Ok(x);
        }
    }
}
