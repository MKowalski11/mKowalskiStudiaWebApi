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
            //obiekt result ustawi ErrorLog na "Error"(+treść) w przypadku błędnego stringu zawierającego formułę
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
                    infix = result.InfixTokensArray,
                    rpn = result.PostfixTokensArray
                };
                return Ok(data);
            }
        }
        [HttpGet]
        [Produces("application/json")]
        [Route("calculate")]
        public IActionResult Get(string formula, string X)
        {
            string stan = "ok";
            double tmpDouble;
            WebApi.RPNclass result = new WebApi.RPNclass(formula);
            //ErrorLog będzie miał "Error" na początku stringa jeśli wystąpi problem
            if (result.ErrorLog[0] == 'E' && result.ErrorLog[1] == 'r' && result.ErrorLog[2] == 'r')
            {
                var data = new
                {
                    status = "error",
                    message = result.ErrorLog
                };
                return Ok(data);
            }
            //Sprawdzanie X, czy da się zparseować do double.
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
                string wynik = RPNclass.PostfixCalcSingleX(result.PostfixTokensArray, tmpDouble);
                //wynik zostanie zwrócony jako string - albo liczba/wynik albo error code w przypadku niedozwolonej operacji/wyjątku co należy zweryfikować
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
            //sprawdzamy czy udało się rozbić na tokeny, jeśli nie to dlaczego
            if (wynik.ErrorLog[0] == 'E' && wynik.ErrorLog[1] == 'r' && wynik.ErrorLog[2] == 'r')
            {
                var data = new
                {
                    status = "error",
                    message = wynik.ErrorLog
                };
                return Ok(data);
            }
            //sprawdzanie liczb podanych przez url
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
                //PostfixCalcMultiXCheck powie nam czy wystąpił błąd przy którymkolwiek z Xów podanych w zakresie
                bool flagaErr = false;
                flagaErr = RPNclass.PostfixCalcMultiXCheck(wynik.PostfixTokensArray,  tmpDouble,  tmpDouble2,  tmpInt);
                //jeśli tak, zwracamy double X, string Y - który może być liczbą albo treścią błędu dla danego X
                //dane takie nadal mogą być użyte po odfiltrowaniu z nich wpisów z poza dziedziny X
                //zabezpieczenie przed próbą potraktowania np 2/X przy X=0 (2/0) jako liczbę rzeczywstą 
                if (flagaErr)
                {
                    WynikError[] result = RPNclass.PostfixCalcMultiXWynikError(wynik.PostfixTokensArray, tmpDouble, tmpDouble2, tmpInt);
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
                    //jeśli jakiegokolwiek błędu nie stwierdzono, wyniki zwracane w postaci double X, double Y, gotowe do dalszego przetwarzania
                    Wynik[] result = RPNclass.PostfixCalcMultiXWynik(wynik.PostfixTokensArray, tmpDouble, tmpDouble2, tmpInt);
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
