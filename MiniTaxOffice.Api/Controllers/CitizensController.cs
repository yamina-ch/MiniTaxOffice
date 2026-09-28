using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MiniTaxOffice.Api.Models;
using MiniTaxOffice.Api.Data;


namespace MiniTaxOffice.Api.Controllers
{

    // =========================================================
    // RUTA DEL CONTROLADOR
    // =========================================================

    /*
        Como nuestro controlador se llama:

        CitizensController

        [controller] se convierte automáticamente en:

        Citizens


        Por tanto:

        [Route("api/[controller]")]

        significa:

        /api/Citizens
    */

    [Route("api/[controller]")]
    [ApiController]

    public class CitizensController : ControllerBase
    {


        // =====================================================
        // ❌ ANTES — LISTA TEMPORAL
        // =====================================================

        /*
            ANTES todavía no teníamos base de datos.

            Por eso guardábamos nuestros ciudadanos
            dentro de una List<Citizen>.


            PROBLEMA:

            Los datos solamente existían mientras
            la aplicación estaba ejecutándose.

            Si cerrábamos ASP.NET:

            ciudadanos → desaparecían.


            Dejamos este código comentado
            para poder repasarlo después.
        */


        /*
        private static readonly List<Citizen> citizens =
            new List<Citizen>
            {

                new Citizen
                {
                    Id = 1,
                    Name = "Yamina",
                    Dni = "X1234567A",
                    Email = "yamina@email.com",
                    City = "Alicante"
                },


                new Citizen
                {
                    Id = 2,
                    Name = "Rama",
                    Dni = "X1235476A",
                    Email = "rama@gmail.com",
                    City = "Alicante"
                }

            };
        */



        // =====================================================
        // ✅ AHORA — APPDBCONTEXT
        // =====================================================

        /*
            Ahora tenemos una base de datos REAL.


            CitizensController
                    ↓
               AppDbContext
                    ↓
            Entity Framework Core
                    ↓
               SQL Server
                    ↓
            MiniTaxOfficeDb
                    ↓
                Citizens


            _context será el objeto que utilizaremos
            para trabajar con nuestra base de datos.
        */

        private readonly AppDbContext _context;



        // =====================================================
        // CONSTRUCTOR
        // =====================================================

        /*
            ASP.NET crea AppDbContext automáticamente.

            Esto funciona porque en Program.cs pusimos:

            builder.Services.AddDbContext<AppDbContext>(...);


            ASP.NET utiliza:

            DEPENDENCY INJECTION

            para entregarnos el contexto.
        */

        public CitizensController(AppDbContext context)
        {

            // Guardamos el contexto recibido
            // para poder utilizarlo en todo el Controller.

            _context = context;

        }



        // =====================================================
        // GET — OBTENER TODOS LOS CIUDADANOS
        // =====================================================


        // -----------------------------------------------------
        // ❌ ANTES — LISTA
        // -----------------------------------------------------

        /*
        [HttpGet]
        public IActionResult Get()
        {

            // Simplemente devolvíamos nuestra lista.

            return Ok(citizens);

        }
        */



        // -----------------------------------------------------
        // ✅ AHORA — BASE DE DATOS
        // -----------------------------------------------------

        /*
            GET

            /api/Citizens


            Ejemplo:

            https://localhost:7278/api/Citizens


            Devuelve TODOS los ciudadanos
            guardados en SQL Server.
        */

        [HttpGet]

        public async Task<IActionResult> Get()
        {

            // =============================================
            // 1. CONSULTAR LA TABLA
            // =============================================

            /*
                _context

                representa nuestra base de datos.


                _context.Citizens

                representa la tabla:

                Citizens


                ToListAsync()

                ejecuta aproximadamente:

                SELECT *
                FROM Citizens;


                y convierte los resultados
                en una List<Citizen>.
            */

            List<Citizen> citizens =
                await _context.Citizens.ToListAsync();



            // =============================================
            // 2. DEVOLVER RESULTADO
            // =============================================

            /*
                ASP.NET convierte automáticamente
                la lista a JSON.

                HTTP:

                200 OK
            */

            return Ok(citizens);

        }



        // =====================================================
        // GET POR ID
        // =====================================================


        // -----------------------------------------------------
        // ❌ ANTES — LISTA
        // -----------------------------------------------------

