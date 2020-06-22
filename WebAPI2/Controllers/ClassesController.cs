using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI2.Controllers
{
    [ApiVersion("1.0")]
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ClassesController : ControllerBase
    {
        private static readonly Guid[] ClassIds = new[] {
            Guid.Parse("7086d3b8-886e-4904-89ec-8cb2e8ae5621"),
            Guid.Parse("e5d926c5-7752-4b43-b491-a98633d21745"),
            Guid.Parse("d24ea656-c1c2-4bb5-bce4-6e030c8bdf79")
        };
        private static readonly string[] ClassNames = new[] {
        "90DBA",
        "90DBB",
        "90CK1",
        };

        [MapToApiVersion("1.0")]
        //[HttpGet("api/v{version:apiVersion}/class")]
        [HttpGet]
        [ProducesResponseType(typeof(ClassRoom), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var rng = new Random();
            var classes = Enumerable.Range(1, 5).Select(index => new ClassRoom
            {
                Id = ClassIds[rng.Next(ClassIds.Length)],
                ClassName = ClassNames[rng.Next(ClassNames.Length)],
                Number = 0
            });

            return Ok(classes);
        }
    }
    public class ClassRoom
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; }
        public int Number { get; set; }
    }
}