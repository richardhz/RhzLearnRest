using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            _manager.Controller = this;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId)
        {
            IEnumerable<CourseDto> x = _manager.GetCoursesForAuthor(authorId);
            return x == null ? NotFound() : (ActionResult<IEnumerable<CourseDto>>)Ok(x);
        }

        [HttpGet("{courseId}",Name = "GetCourseForAuthor")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            var x = _manager.GetCourseForAuthor(authorId, courseId);
            return x == null ? NotFound() : (ActionResult<CourseDto>)Ok(x);
        }

        [HttpPost]
        public ActionResult<CourseDto> CreateCourseForAuthor(Guid authorId, NewCourseDto course)
        {
            var newCourse = _manager.AddCourseForAuthor(authorId, course);
            return newCourse == null ? NotFound() : (ActionResult<CourseDto>)CreatedAtRoute("GetCourseForAuthor",
                new { authorId, courseId = newCourse.Id },newCourse);
        }

        [HttpPut("{courseId}")]
        public ActionResult UpdateCourseForAuthor(Guid authorId, Guid courseId, UpdateCourseDto course)
        {
            var x = _manager.UpdateCourseForAuthor(authorId, courseId,course);
            return x ? NoContent() : (ActionResult)NotFound();
        }

        [HttpPatch("{courseId}")]
        public ActionResult PartialUpdateCourseForAuthor(Guid authorId,Guid courseId, JsonPatchDocument<UpdateCourseDto> patchDoc)
        {
            var x = (ActionResult)_manager.PatchCourseForAuthor(authorId, courseId, patchDoc);
            //return x ? NoContent() : (ActionResult)NotFound();
            return x;
            
        }


        [HttpDelete("{courseId}")]
        public ActionResult DeleteCourseForAuthor(Guid authorId, Guid courseId)
        {
            var x = _manager.DeleteCourseForAuthor(authorId, courseId);
            return x ? NoContent() : (ActionResult)NotFound();
        }

        //We want this controller to use the APIBehavour configured in the startup file :- InvalidModelStateResponseFactory
        //So we must override ValidationProblem.
        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
            
        }
    }
}
