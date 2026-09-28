// ==========================================================
// IMPORTS
// ==========================================================

// Necesitamos Entity Framework Core para poder utilizar DbContext
// y DbSet.
using Microsoft.EntityFrameworkCore;

// Necesitamos Models porque aquí utilizaremos nuestra clase Citizen.
using MiniTaxOffice.Api.Models;


namespace MiniTaxOffice.Api.Data
{
    // ======================================================
    // APP DB CONTEXT
    // ======================================================

    /*
        AppDbContext representa nuestra BASE DE DATOS
        dentro de C#.

        Podemos imaginarlo así:

        AppDbContext
              ↓
        Base de datos
              ↓
        Tablas
    */
    public class AppDbContext : DbContext
    {


        // ==================================================
        // CONSTRUCTOR
        // ==================================================

        /*
            Este constructor recibe la configuración
            necesaria para conectarnos a la base de datos.

            Más adelante Program.cs enviará aquí:

            - qué servidor SQL utilizamos
            - qué base de datos utilizamos
            - la cadena de conexión
        */
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }


        // ==================================================
        // TABLA CITIZENS
        // ==================================================

        /*
            DbSet<Citizen> representa una tabla.

            Citizen
            ↓
            es nuestro modelo C#

            Citizens
            ↓
            será la colección de ciudadanos
            almacenados en la base de datos.


            Podemos imaginar:

            C#                         SQL SERVER

            Citizen.cs       →        Citizens

            Id               →        Id
            Name             →        Name
            Dni              →        Dni
            Email            →        Email
            City             →        City
        */
        public DbSet<Citizen> Citizens { get; set; }
    }
}