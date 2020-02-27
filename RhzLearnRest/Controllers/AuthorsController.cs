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

        [HttpGet()]
        [HttpHead()]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery] AuthorResourceParameters authorsResourceParameters)
        {
            return Ok(_manager.GetAuthors(authorsResourceParameters));
        }

        [HttpGet("{authorId}",Name ="GetAuthor")]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId)
        {
            return Ok(_manager.GetAuthor(authorId));
        }

        [HttpPost()]
        public ActionResult<AuthorDto> CreateAuthor(NewAuthorDto author)
        {
           var newAuthor = _manager.AddAuthor(author);
           return CreatedAtRoute("GetAuthor", new { authorId = newAuthor.Id }, newAuthor);
        }

        [HttpOptions]
        public IActionResult GetAuthorOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }
    }
}
