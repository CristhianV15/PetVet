using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecepcionFacturas.Models
{
    public class ActaConformidad
    {
        // Primary key de la tabla acta de conformidad
        public int IdActa { get; set; }

        // Archivos relacionados con el acta de conformidad
        public byte[] ActaConformidadPDF { get; set; }
        public HttpPostedFileBase ArchivoActaConformidad { get; set; }

        public virtual ICollection<OrdenActa> OrdenesActas { get; set; }
    }
}