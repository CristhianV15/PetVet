using RecepcionFacturas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;


namespace RecepcionFacturas.Controllers
{
    public class FormController : Controller
    {
        //Cadena conexión a la base de datos, modificar el string en caso se mude a la base de datos externa
        string cadenaSQL = ConfigurationManager.ConnectionStrings["cadenaLocal"].ConnectionString;

        //Metodo para mostrar los datos de la tabla "prueba" 
        [HttpGet]
        public ActionResult SubidaArchivos()
        {
            ViewBag.Facturas = ObtenerFacturas(); //Retorna en la variable Facturas, lo devuelto en el metodo ObtenerFacturas()
            return View();
        }

        [HttpPost]
        public ActionResult SubidaArchivos(Prueba model)
        {
            if (ModelState.IsValid && //Validar si el modelo solicitado (prueba) es valido 
                model.ArchivoFactura != null && model.ArchivoFactura.ContentLength > 0 && //Verifica si el archivo de la factura ha sido seleccionado y si contiene datos
                model.ArchivoOrden != null && model.ArchivoOrden.ContentLength > 0) //Verifica si el archivo de la orden ha sido seleccionado y si contiene datos
            {
                try
                {
                    // Convertir la factura en un arreglo de bytes 
                    using (var binaryReader = new BinaryReader(model.ArchivoFactura.InputStream))
                    {
                        model.FacturaPDF = binaryReader.ReadBytes(model.ArchivoFactura.ContentLength);
                    }

                    // Convertir la orden de compra en un arreglo de bytes
                    using (var binaryReader = new BinaryReader(model.ArchivoOrden.InputStream))
                    {
                        model.OrdenCompraPDF = binaryReader.ReadBytes(model.ArchivoOrden.ContentLength);
                    }

                    // Mediante el método crear la fusión del pdf y guardarlo como atributo para el modelo de prueba          
               //     model.FusionPDF = FusionarPDFs(model.FacturaPDF, model.OrdenCompraPDF); //Requiere el byte[] de ambos archivos

                    //Hace uso de la conexión creada al inicio (no modificar aqui)
                    using (SqlConnection conexion = new SqlConnection(cadenaSQL))
                    {
                        conexion.Open();
                        //Consulta para ingresar los archivos pdf con su respectivo nombre (No aplica el nombre e fusion PDF)
                        string sql = "INSERT INTO tablaPrueba (FacturaPdf, OrdenCompraPDF, NumeroFactura) VALUES (@facturaPdf, @ordenPdf, @numeroFactura)";

                        //Ejecutar la consulta y pasar como parametros los atributos creados en el modelo 
                        SqlCommand comando = new SqlCommand(sql, conexion);
                        comando.Parameters.AddWithValue("@facturaPdf", model.FacturaPDF);
                        comando.Parameters.AddWithValue("@ordenPdf", model.OrdenCompraPDF);
                        //comando.Parameters.AddWithValue("@fusionPdf", model.FusionPDF);
                        comando.Parameters.AddWithValue("@numeroFactura", model.ArchivoFactura.FileName);
                        comando.ExecuteNonQuery();

                        ViewBag.Mensaje = "Archivos subidos correctamente";
                    }
                }
                catch (Exception ex)
                {
                    //Manejo de errores
                    ViewBag.Mensaje = "Error al subir archivos: " + ex.Message;
                }
            }
            else
            {
                ViewBag.Mensaje = "Por favor, seleccione un archivo válido.";
            }

            // Actualizar las facturas luego de la inserción de datos
            ViewBag.Facturas = ObtenerFacturas();

            return View();
        }

        //Metodo que devuelve los datos no pertenecientes a los archivos PDF de la tabla Prueba
        private List<Prueba> ObtenerFacturas()
        {
            //Lista que almacena las facturas,se utiliza en el metodo principal para evitar la falta de instancia del modelo Prueba
            List<Prueba> facturas = new List<Prueba>();

            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
            {
                conexion.Open();
                //Se seleccionan los datos que se requieran de la tabla Prueba (Menos los de descarga de archivos) 
                string sql = "SELECT IdFactura, NumeroFactura FROM tablaPrueba";
                SqlCommand comando = new SqlCommand(sql, conexion);

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        facturas.Add(new Prueba
                        {
                            //Se añaden los datos del modelo Prueba a su List<Prueba> facturas 
                            IdFactura = reader.GetInt32(0),
                            NumeroFactura = reader.IsDBNull(1) ? null : reader.GetString(1),
                        });
                    }
                }
            }

