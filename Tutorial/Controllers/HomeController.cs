using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Tutorial.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Metodo para probar el direccionamiento
        /// </summary>
        /// <returns>Una pagina html con contenido estático</returns>
        [Produces("text/html")]
        [HttpGet("index")]
        public IActionResult Index()
        {
            return File("~/test.html", "text/html");
        }
    }
}