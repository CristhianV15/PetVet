using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using RecepcionFacturas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace RecepcionFacturas.Controllers
{
    public class FacturaController : Controller
    {
        //Cadena conexión a la base de datos, modificar el string en caso se mude a la base de datos externa (Modificar cadena en Web.Config)
        string cadenaSQL = ConfigurationManager.ConnectionStrings["cadenaLocal"].ConnectionString;

        // Acción para mostrar el formulario de ingreso de datos
        [HttpGet]
        public ActionResult IngresarDatos()
        {
            var model = new FacturaViewModel(); //Creamos un modelo que contiene la vista de toda la factura(Modelo: FacturaView Model)
                                                // ViewBag.Monedas = ObtenerMonedas(); //Asignar datos de los nombres de las monedas con el método ObtenerMonedas para verlo en el DropDownList
            ViewBag.Monedas = new SelectList(ObtenerMonedas(), "IdMoneda", "Nombre");
            {
                //model.Monedas = ObtenerMonedas().Select(m => new SelectListItem
                //{
                //    Value = m.IdMoneda.ToString(),
                //    Text = m.Nombre
                //}).ToList();
            }
            return View(model); //Retornar siempre la vista que contiene el modelo de factura 
        }

        // Acción para procesar el formulario de ingreso de datos
        [HttpPost]
        public ActionResult IngresarDatos(FacturaViewModel model) //Para guardar datos en multiples tablas se creo un solo modelo que contiene los campos de la tabla factura y orden
        {
            if (ModelState.IsValid &&  //Validar si el modelo solicitado (FacturaViewModel) es valido 
                model.ArchivoFactura != null && model.ArchivoFactura.ContentLength > 0 && //Verifica si el archivo de la factura ha sido seleccionado y si contiene datos
                model.ArchivoOrden != null && model.ArchivoOrden.ContentLength > 0 && //Verifica si el archivo de la boleta ha sido seleccionado y si contiene datos
                model.ArchivoGuia != null && model.ArchivoGuia.ContentLength > 0 && //Verifica si el archivo de la orden ha sido seleccionado y si contiene datos
                model.ArchivoActa != null && model.ArchivoActa.ContentLength > 0) //Verifica si el archivo de la acta ha sido seleccionado y si contiene datos
            {
                try
                {
                    using (var binaryReader = new BinaryReader(model.ArchivoFactura.InputStream)) // Convertir la factura en un arreglo de bytes 
                    {
                        model.FacturaPDF = binaryReader.ReadBytes(model.ArchivoFactura.ContentLength); //Guardar el contenido en el campo: FacturaPDF
                    }
                    using (var binaryReader = new BinaryReader(model.ArchivoOrden.InputStream)) // Convertir la orden en un arreglo de bytes 
                    {
                        model.OrdenPDF = binaryReader.ReadBytes(model.ArchivoOrden.ContentLength); //Guardar el contenido en el campo: OrdenPDF
                    }
                    using (var binaryReader = new BinaryReader(model.ArchivoGuia.InputStream)) // Convertir la guia en un arreglo de bytes 
                    {
                        model.GuiaRemisionPDF = binaryReader.ReadBytes(model.ArchivoGuia.ContentLength); //Guardar el contenido en el campo: GuiaRemisionPDF
                    }
                    using (var binaryReader = new BinaryReader(model.ArchivoActa.InputStream)) // Convertir la acta en un arreglo de bytes 
                    {
                        model.ActaConformidadPDF = binaryReader.ReadBytes(model.ArchivoActa.ContentLength); //Guardar el contenido en el campo: ActaConformidadPDF
                    }

                    // Guardar en base de datos 
                    using (SqlConnection conexion = new SqlConnection(cadenaSQL))
                    {
                        conexion.Open();
                        //FALTA INSERTAR FACTURA EN FORMTO XML

                        // Query para insertar en tabla Factura
                        string sqlFactura = "INSERT INTO Factura (NumeroFactura, FechaEmision, FechaVencimiento, Moneda, ImporteTotal, FacturaPDF,GuiaRemisionPDF, ActaConformidadPDF) " +
                                            "VALUES (@numeroFactura, @fechaEmision, @fechaVencimiento, @Moneda, @importeTotal, @facturaPdf, @guiaRemisionPDF, @actaConformidadPDF)";
                        SqlCommand comandoFactura = new SqlCommand(sqlFactura, conexion); //Ejecutar consulta "sqlFactura" en la conexión creada al inicio
                        comandoFactura.Parameters.AddWithValue("@numeroFactura", model.NumeroFactura); //Insertar la variable@numeroFactura al modelo -> FacturaViewModel
                        comandoFactura.Parameters.AddWithValue("@fechaEmision", model.FechaEmision); //Insertar la variable@fechaEmision al modelo -> FacturaViewModel
                        comandoFactura.Parameters.AddWithValue("@fechaVencimiento", model.FechaVencimiento); //Insertar la variable@fechaVencimiento al modelo -> FacturaViewModel
                        comandoFactura.Parameters.AddWithValue("@Moneda", model.Moneda); //Insertar la variable@idMoneda al modelo -> FacturaViewModel
                        comandoFactura.Parameters.AddWithValue("@importeTotal", model.ImporteTotal); //Insertar la variable@importeTotal al modelo -> FacturaViewModel
                        comandoFactura.Parameters.AddWithValue("@facturaPdf", model.FacturaPDF); //Insertar la variable@facturaPDF al modelo -> FacturaViewModel
                        comandoFactura.Parameters.AddWithValue("@guiaRemisionPDF", model.GuiaRemisionPDF); //Insertar la variable@guiaRemision al modelo -> FacturaViewModel
                        comandoFactura.Parameters.AddWithValue("@actaConformidadPDF", model.ActaConformidadPDF); //Insertar la variable@actaConformidad al modelo -> FacturaViewModel
                        comandoFactura.ExecuteNonQuery(); //Ejecutar Query ONEPAGE

                        //string sqlIdFactura = "SELECT SCOPE_IDENTITY()"; //Forma anterior para seleccionar el ultimo id de factura 
                        string sqlIdFactura = "SELECT IDENT_CURRENT('Factura')"; //Obtener el ID insertado en Factura mediante consulta 
                        SqlCommand comandoIdFactura = new SqlCommand(sqlIdFactura, conexion); //Ejecutar consulta para almacenar la variable 
                        int idFactura = Convert.ToInt32(comandoIdFactura.ExecuteScalar()); //Guardar ultimo idFactura (PrevioParseoAInt) en "idFactura"

                        // Query para insertar en tabla Orden
                        string sqlOrden = "INSERT INTO Orden (IdFactura, OrdenPDF) VALUES ( @idFactura, @ordenPdf)"; 
                        SqlCommand comandoOrden = new SqlCommand(sqlOrden, conexion); //Ejecutar consulta "sqlOrden en la conexión creada al inicio
                        comandoOrden.Parameters.AddWithValue("@idFactura", idFactura); //Insertar la variable@idFactura al modelo -> FacturaViewModel
                        comandoOrden.Parameters.AddWithValue("@ordenPdf", model.OrdenPDF); //Insertar la variable@ordenPDF al modelo -> FacturaViewModel
                        comandoOrden.ExecuteNonQuery(); //Ejecutar Query

                        ViewBag.Mensaje = "Datos ingresados correctamente";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Mensaje = "Error al ingresar datos: " + ex.Message;
                }
            }
            else
            {
                ViewBag.Mensaje = "Por favor, seleccione todos los archivos válidos.";
            }
            model.Monedas = ObtenerMonedas().Select(m => new SelectListItem
            {
                Value = m.IdMoneda.ToString(),
                Text = m.Nombre
            }).ToList();
            return View(model);
        }
        
        private List<Moneda> ObtenerMonedas()
        {
            List<Moneda> monedas = new List<Moneda>();

            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
            {
                conexion.Open();
                string sql = "SELECT IdMoneda, Nombre FROM Moneda";
                SqlCommand comando = new SqlCommand(sql, conexion);

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        monedas.Add(new Moneda
                        {
                            IdMoneda = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        });
                    }
                }
            }

            return monedas;
        }
        // Acción para mostrar la tabla con datos y opciones de descarga
        [HttpGet]
        public ActionResult VisualizarDatos()
        {
            List<FacturaViewModel> facturas = new List<FacturaViewModel>();

            // Obtener datos de la base de datos para mostrar en la tabla
            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
            {
                conexion.Open();
                string sql = "SELECT f.idFactura, f.NumeroFactura,f.fechaEmision, f.FechaVencimiento,f.Moneda, f.ImporteTotal,f.FacturaPDF," +
                    "f.GuiaRemisionPDF, f.ActaConformidadPDF, f.FechaRegistro FROM factura f LEFT JOIN  Orden o ON f.IdFactura = o.IdFactura; ";
                SqlCommand comando = new SqlCommand(sql, conexion);
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var factura = new FacturaViewModel
                        {
                            IdFactura = reader.GetInt32(0),
                            NumeroFactura = reader.IsDBNull(1) ? null : reader.GetString(1),
                            // Otras propiedades de GuiaRemision y ActaConformidad si se desean mostrar
                        };
                        facturas.Add(factura);
                    }
                }
            }

            return View(facturas);
        }

        //Método para descargar solo archivos seleccionados, parametros ( idDeLaFactura, booleanos para ver si se selecciona la factura,orden,guiaremision o actaConformidad, el númeroFactura se usa para mostrar los archivos con el nombre ingresado)
        [HttpPost]
        public ActionResult DescargarSeleccionados(int id, bool? seleccionarFactura, bool? seleccionarOrden, bool? seleccionarGuiaRemision, bool? seleccionarActaConformidad, string numeroFactura)
        {
            //Inicializar los byte para guardar los pdf en nulo (factura,orden,guiaRemision,actaConformidad)
            byte[] factura = null; 
            byte[] orden = null;
            byte[] guiaRemision = null;
            byte[] actaConformidad = null;
             
            List<byte[]> archivosSeleccionados = new List<byte[]>(); //Lista para almacenar los archivos seleccionados
            using (SqlConnection conexion = new SqlConnection(cadenaSQL)) 
            {
                conexion.Open();
                if (seleccionarFactura == true) //Cuando se seleccione Factura
                {
                    string sqlFactura = "SELECT FacturaPDF FROM Factura WHERE IdFactura = @id"; //Seleccionar el archivo PDF de la factura desde la tabla Factura
                    SqlCommand comandoFactura = new SqlCommand(sqlFactura, conexion);
                    comandoFactura.Parameters.AddWithValue("@id", id); //Añadir el id como parametro para la descarga
                    using (SqlDataReader reader = comandoFactura.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            factura = (byte[])reader["FacturaPdf"]; //Seleccionar FacturaPDF de la base de datos y guardarlo en el byte[] de factura
                            archivosSeleccionados.Add(factura); //Añadir el arreglo de byte[] de factura al byte[] de archivosSeleccionados
                        }
                    }
                }


                if (seleccionarOrden == true)
                {
                    string sqlOrden = "SELECT o.OrdenPDF FROM Factura f INNER JOIN ORDEN o ON f.IdFactura = O.IdFactura where f.IdFactura = @id"; //Seleccionar el archivo PDF de la Orden (Compra o Servicio) desde la tabla Orden con el id de la Factura
                    SqlCommand comandoOrden = new SqlCommand(sqlOrden, conexion);
                    comandoOrden.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = comandoOrden.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            orden = (byte[])reader["OrdenPDF"]; //Seleccionar OrdenPDF de la base de datos y guardarlo en el byte[] de orden
                            archivosSeleccionados.Add(orden); //Añadir el arreglo de byte[] de orden al byte[] de archivosSeleccionados
                        }
                    }
                }

                if (seleccionarGuiaRemision == true)
                {
                    string sqlGuiaRemision = "SELECT GuiaRemisionPDF FROM Factura WHERE IdFactura = @id"; //Seleccionar el archivo PDF de la guiaRemision desde la tabla Factura
                    SqlCommand comandoOrden = new SqlCommand(sqlGuiaRemision, conexion);
                    comandoOrden.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = comandoOrden.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            guiaRemision = (byte[])reader["GuiaRemisionPDF"]; //Seleccionar GuiaRemisionPDF de la base de datos y guardarlo en el byte[] de guiaRemision
                            archivosSeleccionados.Add(guiaRemision); //Añadir el arreglo de byte[] de guiaRemision al byte[] de archivosSeleccionados
                        }
                    }
                }

                if (seleccionarActaConformidad == true)
                {
                    string sqlGuiaRemision = "SELECT ActaConformidadPDF FROM Factura WHERE IdFactura = @id"; //Seleccionar el archivo PDF de la actaConformidad desde la tabla Factura
                    SqlCommand comandoActaConformidad = new SqlCommand(sqlGuiaRemision, conexion);
                    comandoActaConformidad.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = comandoActaConformidad.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            actaConformidad = (byte[])reader["ActaConformidadPDF"]; //Seleccionar ActaConformidadPDF de la base de datos y guardarlo en el byte[] de actaConformidad
                            archivosSeleccionados.Add(actaConformidad); //Anadir el arreglo de byte[] de actaConformidad al byte[] de archivosSeleccionados
                        }
                    }
                }
            }
            if (archivosSeleccionados.Count > 0) //Si se selecciona mas de 1 archivo -> llamar al método Fusionar PDFs
            {
                byte[] archivoFusionado = FusionarPDFs(archivosSeleccionados.ToArray()); //nuevo byte[] para almacenar los archivos seleccionados
                return File(archivoFusionado, "application/pdf", "D_" + id + numeroFactura + ".pdf"); //nombre del archivo fusionado 
            }
            else
            {
                return HttpNotFound(); //CREAR MENSAJE DE ERROR cuando no se seleccionen los archivos
            }
        }

        // Método para fusionar archivos PDF
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
                outputDocument.Save(output, false);
                return output.ToArray();
            }
        }

        //Metodo para obtener facturas y guardarlos en una lista
        private List<Factura> ObtenerFacturas()
        {
            List<Factura> facturas = new List<Factura>();

            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
            {
                conexion.Open();
                string sql = @"SELECT 
                           IdFactura, 
                           NumeroFactura, 
                           FechaEmision, 
                           FechaVencimiento, 
                           Moneda AS MonedaNombre, 
                           ImporteTotal 
                       FROM Factura ";
                SqlCommand comando = new SqlCommand(sql, conexion);

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        facturas.Add(new Factura
                        {
                            IdFactura = reader.GetInt32(0),
                            NumeroFactura = reader.GetString(1),
                            FechaEmision = reader.GetDateTime(2),
                            FechaVencimiento = reader.GetDateTime(3),
                            Moneda = reader.GetString(4),
                            ImporteTotal = reader.GetDecimal(5)
                        });
                    }
                }
            }

            return facturas;
        }

        [HttpGet]
        public ActionResult VisualizarArchivo()
        {
            var facturas = ObtenerFacturas();
            var facturaViewModels = new List<FacturaViewModel>();

            foreach (var factura in facturas)
            {
                facturaViewModels.Add(new FacturaViewModel
                {
                    IdFactura = factura.IdFactura,
                    NumeroFactura = factura.NumeroFactura,
                    FechaEmision = factura.FechaEmision,
                    FechaVencimiento = factura.FechaVencimiento,
                    //IdMoneda = factura.IdMoneda,
                    Moneda = factura.Moneda,
                    ImporteTotal = factura.ImporteTotal
                });
            }

            return View(facturaViewModels);
        }

     

        ////Metodo verificado para mostrar archivos los archivos mediante la lista creada List<Factura>
        //[HttpGet]
        //public ActionResult VisualizarArchivo()
        //{
        //    var facturas = ObtenerFacturas();
        //    var facturaViewModels = new List<FacturaViewModel>();

        //    foreach (var factura in facturas)
        //    {
        //        facturaViewModels.Add(new FacturaViewModel
        //        {
        //            IdFactura = factura.IdFactura,
        //            NumeroFactura = factura.NumeroFactura,
        //            FechaEmision = factura.FechaEmision,
        //            FechaVencimiento = factura.FechaVencimiento,
        //            Moneda = factura.Moneda,
        //            ImporteTotal = factura.ImporteTotal
        //        });
        //    }

        //    return View(facturaViewModels);
        //}

        ////Metodo para obtener facturas y guardarlos en una lista
        //private List<Factura> ObtenerFacturas()
        //{
        //    List<Factura> facturas = new List<Factura>();

        //    using (SqlConnection conexion = new SqlConnection(cadenaSQL))
        //    {
        //        conexion.Open();
        //        string sql = "SELECT IdFactura, NumeroFactura, FechaEmision, FechaVencimiento, m.Nombre, ImporteTotal FROM Factura LEFT JOIN Moneda m  On m.idMoneda = factura.idMoneda";
        //        SqlCommand comando = new SqlCommand(sql, conexion);

        //        using (SqlDataReader reader = comando.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                facturas.Add(new Factura
        //                {
        //                    IdFactura = reader.GetInt32(0),
        //                    NumeroFactura = reader.GetString(1),
        //                    FechaEmision = reader.GetDateTime(2),
        //                    FechaVencimiento = reader.GetDateTime(3),
        //                    Moneda = reader.GetString(4),
        //                    ImporteTotal = reader.GetDecimal(5)
        //                });
        //            }
        //        }
        //    }

        //    return facturas;
        //}

    }
}




        //// Acción para descargar archivos individuales
        //public ActionResult DescargarArchivo(int id, string tipo)
        //{
        //    byte[] archivo = null;
        //    string contentType = "";
        //    string nombreArchivo = "";

        //    // Obtener archivo según tipo (Orden, Guía, Acta)
        //    using (SqlConnection conexion = new SqlConnection(cadenaSQL))
        //    {
        //        conexion.Open();

        //        switch (tipo)
        //        {
        //            case "Orden":
        //                string sqlOrden = "SELECT OrdenPDF FROM Orden WHERE IdFactura = @id";
        //                SqlCommand comandoOrden = new SqlCommand(sqlOrden, conexion);
        //                comandoOrden.Parameters.AddWithValue("@id", id);
        //                using (SqlDataReader reader = comandoOrden.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        archivo = (byte[])reader["OrdenPDF"];
        //                        contentType = "application/pdf";
        //                        nombreArchivo = "OrdenDeServicio.pdf";
        //                    }
        //                }
        //                break;
        //            case "Guia":
        //                string sqlGuia = "SELECT GuiaPDF FROM GuiaRemision WHERE IdFactura = @id";
        //                SqlCommand comandoGuia = new SqlCommand(sqlGuia, conexion);
        //                comandoGuia.Parameters.AddWithValue("@id", id);
        //                using (SqlDataReader reader = comandoGuia.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        archivo = (byte[])reader["GuiaPDF"];
        //                        contentType = "application/pdf";
        //                        nombreArchivo = "GuiaRemision.pdf";
        //                    }
        //                }
        //                break;
        //            case "Acta":
        //                //string sqlActa = "SELECT ActaPDF FROM ActaConformidad WHERE idOrden = @id";
        //                string sqlActa = "SELECT ActaConformidad.ActaPDF, OrdenActa.IdOrden " +
        //                                 "FROM ActaConformidad INNER JOIN OrdenActa ON ActaConformidad.IdActa = OrdenActa.IdActa" +
        //                                 "WHERE OrdenActa.IdOrden = @idOrden";
        //                SqlCommand comandoActa = new SqlCommand(sqlActa, conexion);
        //                comandoActa.Parameters.AddWithValue("@idOrden", id);
        //                using (SqlDataReader reader = comandoActa.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        archivo = (byte[])reader["ActaPDF"];
        //                        contentType = "application/pdf";
        //                        nombreArchivo = "ActaConformidad.pdf";
        //                    }
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    if (archivo != null)
        //    {
        //        return File(archivo, contentType, nombreArchivo);
        //    }
        //    else
        //    {
        //        return HttpNotFound();
        //    }
        //}

        //// Acción para eliminar registros
        //[HttpPost]
        //public ActionResult EliminarRegistro(int id)
        //{
        //    try
        //    {
        //        using (SqlConnection conexion = new SqlConnection(cadenaSQL))
        //        {
        //            conexion.Open();

        //            // Eliminar registros relacionados (Orden, Guia, Acta)
        //            string sqlEliminarOrden = "DELETE FROM Orden WHERE IdFactura = @id";
        //            SqlCommand comandoOrden = new SqlCommand(sqlEliminarOrden, conexion);
        //            comandoOrden.Parameters.AddWithValue("@id", id);
        //            comandoOrden.ExecuteNonQuery();

        //            string sqlEliminarGuia = "DELETE FROM GuiaRemision WHERE IdFactura = @id";
        //            SqlCommand comandoGuia = new SqlCommand(sqlEliminarGuia, conexion);
        //            comandoGuia.Parameters.AddWithValue("@id", id);
        //            comandoGuia.ExecuteNonQuery();

        //            string sqlEliminarActa = "DELETE FROM ActaConformidad WHERE IdFactura = @id";
        //            SqlCommand comandoActa = new SqlCommand(sqlEliminarActa, conexion);
        //            comandoActa.Parameters.AddWithValue("@id", id);
        //            comandoActa.ExecuteNonQuery();

        //            // Eliminar registro de Factura
        //            string sqlEliminarFactura = "DELETE FROM Factura WHERE IdFactura = @id";
        //            SqlCommand comandoFactura = new SqlCommand(sqlEliminarFactura, conexion);
        //            comandoFactura.Parameters.AddWithValue("@id", id);
        //            comandoFactura.ExecuteNonQuery();
        //        }

        //        ViewBag.Mensaje = "Registro eliminado correctamente";
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Mensaje = "Error al eliminar registro: " + ex.Message;
        //    }

        //    // Redireccionar a la vista de visualización de datos
        //    return RedirectToAction("VisualizarDatos");
        //}

        



