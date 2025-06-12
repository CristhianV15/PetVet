using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecepcionFacturas.Models
{
    public class Factura
    {
        // Primary key de la tabla factura
        public int IdFactura { get; set; }

        // Datos de la factura
        public string NumeroFactura { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }  
        public string Moneda { get; set; }
        public decimal ImporteTotal { get; set; }
        public int IdMoneda { get; set; }

        // Archivos relacionados con la factura
        public byte[] FacturaPDF { get; set; }
        public HttpPostedFileBase ArchivoFactura { get; set; }

        public byte[] FacturaXML { get; set; }
        public HttpPostedFileBase ArchivoFacturaXML { get; set; }

        public virtual ICollection<Orden> Ordenes { get; set; }
        public virtual ICollection<GuiaRemision> GuiasRemision { get; set; }
    }
}

    