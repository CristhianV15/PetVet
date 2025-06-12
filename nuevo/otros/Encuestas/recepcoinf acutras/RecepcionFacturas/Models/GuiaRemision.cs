using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecepcionFacturas.Models
{
    public class GuiaRemision
    {
        // Primary key de la tabla guía de remisión
        public int IdGuia { get; set; }

        // Foreign key de la factura
        public int IdFactura { get; set; }
        public virtual Factura Factura { get; set; }

        // Archivos relacionados con la guía de remisión
        public byte[] GuiaRemisionPDF { get; set; }
        public HttpPostedFileBase ArchivoGuiaRemision { get; set; }
    }
}