//Version con 4 tablas 
/*
 using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using RecepcionFacturas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;

namespace RecepcionFacturas.Controllers
{
    public class FacturaController : Controller
    {
        string cadenaSQL = ConfigurationManager.ConnectionStrings["cadenaLocal"].ConnectionString;

        // Acción para mostrar el formulario de ingreso de datos
        [HttpGet]
        public ActionResult IngresarDatos()
        {
            // Inicializamos el modelo para el formulario
            var model = new FacturaViewModel();

            return View(model);
        }

        // Acción para procesar el formulario de ingreso de datos
        [HttpPost]
        public ActionResult IngresarDatos(FacturaViewModel model)
        {
            if (ModelState.IsValid &&
                model.ArchivoFactura != null && model.ArchivoFactura.ContentLength > 0 &&
                model.ArchivoOrden != null && model.ArchivoOrden.ContentLength > 0 &&
                model.ArchivoGuia != null && model.ArchivoGuia.ContentLength > 0 &&
                model.ArchivoActa != null && model.ArchivoActa.ContentLength > 0)
            {
                try
                {
                    // Convertir archivos a bytes
                    using (var binaryReader = new BinaryReader(model.ArchivoFactura.InputStream))
                    {
                        model.FacturaPDF = binaryReader.ReadBytes(model.ArchivoFactura.ContentLength);
                    }
                    using (var binaryReader = new BinaryReader(model.ArchivoOrden.InputStream))
                    {
                        model.OrdenPDF = binaryReader.ReadBytes(model.ArchivoOrden.ContentLength);
                    }
                    using (var binaryReader = new BinaryReader(model.ArchivoGuia.InputStream))
                    {
                        model.GuiaRemisionPDF = binaryReader.ReadBytes(model.ArchivoGuia.ContentLength);
                    }
                    using (var binaryReader = new BinaryReader(model.ArchivoActa.InputStream))
                    {
                        model.ActaConformidadPDF = binaryReader.ReadBytes(model.ArchivoActa.ContentLength);
                    }

                    // Guardar en base de datos
                    using (SqlConnection conexion = new SqlConnection(cadenaSQL))
                    {
                        conexion.Open();

                        // Insertar en tabla Factura
                        string sqlFactura = "INSERT INTO Factura (NumeroFactura, FechaEmision, FechaVencimiento, Moneda, ImporteTotal, FacturaPDF) " +
                                            "VALUES (@numeroFactura, @fechaEmision, @fechaVencimiento, @moneda, @importeTotal, @facturaPdf)";
                        SqlCommand comandoFactura = new SqlCommand(sqlFactura, conexion);
                        comandoFactura.Parameters.AddWithValue("@numeroFactura", model.NumeroFactura);
                        comandoFactura.Parameters.AddWithValue("@fechaEmision", model.FechaEmision);
                        comandoFactura.Parameters.AddWithValue("@fechaVencimiento", model.FechaVencimiento);
                        comandoFactura.Parameters.AddWithValue("@moneda", model.Moneda);
                        comandoFactura.Parameters.AddWithValue("@importeTotal", model.ImporteTotal);
                        comandoFactura.Parameters.AddWithValue("@facturaPdf", model.FacturaPDF);
                        comandoFactura.ExecuteNonQuery();

                        // Obtener el ID insertado en Factura
                        //string sqlIdFactura = "SELECT SCOPE_IDENTITY()";
                        string sqlIdFactura = "SELECT IDENT_CURRENT('Factura')";
                        SqlCommand comandoIdFactura = new SqlCommand(sqlIdFactura, conexion);
                        int idFactura = Convert.ToInt32(comandoIdFactura.ExecuteScalar());

                        // Insertar en tabla Orden
                        string sqlOrden = "INSERT INTO Orden (IdFactura, OrdenPDF) VALUES ( @idFactura, @ordenPdf)";
                        SqlCommand comandoOrden = new SqlCommand(sqlOrden, conexion);
                        comandoOrden.Parameters.AddWithValue("@idFactura", idFactura);
                        comandoOrden.Parameters.AddWithValue("@ordenPdf", model.OrdenPDF);
                        comandoOrden.ExecuteNonQuery();

                        // Insertar en tabla GuiaRemision
                        string sqlGuia = "INSERT INTO GuiaRemision (IdFactura, GuiaPDF) VALUES (@idFactura, @guiaPdf)";
                        SqlCommand comandoGuia = new SqlCommand(sqlGuia, conexion);
                        comandoGuia.Parameters.AddWithValue("@idFactura", idFactura);
                        comandoGuia.Parameters.AddWithValue("@guiaPdf", model.GuiaRemisionPDF);
                        comandoGuia.ExecuteNonQuery();

                        // Insertar en tabla ActaConformidad
                        //Previo a insertar OrdenActa se necesita insertar en ActaConformidad
                        string sqlActa = "INSERT INTO ActaConformidad (ActaPDF) VALUES (@actaPdf)";
                        SqlCommand comandoActa = new SqlCommand(sqlActa, conexion);
                        comandoActa.Parameters.AddWithValue("@actaPdf", model.ActaConformidadPDF);
                        comandoActa.ExecuteNonQuery();

                        //Insertar a tablaOrdenActa
                        string sqlIdOrden = "SELECT IDENT_CURRENT('Orden')"; // Obtener el ID insertado en Orden
                        SqlCommand comandoIdOrden= new SqlCommand(sqlIdOrden, conexion);  //Ejecutar consulta para obtener idOrden
                        int idOrden = Convert.ToInt32(comandoIdOrden.ExecuteScalar()); //Conversión de idOrden 

                        string sqlIdActa = "SELECT IDENT_CURRENT('ActaConformidad')"; //Obtener el ID insertado en la Acta
                        SqlCommand comandoIdActa = new SqlCommand(sqlIdActa, conexion);
                        int idActa = Convert.ToInt32(comandoIdActa.ExecuteScalar()); 
                        string sqlOrdenActa = "INSERT INTO OrdenActa (idOrden,idActa) Values (@idOrden,@idActa)"; //Ejecutar consulta para obtener idActa

                        SqlCommand comandoOrdenActa = new SqlCommand(sqlOrdenActa, conexion);
                        comandoOrdenActa.Parameters.AddWithValue("@idOrden", idOrden); //Insercción de la idOrden 
                        comandoOrdenActa.Parameters.AddWithValue("@idActa", idActa);  //Insercción de la idActa                       
                        comandoOrdenActa.ExecuteNonQuery();

                        ViewBag.Mensaje = "Datos ingresados correctamente";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Mensaje = "Error al ingresar datos: " + ex.Message;
                }
            }
            else
            {
                ViewBag.Mensaje = "Por favor, seleccione todos los archivos válidos.";
            }

            return View(model);
        }

        // Acción para mostrar la tabla con datos y opciones de descarga
        [HttpGet]
        public ActionResult VisualizarDatos()
        {
            List<FacturaViewModel> facturas = new List<FacturaViewModel>();

            // Obtener datos de la base de datos para mostrar en la tabla
            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
            {
                conexion.Open();
                string sql = "SELECT f.IdFactura, f.NumeroFactura, g.IdGuia, a.IdActa " +
                             "FROM Factura f " +
                             "LEFT JOIN GuiaRemision g ON f.IdFactura = g.IdFactura " +
                             "LEFT JOIN ActaConformidad a ON f.IdFactura = a.IdActa";
                SqlCommand comando = new SqlCommand(sql, conexion);
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var factura = new FacturaViewModel
                        {
                            IdFactura = reader.GetInt32(0),
                            NumeroFactura = reader.IsDBNull(1) ? null : reader.GetString(1),
                            // Otras propiedades de GuiaRemision y ActaConformidad si se desean mostrar
                        };
                        facturas.Add(factura);
                    }
                }
            }

            return View(facturas);
        }

        // Acción para descargar archivos fusionados
        [HttpPost]
        public ActionResult DescargarSeleccionados(int id, bool? seleccionarOrden, bool? seleccionarGuia, bool? seleccionarActa)
        {
            byte[] orden = null;
            byte[] guia = null;
            byte[] acta = null;

            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
            {
                conexion.Open();

                if (seleccionarOrden == true)
                {
                    string sqlOrden = "SELECT OrdenDeServicioPDF FROM Orden WHERE IdFactura = @id";
                    SqlCommand comandoOrden = new SqlCommand(sqlOrden, conexion);
                    comandoOrden.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = comandoOrden.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            orden = (byte[])reader["OrdenDeServicioPDF"];
                        }
                    }
                }

                if (seleccionarGuia == true)
                {
                    string sqlGuia = "SELECT GuiaRemisionPDF FROM GuiaRemision WHERE IdFactura = @id";
                    SqlCommand comandoGuia = new SqlCommand(sqlGuia, conexion);
                    comandoGuia.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = comandoGuia.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            guia = (byte[])reader["GuiaRemisionPDF"];
                        }
                    }
                }

                if (seleccionarActa == true)
                {
                    string sqlActa = "SELECT ActaConformidadPDF FROM ActaConformidad WHERE IdFactura = @id";
                    SqlCommand comandoActa = new SqlCommand(sqlActa, conexion);
                    comandoActa.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = comandoActa.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            acta = (byte[])reader["ActaConformidadPDF"];
                        }
                    }
                }
            }

            // Fusionar archivos seleccionados
            if (orden != null || guia != null || acta != null)
            {
                List<byte[]> archivosSeleccionados = new List<byte[]>();
                if (orden != null) archivosSeleccionados.Add(orden);
                if (guia != null) archivosSeleccionados.Add(guia);
                if (acta != null) archivosSeleccionados.Add(acta);

                byte[] archivoFusionado = FusionarPDFs(archivosSeleccionados.ToArray());
                return File(archivoFusionado, "application/pdf", "DocumentosCombinados.pdf");
            }
            else
            {
                return HttpNotFound();
            }
        }
        // Método para fusionar archivos PDF
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
                outputDocument.Save(output, false);
                return output.ToArray();
            }
        }

        // Acción para descargar archivos individuales
        public ActionResult DescargarArchivo(int id, string tipo)
        {
            byte[] archivo = null;
            string contentType = "";
            string nombreArchivo = "";

            // Obtener archivo según tipo (Orden, Guía, Acta)
            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
            {
                conexion.Open();

                switch (tipo)
                {
                    case "Orden":
                        string sqlOrden = "SELECT OrdenPDF FROM Orden WHERE IdFactura = @id";
                        SqlCommand comandoOrden = new SqlCommand(sqlOrden, conexion);
                        comandoOrden.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = comandoOrden.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                archivo = (byte[])reader["OrdenPDF"];
                                contentType = "application/pdf";
                                nombreArchivo = "OrdenDeServicio.pdf";
                            }
                        }
                        break;
                    case "Guia":
                        string sqlGuia = "SELECT GuiaPDF FROM GuiaRemision WHERE IdFactura = @id";
                        SqlCommand comandoGuia = new SqlCommand(sqlGuia, conexion);
                        comandoGuia.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = comandoGuia.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                archivo = (byte[])reader["GuiaPDF"];
                                contentType = "application/pdf";
                                nombreArchivo = "GuiaRemision.pdf";
                            }
                        }
                        break;
                    case "Acta":
                        //string sqlActa = "SELECT ActaPDF FROM ActaConformidad WHERE idOrden = @id";
                        string sqlActa = "SELECT ActaConformidad.ActaPDF, OrdenActa.IdOrden " +
                                         "FROM ActaConformidad INNER JOIN OrdenActa ON ActaConformidad.IdActa = OrdenActa.IdActa" +
                                         "WHERE OrdenActa.IdOrden = @idOrden";
                        SqlCommand comandoActa = new SqlCommand(sqlActa, conexion);
                        comandoActa.Parameters.AddWithValue("@idOrden", id);
                        using (SqlDataReader reader = comandoActa.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                archivo = (byte[])reader["ActaPDF"];
                                contentType = "application/pdf";
                                nombreArchivo = "ActaConformidad.pdf";
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            if (archivo != null)
            {
                return File(archivo, contentType, nombreArchivo);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // Acción para eliminar registros
        [HttpPost]
        public ActionResult EliminarRegistro(int id)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();

                    // Eliminar registros relacionados (Orden, Guia, Acta)
                    string sqlEliminarOrden = "DELETE FROM Orden WHERE IdFactura = @id";
                    SqlCommand comandoOrden = new SqlCommand(sqlEliminarOrden, conexion);
                    comandoOrden.Parameters.AddWithValue("@id", id);
                    comandoOrden.ExecuteNonQuery();

                    string sqlEliminarGuia = "DELETE FROM GuiaRemision WHERE IdFactura = @id";
                    SqlCommand comandoGuia = new SqlCommand(sqlEliminarGuia, conexion);
                    comandoGuia.Parameters.AddWithValue("@id", id);
                    comandoGuia.ExecuteNonQuery();

                    string sqlEliminarActa = "DELETE FROM ActaConformidad WHERE IdFactura = @id";
                    SqlCommand comandoActa = new SqlCommand(sqlEliminarActa, conexion);
                    comandoActa.Parameters.AddWithValue("@id", id);
                    comandoActa.ExecuteNonQuery();

                    // Eliminar registro de Factura
                    string sqlEliminarFactura = "DELETE FROM Factura WHERE IdFactura = @id";
                    SqlCommand comandoFactura = new SqlCommand(sqlEliminarFactura, conexion);
                    comandoFactura.Parameters.AddWithValue("@id", id);
                    comandoFactura.ExecuteNonQuery();
                }

                ViewBag.Mensaje = "Registro eliminado correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al eliminar registro: " + ex.Message;
            }

            // Redireccionar a la vista de visualización de datos
            return RedirectToAction("VisualizarDatos");
        }
    }
}



     */



