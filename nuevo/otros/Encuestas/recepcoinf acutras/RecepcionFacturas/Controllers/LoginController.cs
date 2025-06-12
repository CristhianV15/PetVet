using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Digests; 
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Macs;
using System.Text;
using RecepcionFacturas.Models;
using System.Collections.Generic;
using System.Configuration;

namespace RecepcionFacturas.Controllers
{
    public class LoginController : Controller
    {


        public ActionResult Login(Usuario usuario)
        {
            try
            {
                // Validar las credenciales en la base de datos
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["cadenaLocal"].ConnectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM Usuario WHERE Nombre = @NombreUsuario AND pass = @Password";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NombreUsuario", usuario.Nombre);
                        command.Parameters.AddWithValue("@Password", usuario.Pass); // Sin encriptación

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Inicio de sesión exitoso
                                // Redirigir a la página correspondiente según el rol del usuario
                                // ...
                                return View("~/Views/Factura/VisualizarArchivo.cshtml");
                                ViewBag.Mensaje = "Inicio de sesión exitoso";
                            }
                            else
                            {
                                // Credenciales incorrectas
                                ViewBag.Mensaje = "Usuario o contraseña incorrectos";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al iniciar sesión: " + ex.Message;
            }


            return View("~/Views/Factura/VisualizarArchivo.cshtml");
            //   return View("~/Views/Login/Login.cshtml");
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
        //Metodo para obtener facturas y guardarlos en una lista
        private List<Factura> ObtenerFacturas()
        {
            List<Factura> facturas = new List<Factura>();
            string cadenaSQL = ConfigurationManager.ConnectionStrings["cadenaLocal"].ConnectionString;

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
        //public ActionResult Login(Usuario usuario)
        //{
        //    try
        //    {
        //        // Encriptar la contraseña del usuario
        //        string passwordEncriptada = EncriptarPassword(usuario.Password);

        //        // Validar las credenciales en la base de datos
        //        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["prueba"].ConnectionString))
        //        {
        //            connection.Open();

        //            string sql = "SELECT * FROM Usuario WHERE Nombre = @NombreUsuario AND Password = @PasswordEncriptada";
        //            using (SqlCommand command = new SqlCommand(sql, connection))
        //            {
        //                command.Parameters.AddWithValue("@NombreUsuario", usuario.Nombre);
        //                command.Parameters.AddWithValue("@PasswordEncriptada", passwordEncriptada);

        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        // Inicio de sesión exitoso
        //                        // Redirigir a la página correspondiente según el rol del usuario
        //                        // ...

        //                        ViewBag.Mensaje = "Inicio de sesión exitoso";
        //                    }
        //                    else
        //                    {
        //                        // Credenciales incorrectas
        //                        ViewBag.Mensaje = "Usuario o contraseña incorrectos";
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Mensaje = "Error al iniciar sesión: " + ex.Message;
        //    }

        //    return View("~/Views/Login/Login.cshtml");
        //}

        //private string EncriptarPassword(string password)
        //{
        //    // Ejemplo de encriptación con SHA256
        //    using (var sha256 = new Sha256Digest())
        //    {
        //        var digest = new byte[sha256.GetDigestSize()];
        //        sha256.ComputeHash(Encoding.UTF8.GetBytes(password), digest);

        //        return Convert.ToBase64String(digest);
        //    }
        //}
    }
}



    //// variable String para la conexión declarada en el web config, modificar si es cadenaLocal o de la nube
    //private readonly string connectionString = "cadenaLocal";

    //// Método para la acción "Index" (página de login)
    //public ActionResult Index()
    //{
    //    return View();
    //}

    //// Formulario de Login
    //[HttpPost]
    //public ActionResult Login(string usuario, string password)
    //{
    //    // Abriendo la conexión a la base de datos
    //    using (var connection = new SqlConnection(connectionString))
    //    {
    //        // Consulta para el logeo 
    //        string sql = @"
    //            SELECT *
    //            FROM Usuario
    //            WHERE nombre = @usuario AND pass = @password";

    //        //parametros para el usuario
    //        //planificar si abra otra variable para el tipo de usuario
    //        var parameters = new DynamicParameters();
    //        parameters.Add("@usuario", usuario);
    //        parameters.Add("@password", password);

    //        var usuarioEncontrado = connection.QueryFirstOrDefault<Usuario>(sql, parameters);

    //        // Verificar si el usuario existe y la contraseña es correcta
    //        if (usuarioEncontrado != null)
    //        {
    //            // Iniciar sesión del usuario (opcional)
    //            // Redireccionar a la página principal 
    //            return RedirectToAction("Index", "Home"); // Pagina principal
    //        }
    //        else
    //        {
    //            // Mensaje de error (usuario o contraseña incorrectos)
    //            ViewBag.Error = "Usuario o contraseña incorrectos.";
    //            return View("Index");
    //        }
    //    }
    //}







    



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