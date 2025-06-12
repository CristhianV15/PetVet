using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecepcionFacturas.Models
{
    public class Factura_old
    {
        public int IdFactura { get; set; }
        public int IdProveedor { get; set; } // Relación con la tabla Proveedor
        public int IdSociedad { get; set; } // Relación con la tabla Sociedad
        public string NumeroFactura { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Moneda { get; set; }
        public decimal ImporteTotal { get; set; }
        public byte[] PreRegistroCompletado { get; set; }
        public byte[] FacturaPDF { get; set; }
        public byte[] XMLFactura { get; set; }
        public byte[] OrdenServicioPDF { get; set; }
        public byte[] OrdenCompraPDF { get; set; }
        public byte[] GuiaRemisionPDF { get; set; }
        public byte[] ActaConformidadPDF { get; set; }
        public byte[] PreRegistroPDF { get; set; }
        public string Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Observaciones { get; set; }

        // Propiedades de navegación (opcional)
        public Proveedor Proveedor { get; set; } // Navegación hacia el proveedor
        public Sociedad Sociedad { get; set; } // Navegación hacia la sociedad

    }
}