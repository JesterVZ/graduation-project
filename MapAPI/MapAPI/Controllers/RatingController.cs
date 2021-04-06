using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace MapAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Home> Get()
        {
            var random = new Random();
            return Enumerable.Range(1, 20).Select(index => new Home
            {
                coordinates = new double[2] { 
                    random.NextDouble() * (60-50)+50, 
                    random.NextDouble() * (60 - 50) + 50 
                },
                rating = random.Next(0, 100)
            })
            .ToArray();
        }
    }
}