        /*
        [HttpGet("{id}")]

        public IActionResult GetById(int id)
        {

            Citizen? citizen =
                citizens.FirstOrDefault(c => c.Id == id);


            if (citizen == null)
            {

                return NotFound();

            }


            return Ok(citizen);

        }
        */



        // -----------------------------------------------------
        // ✅ AHORA — BASE DE DATOS
        // -----------------------------------------------------

        /*
            GET

            /api/Citizens/2


            Significa:

            dame el ciudadano cuyo Id sea 2.
        */

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {

            // =============================================
            // 1. BUSCAR EN SQL SERVER
            // =============================================

            /*
                Buscamos directamente en la tabla Citizens.

                FirstOrDefaultAsync busca el primer ciudadano
                que cumpla:

                    c.Id == id


                Por ejemplo:

                    id = 2


                aproximadamente sería:

                    SELECT *
                    FROM Citizens
                    WHERE Id = 2;
            */

            Citizen? citizen =
                await _context.Citizens
                    .FirstOrDefaultAsync(c => c.Id == id);



            // =============================================
            // 2. COMPROBAR SI EXISTE
            // =============================================

            if (citizen == null)
            {

                // Si no existe:
                //
                // HTTP 404 Not Found

                return NotFound();

            }



            // =============================================
            // 3. DEVOLVER CIUDADANO
            // =============================================

            // HTTP 200 OK

            return Ok(citizen);

        }



        // =====================================================
        // POST — CREAR CIUDADANO
        // =====================================================


        // -----------------------------------------------------
        // ❌ ANTES — LISTA
        // -----------------------------------------------------

        /*
        [HttpPost]

        public IActionResult Create(Citizen citizen)
        {

            // Antes teníamos que generar nosotros el ID.

            citizen.Id = citizens.Max(c => c.Id) + 1;


            // Después añadíamos el objeto a la lista.

            citizens.Add(citizen);


            return CreatedAtAction(
                nameof(GetById),
                new { id = citizen.Id },
                citizen
            );

        }
        */



        // -----------------------------------------------------
        // ✅ AHORA — SQL SERVER
        // -----------------------------------------------------

        /*
            POST

            /api/Citizens


            Vue enviará por ejemplo:


            {
                "name": "Samira",
                "dni": "Y7654321H",
                "email": "samira@email.com",
                "city": "Elche"
            }


            IMPORTANTE:

            Ya NO generamos nosotros el Id.


            SQL Server tiene:

            IDENTITY(1,1)


            Por tanto SQL Server generará:

            1
            2
            3
            4
            ...
        */

        [HttpPost]

        public async Task<IActionResult> Create(Citizen citizen)
        {

            // =============================================
            // 1. AÑADIR AL CONTEXTO
            // =============================================

            /*
                Le decimos a Entity Framework:

                "Quiero añadir este Citizen
                 a la tabla Citizens."


                TODAVÍA no se ha guardado
                físicamente en SQL Server.
            */

            _context.Citizens.Add(citizen);



            // =============================================
            // 2. GUARDAR EN SQL SERVER
            // =============================================

            /*
                ESTA LÍNEA ES MUY IMPORTANTE.


                SaveChangesAsync()

                guarda realmente los cambios
                en la base de datos.


                Aproximadamente EF genera:

                INSERT INTO Citizens
                (Name, Dni, Email, City)

                VALUES
                (...);
            */

            await _context.SaveChangesAsync();



            // =============================================
            // 3. RESPUESTA
            // =============================================

            /*
                Devolvemos:

                HTTP 201 Created


                Además indicamos que el ciudadano
                recién creado puede consultarse en:

                /api/Citizens/{id}
            */

            return CreatedAtAction(

                nameof(GetById),

                new
                {
                    id = citizen.Id
                },

                citizen

            );

        }



        // =====================================================
        // PUT — EDITAR CIUDADANO
        // =====================================================


        // -----------------------------------------------------
        // ❌ ANTES — LISTA
        // -----------------------------------------------------

        /*
        [HttpPut("{id}")]

        public IActionResult Update(
            int id,
            Citizen updatedCitizen
        )
        {

            // Buscábamos en la lista.

            Citizen? citizen =
                citizens.FirstOrDefault(c => c.Id == id);


            if (citizen == null)
            {

                return NotFound();

            }


            // Modificábamos el objeto de memoria.

            citizen.Name = updatedCitizen.Name;

            citizen.Dni = updatedCitizen.Dni;

            citizen.Email = updatedCitizen.Email;

            citizen.City = updatedCitizen.City;


            return Ok(citizen);

        }
        */



