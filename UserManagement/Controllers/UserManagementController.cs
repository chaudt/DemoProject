using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        public async Task<IActionResult> Get() {
            return Ok("user1,user2,user3");
        }
    }
}