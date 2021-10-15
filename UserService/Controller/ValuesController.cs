using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;

namespace UserService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public ValuesController(AuthenticationContext context)
        {

        }

        [HttpGet]
        public ActionResult<String> Get()
        {
            return "Bleh !!! This work too :O";
        }
    }
}
