using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecepcionFacturas.Models
{
    public class Prueba
    {
        //Primary key de la tabla prueba
        public int IdFactura { get; set; }

        //Factura pdf
        public byte[] FacturaPDF { get; set; } //Almacena la información en bytes
        public string NumeroFactura { get; set; } //Almacena el número de la factura para descargar o operaciones
        public HttpPostedFileBase ArchivoFactura { get; set; } //Archivo de la factura

        public byte[] OrdenCompraPDF { get; set; } //Almacena la información en bytes
        public HttpPostedFileBase ArchivoOrden { get; set; } //Aarchivo de la orden de compra


        public byte[] FusionPDF { get; set; } //Almacena la información del los pdf ingresados (Factura y orden de compra)
              
    }
}