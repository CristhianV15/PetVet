using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecepcionFacturas.Models
{
    public class Orden
    {
        // Primary key de la tabla orden
        public int IdOrden { get; set; }

        // Foreign key de la factura
        public int IdFactura { get; set; }
        public virtual Factura Factura { get; set; }

        // Archivos relacionados con la orden
        public byte[] OrdenDeServicioPDF { get; set; }
        public HttpPostedFileBase ArchivoOrdenDeServicio { get; set; }

        public virtual ICollection<OrdenActa> OrdenesActas { get; set; }
    }
}