        // -----------------------------------------------------
        // ✅ AHORA — SQL SERVER
        // -----------------------------------------------------

        /*
            PUT

            /api/Citizens/2


            Por ejemplo Vue envía:


            {
                "name": "Rahma",
                "dni": "X1235476A",
                "email": "rama@gmail.com",
                "city": "Alicante"
            }


            Aquí NO creamos otro ciudadano.

            Buscamos el que ya existe y
            modificamos sus propiedades.
        */

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(
            int id,
            Citizen updatedCitizen
        )
        {

            // =============================================
            // 1. BUSCAR EL CIUDADANO ORIGINAL
            // =============================================

            /*
                Si recibimos:

                PUT /api/Citizens/2


                buscamos:

                Id = 2
            */

            Citizen? citizen =
                await _context.Citizens
                    .FirstOrDefaultAsync(c => c.Id == id);



            // =============================================
            // 2. COMPROBAR SI EXISTE
            // =============================================

            if (citizen == null)
            {

                // No existe.
                //
                // HTTP 404

                return NotFound();

            }



            // =============================================
            // 3. MODIFICAR SUS DATOS
            // =============================================

            /*
                citizen

                = objeto que YA existe en SQL Server.


                updatedCitizen

                = nuevos datos enviados desde Vue.
            */


            // Nombre nuevo

            citizen.Name = updatedCitizen.Name;


            // DNI nuevo

            citizen.Dni = updatedCitizen.Dni;


            // Email nuevo

            citizen.Email = updatedCitizen.Email;


            // Ciudad nueva

            citizen.City = updatedCitizen.City;



            // =============================================
            // 4. GUARDAR CAMBIOS
            // =============================================

            /*
                Como Entity Framework está siguiendo
                el objeto "citizen", detectará que
                hemos cambiado sus propiedades.


                SaveChangesAsync genera aproximadamente:

                UPDATE Citizens

                SET
                    Name = ...,
                    Dni = ...,
                    Email = ...,
                    City = ...

                WHERE Id = ...;
            */

            await _context.SaveChangesAsync();



            // =============================================
            // 5. DEVOLVER RESULTADO
            // =============================================

            // HTTP 200 OK
            //
            // devolvemos el ciudadano actualizado.

            return Ok(citizen);

        }



        // =====================================================
        // DELETE — ELIMINAR CIUDADANO
        // =====================================================


        // -----------------------------------------------------
        // ❌ ANTES — LISTA
        // -----------------------------------------------------

        /*
        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {

            Citizen? citizen =
                citizens.FirstOrDefault(c => c.Id == id);


            if (citizen == null)
            {

                return NotFound();

            }


            // Lo eliminábamos solamente
            // de nuestra lista en memoria.

            citizens.Remove(citizen);


            return NoContent();

        }
        */



        // -----------------------------------------------------
        // ✅ AHORA — SQL SERVER
        // -----------------------------------------------------

        /*
            DELETE

            /api/Citizens/3


            Eliminará de SQL Server
            el ciudadano con Id = 3.
        */

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {

            // =============================================
            // 1. BUSCAR CIUDADANO
            // =============================================

            Citizen? citizen =
                await _context.Citizens
                    .FirstOrDefaultAsync(c => c.Id == id);



            // =============================================
            // 2. COMPROBAR SI EXISTE
            // =============================================

            if (citizen == null)
            {

                // HTTP 404

                return NotFound();

            }



            // =============================================
            // 3. MARCAR PARA ELIMINAR
            // =============================================

            /*
                Le decimos a Entity Framework:

                "Quiero eliminar este Citizen."


                Todavía necesitamos SaveChangesAsync().
            */

            _context.Citizens.Remove(citizen);



            // =============================================
            // 4. GUARDAR EN SQL SERVER
            // =============================================

            /*
                Ahora sí se ejecuta aproximadamente:

                DELETE FROM Citizens
                WHERE Id = ...;
            */

            await _context.SaveChangesAsync();



            // =============================================
            // 5. RESPUESTA
            // =============================================

            /*
                HTTP 204 No Content

                Significa:

                operación correcta,
                pero no devolvemos contenido.
            */

            return NoContent();

        }

    }

}