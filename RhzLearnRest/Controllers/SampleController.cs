using Microsoft.AspNetCore.Mvc;
using System;

namespace RhzLearnRest.Controllers
{
    public class Sample
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
    }

    [ApiController]
    [Route("api/sample")]
    public class SampleController : ControllerBase
    {
        [HttpGet()]
        public IActionResult GetSample()
        {
            return Ok(new Sample
            {
                Id = 100,
                Name = "Sample Data",
                Created = DateTime.Now.Date
            });
        }
    }
}