            return facturas;
        }

        //private byte[] FusionarPDFs(byte[] archivo1, byte[] archivo2)
        //{
        //    using (var output = new MemoryStream())
        //    {
        //        PdfDocument outputDocument = new PdfDocument();
        //        PdfDocument inputDocument1 = PdfReader.Open(new MemoryStream(archivo1), PdfDocumentOpenMode.Import);
        //        PdfDocument inputDocument2 = PdfReader.Open(new MemoryStream(archivo2), PdfDocumentOpenMode.Import);
        //        for (int i = 0; i < inputDocument1.PageCount; i++)
        //        {
        //            PdfPage page = inputDocument1.Pages[i];
        //            outputDocument.AddPage(page);
        //        }
        //        for (int i = 0; i < inputDocument2.PageCount; i++)
        //        {
        //            PdfPage page = inputDocument2.Pages[i];
        //            outputDocument.AddPage(page);
        //        }
        //        outputDocument.Save(output);
        //        return output.ToArray();
        //    }
        //}
        [HttpPost]
        public ActionResult DescargarSeleccionados(int id, bool? seleccionarFactura, bool? seleccionarOrden, string numeroFactura)
        {
            byte[] factura = null;
            byte[] orden = null;
           
            List<byte[]> archivosSeleccionados = new List<byte[]>();
            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
            {
                conexion.Open();
                if (seleccionarFactura == true)
                {
                    string sqlFactura = "SELECT FacturaPdf FROM tablaPrueba WHERE IdFactura = @id";
                    SqlCommand comandoFactura = new SqlCommand(sqlFactura, conexion);
                    comandoFactura.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = comandoFactura.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            factura = (byte[])reader["FacturaPdf"];
                            archivosSeleccionados.Add(factura);
                        }
                    }
                }


                if (seleccionarOrden == true)
                {
                    string sqlOrden = "SELECT OrdenCompraPDF FROM tablaPrueba WHERE IdFactura = @id";
                    SqlCommand comandoOrden = new SqlCommand(sqlOrden, conexion);
                    comandoOrden.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = comandoOrden.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            orden = (byte[])reader["OrdenCompraPDF"];
                            archivosSeleccionados.Add(orden);
                        }
                    }
                }
            }
            if (archivosSeleccionados.Count > 0)
            {
                byte[] archivoFusionado = FusionarPDFs(archivosSeleccionados.ToArray());
                return File(archivoFusionado, "application/pdf", "D_" + id + numeroFactura+".pdf");
            }
            else
            {
                return HttpNotFound();
            }
        }
     
        private byte[] FusionarPDFs(byte[][] archivos)
        {
            using (var output = new MemoryStream())
            {
                PdfDocument outputDocument = new PdfDocument();
                foreach (var archivo in archivos)
                {
                    PdfDocument inputDocument = PdfReader.Open(new MemoryStream(archivo), PdfDocumentOpenMode.Import);
                    for (int i = 0; i < inputDocument.PageCount; i++)
                    {
                        PdfPage page = inputDocument.Pages[i];
                        outputDocument.AddPage(page);
                    }
                }
                outputDocument.Save(output);
                return output.ToArray();
            }
        }
    }
}







