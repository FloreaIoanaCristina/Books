using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Books.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        
    }
}