using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecepcionFacturas.Models
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public int IdSociedad { get; set; } // Relación con la tabla Sociedad
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string RUC { get; set; }
        public string RazonSocial { get; set; }

        // Propiedades de navegación (opcional)
        public Sociedad Sociedad { get; set; } // Navegación hacia la sociedad a la que pertenece
    }
}