//namespace RecepcionFacturas.Controllers
//{
//    public class FormController : Controller
//    {
//        string cadenaSQL = ConfigurationManager.ConnectionStrings["cadenaLocal"].ConnectionString;
//        [HttpGet]
//        public ActionResult SubidaArchivos()
//        {
//            ViewBag.Facturas = ObtenerFacturas();
//            return View();
//        }
//        [HttpPost]
//        public ActionResult SubidaArchivos(Prueba model)
//        {
//            if (ModelState.IsValid &&
//                model.ArchivoFactura != null && model.ArchivoFactura.ContentLength > 0 &&
//                model.ArchivoOrden != null && model.ArchivoOrden.ContentLength > 0)
//            {
//                try
//                {
//                    using (var binaryReader = new BinaryReader(model.ArchivoFactura.InputStream))
//                    {
//                        model.FacturaPDF = binaryReader.ReadBytes(model.ArchivoFactura.ContentLength);
//                    }
//                    using (var binaryReader = new BinaryReader(model.ArchivoOrden.InputStream))
//                    {
//                        model.OrdenCompraPDF = binaryReader.ReadBytes(model.ArchivoOrden.ContentLength);
//                    }
//                    model.FusionPDF = FusionarPDFs(model.FacturaPDF, model.OrdenCompraPDF);
//                    using (SqlConnection conexion = new SqlConnection(cadenaSQL))
//                    {
//                        conexion.Open();
//                        string sql = "INSERT INTO tablaPrueba (FacturaPdf, OrdenCompraPDF, FusionPDF, NumeroFactura) VALUES (@facturaPdf, @ordenPdf, @fusionPdf, @numeroFactura)";
//                        SqlCommand comando = new SqlCommand(sql, conexion);
//                        comando.Parameters.AddWithValue("@facturaPdf", model.FacturaPDF);
//                        comando.Parameters.AddWithValue("@ordenPdf", model.OrdenCompraPDF);
//                        comando.Parameters.AddWithValue("@fusionPdf", model.FusionPDF);
//                        comando.Parameters.AddWithValue("@numeroFactura", model.ArchivoFactura.FileName);
//                        comando.ExecuteNonQuery();
//                        ViewBag.Mensaje = "Archivos subidos correctamente";
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ViewBag.Mensaje = "Error al subir archivos: " + ex.Message;
//                }
//            }
//            else
//            {
//                ViewBag.Mensaje = "Por favor, seleccione un archivo válido.";
//            }
//            ViewBag.Facturas = ObtenerFacturas();
//            return View();
//        }
//        private List<Prueba> ObtenerFacturas()
//        {
//            List<Prueba> facturas = new List<Prueba>();
//            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
//            {
//                conexion.Open();
//                string sql = "SELECT IdFactura, NumeroFactura FROM tablaPrueba";
//                SqlCommand comando = new SqlCommand(sql, conexion);
//                using (SqlDataReader reader = comando.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        facturas.Add(new Prueba
//                        {
//                            IdFactura = reader.GetInt32(0),
//                            NumeroFactura = reader.IsDBNull(1) ? null : reader.GetString(1),
//                        });
//                    }
//                }
//            }
//            return facturas;
//        }
//        private byte[] FusionarPDFs(byte[] archivo1, byte[] archivo2)
//        {
//            using (var output = new MemoryStream())
//            {
//                PdfDocument outputDocument = new PdfDocument();
//                PdfDocument inputDocument1 = PdfReader.Open(new MemoryStream(archivo1), PdfDocumentOpenMode.Import);
//                PdfDocument inputDocument2 = PdfReader.Open(new MemoryStream(archivo2), PdfDocumentOpenMode.Import);
//                for (int i = 0; i < inputDocument1.PageCount; i++)
//                {
//                    PdfPage page = inputDocument1.Pages[i];
//                    outputDocument.AddPage(page);
//                }
//                for (int i = 0; i < inputDocument2.PageCount; i++)
//                {
//                    PdfPage page = inputDocument2.Pages[i];
//                    outputDocument.AddPage(page);
//                }
//                outputDocument.Save(output);
//                return output.ToArray();
//            }
//        }
//        [HttpPost]
//        public ActionResult DescargarSeleccionados(int id, bool? seleccionarFactura, bool? seleccionarOrden)
//        {
//            byte[] factura = null;
//            byte[] orden = null;
//            List<byte[]> archivosSeleccionados = new List<byte[]>();
//            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
//            {
//                conexion.Open();
//                if (seleccionarFactura == true)
//                {
//                    string sqlFactura = "SELECT FacturaPdf FROM tablaPrueba WHERE IdFactura = @id";
//                    SqlCommand comandoFactura = new SqlCommand(sqlFactura, conexion);
//                    comandoFactura.Parameters.AddWithValue("@id", id);
//                    using (SqlDataReader reader = comandoFactura.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            factura = (byte[])reader["FacturaPdf"];
//                            archivosSeleccionados.Add(factura);
//                        }
//                    }
//                }
//                if (seleccionarOrden == true)
//                {
//                    string sqlOrden = "SELECT OrdenCompraPDF FROM tablaPrueba WHERE IdFactura = @id";
//                    SqlCommand comandoOrden = new SqlCommand(sqlOrden, conexion);
//                    comandoOrden.Parameters.AddWithValue("@id", id);
//                    using (SqlDataReader reader = comandoOrden.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            orden = (byte[])reader["OrdenCompraPDF"];
//                            archivosSeleccionados.Add(orden);
//                        }
//                    }
//                }
//            }
//            if (archivosSeleccionados.Count > 0)
//            {
//                byte[] archivoFusionado = FusionarPDFs(archivosSeleccionados.ToArray());
//                return File(archivoFusionado, "application/pdf", "Fusion_" + id + ".pdf");
//            }
//            else
//            {
//                return HttpNotFound();
//            }
//        }
//        private byte[] FusionarPDFs(byte[][] archivos)
//        {
//            using (var output = new MemoryStream())
//            {
//                PdfDocument outputDocument = new PdfDocument();
//                foreach (var archivo in archivos)
//                {
//                    PdfDocument inputDocument = PdfReader.Open(new MemoryStream(archivo), PdfDocumentOpenMode.Import);
//                    for (int i = 0; i < inputDocument.PageCount; i++)
//                    {
//                        PdfPage page = inputDocument.Pages[i];
//                        outputDocument.AddPage(page);
//                    }
//                }
//                outputDocument.Save(output);
//                return output.ToArray();
//            }
//        }
//    }
//}