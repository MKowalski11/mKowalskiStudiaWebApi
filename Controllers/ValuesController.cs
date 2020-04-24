using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi;

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
            WebApi.RPNclass result = new WebApi.RPNclass(formula);
            if (result.ErrorLog[0] == 'E' && result.ErrorLog[1] == 'r' && result.ErrorLog[2] == 'r')
            {
                var data = new
                {
                    status = "error",
                    message = result.ErrorLog
                };
                return Ok(data);
            }
            else
            {
                var data = new
                {
                    status = stan,
                    //formula = formula
                    infix = result.Infix_Tokens_Array,
                    rpn = result.Postfix_Tokens_Array
                    
                };
                return Ok(data);
            }
            //return Ok(data);
        }
        [HttpGet]
        [Produces("application/json")]
        [Route("calculate")]
        public IActionResult Get(string formula, string X)
        {
            string stan = "ok";
            double tmpDouble;
            WebApi.RPNclass result = new WebApi.RPNclass(formula);
            if (result.ErrorLog[0] == 'E' && result.ErrorLog[1] == 'r' && result.ErrorLog[2] == 'r')
            {
                var data = new
                {
                    status = "error",
                    message = result.ErrorLog
                };
                return Ok(data);
            }
            else if (double.TryParse(X, out tmpDouble) != true)
            {
                var data = new
                {
                    status = "error",
                    message = "Error: 'X' is not a valid double number or has not been set"
                };
                return Ok(data);
            }
            else {
                string wynik = RPNclass.PostfixCalcSingleX(result.Postfix_Tokens_Array, tmpDouble);
                if (double.TryParse(wynik, out tmpDouble) != true)
                {
                    var data = new
                    {
                        status = "error",
                        message = wynik
                    };
                    return Ok(data);
                }
                else
                {
                    var data = new
                    {
                        status = stan,
                        result = tmpDouble
                    };
                    return Ok(data);
                }
            }
        }
        [HttpGet]
        [Produces("application/json")]
        [Route("calculate/xy")]
       
        public IActionResult Get(string formula, string from, string to, string n)
        {
            double tmpDouble, tmpDouble2;
            int tmpInt;
            WebApi.RPNclass wynik = new WebApi.RPNclass(formula);
            if (wynik.ErrorLog[0] == 'E' && wynik.ErrorLog[1] == 'r' && wynik.ErrorLog[2] == 'r')
            {
                var data = new
                {
                    status = "error",
                    message = wynik.ErrorLog
                };
                return Ok(data);
            }
            else if (double.TryParse(from, out tmpDouble) != true)
            {
                var data = new
                {
                    status = "error",
                    message = "Error: 'from' is not a valid double number or has not been set"
                };
                return Ok(data);
            }
            else if (double.TryParse(to, out tmpDouble2) != true)
            {
                var data = new
                {
                    status = "error",
                    message = "Error: 'to' is not a valid double number or has not been set"
                };
                return Ok(data);
            }
            else if (Int32.TryParse(n, out tmpInt) != true)
            {
                var data = new
                {
                    status = "error",
                    message = "Error: 'n' is not a valid integer number or has not been set"
                };
                return Ok(data);
            }
            else
            {
                bool flagaErr = false;
                flagaErr = RPNclass.PostfixCalcMultiXCheck(wynik.Postfix_Tokens_Array,  tmpDouble,  tmpDouble2,  tmpInt);
                if (flagaErr)
                {
                    WynikError[] result = RPNclass.PostfixCalcMultiXWynikError(wynik.Postfix_Tokens_Array, tmpDouble, tmpDouble2, tmpInt);
                    var data = new
                    {
                        status = "error",
                        message = "Error: for some 'x', errors have occured",
                        result = result
                    };
                    return Ok(data);
                }
                else
                {
                    Wynik[] result = RPNclass.PostfixCalcMultiXWynik(wynik.Postfix_Tokens_Array, tmpDouble, tmpDouble2, tmpInt);
                    var data = new
                    {
                        status = "ok",
                        result  =  result
                    };
                    return Ok(data);
                }
            }
        }
    }
}
