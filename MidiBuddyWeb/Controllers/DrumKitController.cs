using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MidiBuddyWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrumKitController : ControllerBase
    {
        
        [HttpGet]
        public ActionResult GetDrumkits()
        {
            
            return new JsonResult(Encoding.UTF8.GetString(Properties.Resources.map));
        }
    }
}
