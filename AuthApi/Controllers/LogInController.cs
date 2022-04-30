using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogInController : Controller
    {
        [HttpGet]
        public ObjectResult LogIn(string login, string pass)
        {
           if(DBrequests.IsUserExists(login,pass))
                return StatusCode(200, "OK");

            return StatusCode(203, "USER NOT FOUND");
        }
    }
}

