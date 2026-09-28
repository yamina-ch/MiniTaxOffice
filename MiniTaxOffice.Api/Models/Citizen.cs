using Microsoft.AspNetCore.Razor.Hosting;
using System.Runtime.InteropServices;

namespace MiniTaxOffice.Api.Models
{
    public class Citizen
    {
        // Identificador único del ciudadano
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public string City { get; set; }

       

    }
}
