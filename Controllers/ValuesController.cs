using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class ONPController: ControllerBase
    {
	    [HttpGet]
        [Produces("application/json")]
	    [Route("tokens")]
        public IActionResult Get(string formula)
        {
            string stan = "ok";
            var data = new
            {
                status = stan,
                formula = formula
            };
            return Ok(data);
        }
        [HttpGet]
        [Produces("application/json")]
        [Route("calculate")]
        public IActionResult Get(string formula, double X)
        {
            string stan = "ok";
            if(double.TryParse())
            var data = new
            {
                status = stan,
                formula = formula,
                X = X
            };
            return Ok(data);
        }
        [HttpGet]
        [Produces("application/json")]
        [Route("calculate/xy")]
       
        public IActionResult Get(string formula, double from, double to, int n)
        {
            string stan = "ok";
            var data = new
            {
                status = stan,
                formula = formula,
                from = from,
                to = to,
                n = n
            };
            return Ok(data);
        }
    }
}