//Version con 4 tablas (funciona descarga multiple)

//using RecepcionFacturas.Models;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data.SqlClient;
//using System.IO;
//using System.Web;
//using System.Web.Mvc;

//namespace RecepcionFacturas.Controllers
//{
//    public class HomeController : Controller
//    {
//        string cadenaSQL = ConfigurationManager.ConnectionStrings["cadenaLocal"].ConnectionString;

//        // Acción para mostrar el formulario de ingreso de datos
//        [HttpGet]
//        public ActionResult IngresarDatos()
//        {
//            // Inicializamos el modelo para el formulario
//            var model = new FacturaViewModel();

//            return View(model);
//        }

//        // Acción para procesar el formulario de ingreso de datos
//        [HttpPost]
//        public ActionResult IngresarDatos(FacturaViewModel model)
//        {
//            if (ModelState.IsValid &&
//                model.ArchivoFactura != null && model.ArchivoFactura.ContentLength > 0 &&
//                model.ArchivoOrden != null && model.ArchivoOrden.ContentLength > 0 &&
//                model.ArchivoGuia != null && model.ArchivoGuia.ContentLength > 0 &&
//                model.ArchivoActa != null && model.ArchivoActa.ContentLength > 0)
//            {
//                try
//                {
//                    // Convertir archivos a bytes
//                    using (var binaryReader = new BinaryReader(model.ArchivoFactura.InputStream))
//                    {
//                        model.FacturaPDF = binaryReader.ReadBytes(model.ArchivoFactura.ContentLength);
//                    }
//                    using (var binaryReader = new BinaryReader(model.ArchivoOrden.InputStream))
//                    {
//                        model.OrdenCompraPDF = binaryReader.ReadBytes(model.ArchivoOrden.ContentLength);
//                    }
//                    using (var binaryReader = new BinaryReader(model.ArchivoGuia.InputStream))
//                    {
//                        model.GuiaRemisionPDF = binaryReader.ReadBytes(model.ArchivoGuia.ContentLength);
//                    }
//                    using (var binaryReader = new BinaryReader(model.ArchivoActa.InputStream))
//                    {
//                        model.ActaConformidadPDF = binaryReader.ReadBytes(model.ArchivoActa.ContentLength);
//                    }

