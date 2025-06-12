using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecepcionFacturas.Models
{
    public class FacturaViewModel
    {
        // Propiedades para la Factura
        public int IdFactura { get; set; }

        [Required(ErrorMessage = "El número de factura es obligatorio")]
        [Display(Name = "Número de Factura")]
        public string NumeroFactura { get; set; }

        [Required(ErrorMessage = "La fecha de emisión es obligatoria")]
        [Display(Name = "Fecha de Emisión")]
        public DateTime FechaEmision { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
        [Display(Name = "Fecha de Vencimiento")]
        public DateTime FechaVencimiento { get; set; }

        [Required(ErrorMessage = "La moneda es obligatoria")]
        public string Moneda { get; set; }

        //Para insertar monedas pero desde tabla moneda
        [Required(ErrorMessage = "La moneda es obligatoria")]
        [Display(Name = "Moneda")]
        public int IdMoneda { get; set; }
        [Display(Name = "Nombre de la Moneda")]
        public string Nombre { get; set; }


        ////public string MonedaNombre { get; set; } // Para mostrar el nombre de la moneda
        //[Required(ErrorMessage = "La moneda es obligatoria")]
        //[Display(Name = "Moneda")]
        //public int IdMoneda { get; set; }



        [Required(ErrorMessage = "El importe total es obligatorio")]
        [Display(Name = "Importe Total")]
        public decimal ImporteTotal { get; set; }

        // Propiedades para archivos PDF
        [Required(ErrorMessage = "Seleccione un archivo de factura")]
        [Display(Name = "Archivo de Factura")]
        public HttpPostedFileBase ArchivoFactura { get; set; }

        [Required(ErrorMessage = "Seleccione un archivo de orden de compra")]
        [Display(Name = "Archivo de Orden de Compra")]
        public HttpPostedFileBase ArchivoOrden { get; set; }

        [Required(ErrorMessage = "Seleccione un archivo de guía de remisión")]
        [Display(Name = "Archivo de Guía de Remisión")]
        public HttpPostedFileBase ArchivoGuia { get; set; }

        [Required(ErrorMessage = "Seleccione un archivo de acta de conformidad")]
        [Display(Name = "Archivo de Acta de Conformidad")]
        public HttpPostedFileBase ArchivoActa { get; set; }

        // Propiedades para los bytes de los archivos PDF
        public byte[] FacturaPDF { get; set; }
        public byte[] OrdenPDF { get; set; }
        public byte[] GuiaRemisionPDF { get; set; }
        public byte[] ActaConformidadPDF { get; set; }

        // Lista de monedas para el dropdown
      //  public List<SelectListItem> Monedas { get; set; }

        // Lista de monedas para el dropdown
        public IEnumerable<SelectListItem> Monedas { get; set; }
        public IEnumerable<SelectListItem> idMonedas { get; set; }


        //enun para monedas
        public moned FacturaMon { get; set;   }
        public enum moned
        {   Pen,
            Dol,
            Eur
        }
        //public int IdFactura { get; set; }
        //public string NumeroFactura { get; set; }
        //public DateTime FechaEmision { get; set; }
        //public DateTime FechaVencimiento { get; set; }
        //public string Moneda { get; set; }
        //public decimal ImporteTotal { get; set; }
        //public HttpPostedFileBase ArchivoFactura { get; set; }
        //public HttpPostedFileBase ArchivoFacturaXML { get; set; }
        //public HttpPostedFileBase ArchivoOrden { get; set; }
        //public HttpPostedFileBase ArchivoActa { get; set; }
        //public HttpPostedFileBase ArchivoGuia { get; set; }



    }
}