using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecepcionFacturas.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Pass { get; set; }
        public int IdProveedor { get; set; } // Relación con la tabla Proveedor (puede ser null)
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }

        // Propiedades de navegación (opcional)
        public Proveedor Proveedor { get; set; } // Navegación hacia el proveedor al que pertenece (si existe)
        public bool RememberMe { get; set; }

    }
}