//                    // Guardar en base de datos
//                    using (SqlConnection conexion = new SqlConnection(cadenaSQL))
//                    {
//                        conexion.Open();

//                        // Insertar en tabla Factura
//                        string sqlFactura = "INSERT INTO Factura (NumeroFactura, FechaEmision, FechaVencimiento, Moneda, ImporteTotal, FacturaPDF) " +
//                                            "VALUES (@numeroFactura, @fechaEmision, @fechaVencimiento, @moneda, @importeTotal, @facturaPdf)";
//                        SqlCommand comandoFactura = new SqlCommand(sqlFactura, conexion);
//                        comandoFactura.Parameters.AddWithValue("@numeroFactura", model.NumeroFactura);
//                        comandoFactura.Parameters.AddWithValue("@fechaEmision", model.FechaEmision);
//                        comandoFactura.Parameters.AddWithValue("@fechaVencimiento", model.FechaVencimiento);
//                        comandoFactura.Parameters.AddWithValue("@moneda", model.Moneda);
//                        comandoFactura.Parameters.AddWithValue("@importeTotal", model.ImporteTotal);
//                        comandoFactura.Parameters.AddWithValue("@facturaPdf", model.FacturaPDF);
//                        comandoFactura.ExecuteNonQuery();

//                        // Obtener el ID insertado en Factura
//                        string sqlIdFactura = "SELECT SCOPE_IDENTITY()";
//                        SqlCommand comandoIdFactura = new SqlCommand(sqlIdFactura, conexion);
//                        int idFactura = Convert.ToInt32(comandoIdFactura.ExecuteScalar());

