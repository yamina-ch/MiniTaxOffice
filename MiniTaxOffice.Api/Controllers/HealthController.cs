// Importa funcionalidades relacionadas con HTTP.
// En este controlador realmente no lo necesitamos directamente,
// pero Visual Studio lo añadió automáticamente.
using Microsoft.AspNetCore.Http;

// Importa las herramientas de ASP.NET Core MVC.
// Aquí están ControllerBase, ApiController, Route, HttpGet, IActionResult, etc.
using Microsoft.AspNetCore.Mvc;

namespace MiniTaxOffice.Api.Controllers
{
    // Define la ruta base de este controlador.
    // [controller] toma el nombre de la clase sin la palabra "Controller".
    //
    // HealthController -> Health
    //
    // Por tanto, la ruta será:
    // /api/Health
    [Route("api/[controller]")]

    // Indica que esta clase es un controlador de una API.
    [ApiController]
    public class HealthController : ControllerBase
    {
        // Indica que este método responde a peticiones HTTP GET.
        //
        // GET /api/Health
        [HttpGet]
        public IActionResult Get()
        {
            // Creamos un objeto anónimo con información
            // sobre el estado de nuestra API.
            //
            // Ok(...) genera una respuesta HTTP 200 OK.
            // ASP.NET Core convierte automáticamente
            // este objeto C# a formato JSON.
            return Ok(new
            {
                status = "OK",
                application = "MiniTaxOffice API"
            });
        }
    }
}