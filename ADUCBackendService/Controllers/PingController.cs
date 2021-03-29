using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ADUCBackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        // GET: api/Account/5
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok(new { message = "ADUCBackendService is running" });
        }
    }
}