//                        // Insertar en tabla Orden
//                        string sqlOrden = "INSERT INTO Orden (IdFactura, OrdenDeServicioPDF) VALUES (@idFactura, @ordenPdf)";
//                        SqlCommand comandoOrden = new SqlCommand(sqlOrden, conexion);
//                        comandoOrden.Parameters.AddWithValue("@idFactura", idFactura);
//                        comandoOrden.Parameters.AddWithValue("@ordenPdf", model.OrdenCompraPDF);
//                        comandoOrden.ExecuteNonQuery();

//                        // Insertar en tabla GuiaRemision
//                        string sqlGuia = "INSERT INTO GuiaRemision (IdFactura, GuiaRemisionPDF) VALUES (@idFactura, @guiaPdf)";
//                        SqlCommand comandoGuia = new SqlCommand(sqlGuia, conexion);
//                        comandoGuia.Parameters.AddWithValue("@idFactura", idFactura);
//                        comandoGuia.Parameters.AddWithValue("@guiaPdf", model.GuiaRemisionPDF);
//                        comandoGuia.ExecuteNonQuery();

//                        // Insertar en tabla ActaConformidad
//                        string sqlActa = "INSERT INTO ActaConformidad (ActaConformidadPDF) VALUES (@actaPdf)";
//                        SqlCommand comandoActa = new SqlCommand(sqlActa, conexion);
//                        comandoActa.Parameters.AddWithValue("@actaPdf", model.ActaConformidadPDF);
//                        comandoActa.ExecuteNonQuery();

