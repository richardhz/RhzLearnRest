using Microsoft.AspNetCore.Mvc;
using RhzLearnRest.Domains.Interfaces;
using RhzLearnRest.Domains.Models.Dtos;
using RhzLearnRest.Domains.Models.Helpers;
using RhzLearnRest.Domains.Models.ResourceParameters;
using System;
using System.Collections.Generic;

namespace RhzLearnRest.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly IDataManagerService _manager;
        private readonly IPropertyCheckerService _propertyCheck;
        public AuthorsController(IDataManagerService manager, IPropertyCheckerService propertyChecker)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _propertyCheck = propertyChecker ?? throw new ArgumentNullException(nameof(propertyChecker));
        }

        [HttpGet(Name = "GetAuthors")]
        [HttpHead()]
        public IActionResult GetAuthors([FromQuery] AuthorResourceParameters authorsResourceParameters)
        {
            if (!_propertyCheck.TypeHasProperties<AuthorDto>(authorsResourceParameters.Fields))
            {
                return BadRequest();
            }

            var x = _manager.GetAuthors(authorsResourceParameters);
            return x == null ? BadRequest() : (IActionResult)Ok(x.ShapeData(authorsResourceParameters.Fields));
        }

        [HttpGet("{authorId}",Name ="GetAuthor")]
        public IActionResult GetAuthor(Guid authorId,string fields)
        {

            if (!_propertyCheck.TypeHasProperties<AuthorDto>(fields))
            {
                return BadRequest();
            }

            var x = _manager.GetAuthor(authorId);
            return x == null ? NotFound() : (IActionResult)Ok(x.ShapeData(fields));
        }

        [HttpPost()]
        public ActionResult<AuthorDto> CreateAuthor(NewAuthorDto author)
        {
           var newAuthor = _manager.AddAuthor(author);
           return CreatedAtRoute("GetAuthor", new { authorId = newAuthor.Id }, newAuthor);
        }

        [HttpDelete("{authorId}")]
        public ActionResult DeleteAuthor(Guid authorId)
        {
            var x = _manager.DeleteAuthor(authorId);
            return x ? NoContent() : (ActionResult)NotFound();
        }

        [HttpOptions]
        public IActionResult GetAuthorOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }
        
    }
}
