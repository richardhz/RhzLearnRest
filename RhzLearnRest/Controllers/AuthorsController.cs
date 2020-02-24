﻿using Microsoft.AspNetCore.Mvc;
using RhzLearnRest.Domains.Interfaces;
using RhzLearnRest.Domains.Models.Dtos;
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
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors()
        {
            return Ok(_manager.GetAuthors());
        }

        [HttpGet("{authorId}")]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthor(Guid authorId)
        {
            return Ok(_manager.GetAuthor(authorId));
        }

    }
}