//                        ViewBag.Mensaje = "Datos ingresados correctamente";
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ViewBag.Mensaje = "Error al ingresar datos: " + ex.Message;
//                }
//            }
//            else
//            {
//                ViewBag.Mensaje = "Por favor, seleccione todos los archivos válidos.";
//            }

//            return View(model);
//        }

//        // Acción para mostrar la tabla con datos y opciones de descarga
//        [HttpGet]
//        public ActionResult VisualizarDatos()
//        {
//            List<FacturaViewModel> facturas = new List<FacturaViewModel>();

//            // Obtener datos de la base de datos para mostrar en la tabla
//            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
//            {
//                conexion.Open();
//                string sql = "SELECT f.IdFactura, f.NumeroFactura, g.IdGuia, a.IdActaConformidad " +
//                             "FROM Factura f " +
//                             "LEFT JOIN GuiaRemision g ON f.IdFactura = g.IdFactura " +
//                             "LEFT JOIN ActaConformidad a ON f.IdFactura = a.IdFactura";
//                SqlCommand comando = new SqlCommand(sql, conexion);
//                using (SqlDataReader reader = comando.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        var factura = new FacturaViewModel
//                        {
//                            IdFactura = reader.GetInt32(0),
//                            NumeroFactura = reader.IsDBNull(1) ? null : reader.GetString(1),
//                            // Otras propiedades de GuiaRemision y ActaConformidad si se desean mostrar
//                        };
//                        facturas.Add(factura);
//                    }
//                }
//            }

