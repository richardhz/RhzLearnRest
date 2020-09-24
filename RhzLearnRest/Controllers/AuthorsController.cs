using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RhzLearnRest.Domains.Interfaces;
using RhzLearnRest.Domains.Models.Dtos;
using RhzLearnRest.Domains.Models.Helpers;
using RhzLearnRest.Domains.Models.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RhzLearnRest.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDataManagerService _manager;
        private readonly IPropertyCheckerService _propertyCheck;
        public AuthorsController(IDataManagerService manager, IPropertyCheckerService propertyChecker, IMapper mapper)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
            if (x == null)
            {
                return NotFound();
            }

            var links = CreateLinksForAuthors(authorsResourceParameters,x.HasNext,x.HasPrevious);

            var shapedAuthors = _mapper.Map<IEnumerable<AuthorDto>>(x).ShapeData(authorsResourceParameters.Fields);

            var shapedAuthorsWithLinks = shapedAuthors.Select(author =>
            {
                var authorAsDictionary = author as IDictionary<string, object>;
                var authorLinks = CreateLinksForAuthor((Guid)authorAsDictionary["Id"], null);
                authorAsDictionary.Add("links", authorLinks);
                return authorAsDictionary;
            });

            var LinkedCollectionResource = new
            {
                value = shapedAuthorsWithLinks,
                links
            };

            return Ok(LinkedCollectionResource);
        }

        [HttpGet("{authorId}",Name ="GetAuthor")]
        
        public IActionResult GetAuthor(Guid authorId,string fields, [FromHeader(Name = "Accept")] string mediaType)
        {

            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType)) 
            {
                return BadRequest();
            }

            if (!_propertyCheck.TypeHasProperties<AuthorDto>(fields))
            {
                return BadRequest();
            }

            var fulldata = VendorMediaTypes.Full(parsedMediaType.SubTypeWithoutSuffix.ToString());
            var x = _manager.GetAuthor(authorId,fulldata);
           
            if (x == null)
            {
                return NotFound();
            }

            var includeLinks = VendorMediaTypes.WithHateoas(parsedMediaType.SubTypeWithoutSuffix.ToString());

            IEnumerable<LinkDto> links = new List<LinkDto>();
            if (includeLinks)
            {
                links = CreateLinksForAuthor(authorId, fields);
            }

            var primaryMediaType = includeLinks ? parsedMediaType.SubTypeWithoutSuffix.Substring(0, parsedMediaType.SubTypeWithoutSuffix.Length - 8) : parsedMediaType.SubTypeWithoutSuffix;

            if (fulldata)
            {
                var linkedResourceToReturn = ((AuthorFullDto)x).ShapeData(fields) as IDictionary<string, object>;
                if (includeLinks)
                    linkedResourceToReturn.Add("links", links);
                return Ok(linkedResourceToReturn);
            }
            else
            {
                var linkedResourceToReturn = ((AuthorDto)x).ShapeData(fields) as IDictionary<string, object>;
                if (includeLinks)
                    linkedResourceToReturn.Add("links", links);
                return Ok(linkedResourceToReturn);
            }
        }

        [HttpPost(Name = "CreateAuthor")]
        public ActionResult<AuthorDto> CreateAuthor(NewAuthorDto author)
        {
            var newAuthor = _manager.AddAuthor(author);
            var linkedResourceToReturn = newAuthor.ShapeData(null) as IDictionary<string, object>;
            var links = CreateLinksForAuthor(newAuthor.Id, null);
            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetAuthor", new { authorId = linkedResourceToReturn["Id"] }, linkedResourceToReturn);
        }

        [HttpDelete("{authorId}", Name = "DeleteAuthor")]
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

        private IEnumerable<LinkDto> CreateLinksForAuthor(Guid authorId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinkDto(Url.Link("GetAuthor", new { authorId }), "self", "GET"));
            } 
            else
            {
                links.Add(new LinkDto(Url.Link("GetAuthor", new { authorId, fields }), "self", "GET"));
            }

            links.Add(new LinkDto(Url.Link("DeleteAuthor", new { authorId }), "delete_author", "DELETE"));

            links.Add(new LinkDto(Url.Link("CrateCourseForAuthor", new { authorId }), "create_course_for_author", "POST"));

            links.Add(new LinkDto(Url.Link("GetCoursesForAuthor", new { authorId }), "courses", "GET"));


            return links;
        }


        private IEnumerable<LinkDto> CreateLinksForAuthors(AuthorResourceParameters authorsResourceParameters,bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();
            links.Add(new LinkDto(_manager.CreateAuthorsResourceUri(authorsResourceParameters,ResourceUriType.Current),"self","GET"));
            if (hasNext)
            {
                links.Add(new LinkDto(_manager.CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.NextPage), "nextPage", "GET"));
            }
            if (hasPrevious)
            {
                links.Add(new LinkDto(_manager.CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.PreviousPage), "previousPage", "GET"));
            }
            return links;
        }


    }
}
