using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI1.Controllers
{
    [ApiVersion("1.0")]
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private static readonly string[] Address = new[] {
        "12 bis nguyễn huệ,Q1,TPHCM",
        "135B Trần Hưng Đạo,Q1,TPHCM",
        "39/8 Trần Đình Xu,Q1,TPHCM",
        "1290 Nguyễn Phụng Hiểu,Q2,TP.HCM",
        "18A Nguyễn Thiện Thuật,Q3,TP.HCM",
        "D2/481 Vườn Thơm,Bình Chánh,TPHCM",
        "9 Nguyễn Thị Minh Khai,Q1,TPHCM",
        "2 Nguyễn Huệ,Q1,TPHCM",
        "12/4/6/7 Lê Anh Xuân,Tân Bình,TPHCM",

        };
        private static readonly Guid[] ClassIds = new[] {
            Guid.Parse("7086d3b8-886e-4904-89ec-8cb2e8ae5621"),
            Guid.Parse("e5d926c5-7752-4b43-b491-a98633d21745"),
            Guid.Parse("d24ea656-c1c2-4bb5-bce4-6e030c8bdf79")
        };
        [MapToApiVersion("1.0")]
        //[HttpGet("api/v{version:apiVersion}/students")]
        [HttpGet]
        [ProducesResponseType(typeof(StudentEntity), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var rng = new Random();
            var arrays = Enumerable.Range(1, 5).Select(index => new StudentEntity
            {
                StudentId = Guid.NewGuid(),
                StudentName = Summaries[rng.Next(Summaries.Length)],
                Address = Address[rng.Next(Address.Length)],
                ClassId = ClassIds[rng.Next(ClassIds.Length)],
            })
            .ToArray();

            return Ok(arrays);
        }
    }

    public class StudentEntity
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public string Address { get; set; }
        public Guid ClassId { get; set; }
    }
}