//            return View(facturas);
//        }

//        // Acción para descargar archivos fusionados
//        [HttpPost]
//        public ActionResult DescargarSeleccionados(int id, bool? seleccionarOrden, bool? seleccionarGuia, bool? seleccionarActa)
//        {
//            byte[] orden = null;
//            byte[] guia = null;
//            byte[] acta = null;

//            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
//            {
//                conexion.Open();

//                if (seleccionarOrden == true)
//                {
//                    string sqlOrden = "SELECT OrdenDeServicioPDF FROM Orden WHERE IdFactura = @id";
//                    SqlCommand comandoOrden = new SqlCommand(sqlOrden, conexion);
//                    comandoOrden.Parameters.AddWithValue("@id", id);
//                    using (SqlDataReader reader = comandoOrden.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            orden = (byte[])reader["OrdenDeServicioPDF"];
//                        }
//                    }
//                }

//                if (seleccionarGuia == true)
//                {
//                    string sqlGuia = "SELECT GuiaRemisionPDF FROM GuiaRemision WHERE IdFactura = @id";
//                    SqlCommand comandoGuia = new SqlCommand(sqlGuia, conexion);
//                    comandoGuia.Parameters.AddWithValue("@id", id);
//                    using (SqlDataReader reader = comandoGuia.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            guia = (byte[])reader["GuiaRemisionPDF"];
//                        }
//                    }
//                }

