using Microsoft.AspNetCore.Mvc;
using RhzLearnRest.Domains.Interfaces;
using RhzLearnRest.Domains.Models.Dtos;
using RhzLearnRest.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RhzLearnRest.Controllers
{
    [ApiController]
    [Route("api/authorCollection")]
    public class AuthorCollectionController : ControllerBase
    {
        private readonly IDataManagerService _manager;
        public AuthorCollectionController(IDataManagerService manager)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
        }

        [HttpGet("({ids})", Name = "GetAuthorCollection")]
        public IActionResult GetAuthorCollection(
            [FromRoute]
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }
            var authors = _manager.GetAuthors(ids);

            return Ok(authors);
        }

        [HttpPost]
        public ActionResult<IEnumerable<AuthorDto>> CreateAuthorCollection(IEnumerable<NewAuthorDto> authorCollection)
        {
            var newAuthors = _manager.AddAuthorCollection(authorCollection);
            var idsAsString = string.Join(",", newAuthors.Select(a => a.Id));
            return CreatedAtRoute("GetAuthorCollection", new { ids = idsAsString }, newAuthors);
            
        }
    }
}
