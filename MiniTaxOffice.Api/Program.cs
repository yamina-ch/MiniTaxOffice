// ======================================================
// IMPORTS
// ======================================================

// Necesitamos Entity Framework Core
// para utilizar SQL Server.
using Microsoft.EntityFrameworkCore;

// Importamos nuestro AppDbContext,
// que está dentro de la carpeta Data.
using MiniTaxOffice.Api.Data;



// ======================================================
// CREAR LA APLICACIÓN
// ======================================================

var builder = WebApplication.CreateBuilder(args);



// ======================================================
// SERVICIOS
// ======================================================

// Permite utilizar Controllers.
//
// Por ejemplo:
//
// CitizensController
builder.Services.AddControllers();


// Swagger.
//
// Nos permite probar nuestra API
// desde el navegador.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// ======================================================
// ENTITY FRAMEWORK + SQL SERVER
// ======================================================

/*
    Registramos AppDbContext dentro de ASP.NET.

    Le estamos diciendo:

    "Cuando necesites acceder a la base de datos,
     utiliza AppDbContext y SQL Server."
*/
builder.Services.AddDbContext<AppDbContext>(options =>
{

    /*
        UseSqlServer indica que utilizaremos SQL Server.

        GetConnectionString("DefaultConnection")

        busca en appsettings.json:

        "ConnectionStrings": {
            "DefaultConnection": "..."
        }
    */
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );

});



/*
    Ahora tenemos esta conexión:

    Program.cs
        ↓
    AppDbContext
        ↓
    DefaultConnection
        ↓
    SQL Server LocalDB
        ↓
    MiniTaxOfficeDb
*/



// ======================================================
// CORS
// ======================================================

/*
    Nuestro frontend Vue funciona en:

    http://localhost:5173

    Pero nuestra API ASP.NET funciona
    en otro puerto.

    CORS permite que Vue pueda hacer:

    GET
    POST
    PUT
    DELETE

    contra nuestra API.
*/
builder.Services.AddCors(options =>
{

    options.AddPolicy("VueApp", policy =>
    {

        policy

            // Dirección donde funciona Vue.
            .WithOrigins("http://localhost:5173")

            // Permitimos las cabeceras HTTP.
            .AllowAnyHeader()

            // Permitimos:
            //
            // GET
            // POST
            // PUT
            // DELETE
            .AllowAnyMethod();

    });

});



// ======================================================
// CONSTRUIR LA APLICACIÓN
// ======================================================

var app = builder.Build();



// ======================================================
// SWAGGER
// ======================================================

if (app.Environment.IsDevelopment())
{

    app.UseSwagger();

    app.UseSwaggerUI();

}



// ======================================================
// MIDDLEWARE
// ======================================================

// Redirige HTTP → HTTPS.
app.UseHttpsRedirection();


// IMPORTANTE:
//
// CORS debe ejecutarse antes
// de Authorization.
app.UseCors("VueApp");


// Autorización.
//
// Más adelante lo utilizaremos cuando hagamos:
//
// USER
// ADMIN
app.UseAuthorization();



// ======================================================
// CONTROLLERS
// ======================================================

// Activa las rutas de nuestros controllers.
//
// Por ejemplo:
//
// /api/Citizens
app.MapControllers();



// ======================================================
// INICIAR API
// ======================================================

app.Run();