//                if (seleccionarActa == true)
//                {
//                    string sqlActa = "SELECT ActaConformidadPDF FROM ActaConformidad WHERE IdFactura = @id";
//                    SqlCommand comandoActa = new SqlCommand(sqlActa, conexion);
//                    comandoActa.Parameters.AddWithValue("@id", id);
//                    using (SqlDataReader reader = comandoActa.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            acta = (byte[])reader["ActaConformidadPDF"];
//                        }
//                    }
//                }
//            }

//            // Fusionar archivos seleccionados
//            if (orden != null || guia != null || acta != null)
//            {
//                List<byte[]> archivosSeleccionados = new List<byte[]>();
//                if (orden != null) archivosSeleccionados.Add(orden);
//                if (guia != null) archivosSeleccionados.Add(guia);
//                if (acta != null) archivosSeleccionados.Add(acta);

//                byte[] archivoFusionado = FusionarPDFs(archivosSeleccionados.ToArray());
//                return File(archivoFusionado, "application/pdf", "DocumentosCombinados.pdf");
//            }
//            else
//            {
//                return HttpNotFound();
//            }
//        }



//using RecepcionFacturas.Models;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data.SqlClient;
//using System.IO;
//using System.Web.Mvc;

//namespace RecepcionFacturas.Controllers
//{
//    public class FacturaController : Controller
//    {
//        string cadenaSQL = ConfigurationManager.ConnectionStrings["cadenaLocal"].ConnectionString;

//        // Método para mostrar la vista de subida de archivos
//        [HttpGet]
//        public ActionResult SubidaArchivos()
//        {
//            ViewBag.Facturas = ObtenerFacturas();
//            return View();
//        }

//        [HttpPost]
//        public ActionResult SubidaArchivos(Factura model)
//        {
//            if (ModelState.IsValid &&
//                model.ArchivoFactura != null && model.ArchivoFactura.ContentLength > 0)
//            {
//                try
//                {
//                    // Convertir el archivo de factura en un arreglo de bytes
//                    using (var binaryReader = new BinaryReader(model.ArchivoFactura.InputStream))
//                    {
//                        model.FacturaPDF = binaryReader.ReadBytes(model.ArchivoFactura.ContentLength);
//                    }

//                    // Convertir el XML en un arreglo de bytes (si se proporciona)
//                    if (model.ArchivoFacturaXML != null && model.ArchivoFacturaXML.ContentLength > 0)
//                    {
//                        using (var binaryReader = new BinaryReader(model.ArchivoFacturaXML.InputStream))
//                        {
//                            model.FacturaXML = binaryReader.ReadBytes(model.ArchivoFacturaXML.ContentLength);
//                        }
//                    }

//                    // Guardar la factura en la base de datos
//                    using (SqlConnection conexion = new SqlConnection(cadenaSQL))
//                    {
//                        conexion.Open();
//                        string sql = "INSERT INTO Factura (NumeroFactura, FechaEmision, FechaVencimiento, Moneda, ImporteTotal, FacturaPDF, FacturaXML) " +
//                                     "VALUES (@numeroFactura, @fechaEmision, @fechaVencimiento, @moneda, @importeTotal, @facturaPDF, @facturaXML)";

//                        SqlCommand comando = new SqlCommand(sql, conexion);
//                        comando.Parameters.AddWithValue("@numeroFactura", model.NumeroFactura);
//                        comando.Parameters.AddWithValue("@fechaEmision", model.FechaEmision);
//                        comando.Parameters.AddWithValue("@fechaVencimiento", model.FechaVencimiento);
//                        comando.Parameters.AddWithValue("@moneda", model.Moneda);
//                        comando.Parameters.AddWithValue("@importeTotal", model.ImporteTotal);
//                        comando.Parameters.AddWithValue("@facturaPDF", model.FacturaPDF);
//                        comando.Parameters.AddWithValue("@facturaXML", model.FacturaXML ?? new byte[0]);
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


//        [HttpGet]
//        public ActionResult SubirFactura()
//        {
//            return View();
//        }

//        [HttpPost]
//        public ActionResult SubirFactura(FacturaViewModel model)
//        {
//            // Lógica para subir archivos y guardar datos en la base de datos
//            // Nota: Esta lógica debe ser implementada según tus necesidades específicas
//            return RedirectToAction("VisualizarArchivo");
//        }

//        [HttpGet]
//        public ActionResult VisualizarArchivo()
//        {
//            var facturas = ObtenerFacturas();
//            var facturaViewModels = new List<FacturaViewModel>();

//            foreach (var factura in facturas)
//            {
//                facturaViewModels.Add(new FacturaViewModel
//                {
//                    IdFactura = factura.IdFactura,
//                    NumeroFactura = factura.NumeroFactura,
//                    FechaEmision = factura.FechaEmision,
//                    FechaVencimiento = factura.FechaVencimiento,
//                    Moneda = factura.Moneda,
//                    ImporteTotal = factura.ImporteTotal
//                });
//            }

//            return View(facturaViewModels);
//        }

//        private List<Factura> ObtenerFacturas()
//        {
//            List<Factura> facturas = new List<Factura>();

//            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
//            {
//                conexion.Open();
//                string sql = "SELECT IdFactura, NumeroFactura, FechaEmision, FechaVencimiento, Moneda, ImporteTotal FROM Factura";
//                SqlCommand comando = new SqlCommand(sql, conexion);

//                using (SqlDataReader reader = comando.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        facturas.Add(new Factura
//                        {
//                            IdFactura = reader.GetInt32(0),
//                            NumeroFactura = reader.GetString(1),
//                            FechaEmision = reader.GetDateTime(2),
//                            FechaVencimiento = reader.GetDateTime(3),
//                            Moneda = reader.GetString(4),
//                            ImporteTotal = reader.GetDecimal(5)
//                        });
//                    }
//                }
//            }

//            return facturas;
//        }
//    }
//}
