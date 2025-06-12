using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecepcionFacturas.Models
{
    public class OrdenActa
    {
        // Primary key de la tabla intermedia
        public int Id { get; set; }

        // Foreign key de la orden
        public int IdOrden { get; set; }
        public virtual Orden Orden { get; set; }

        // Foreign key del acta de conformidad
        public int IdActa { get; set; }
        public virtual ActaConformidad ActaConformidad { get; set; }
    }
}