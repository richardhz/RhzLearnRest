using Microsoft.AspNetCore.Mvc;
using RhzLearnRest.Domains.Interfaces;
using RhzLearnRest.Domains.Models.Dtos;
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
        public AuthorsController(IDataManagerService manager)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
        }

        [HttpGet(Name = "GetAuthors")]
        [HttpHead()]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery] AuthorResourceParameters authorsResourceParameters)
        {
            var x = _manager.GetAuthors(authorsResourceParameters);
            return x == null ? BadRequest() : (ActionResult<IEnumerable<AuthorDto>>)Ok(x);
        }

        [HttpGet("{authorId}",Name ="GetAuthor")]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId)
        {
            var x = _manager.GetAuthor(authorId);
            return x == null ? NotFound() : (ActionResult<AuthorDto>)Ok(x);
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
