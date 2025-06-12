using RecepcionFacturas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace RecepcionFacturas.Controllers
{
    public class GuiaRemisionController : Controller
    {
        string cadenaSQL = ConfigurationManager.ConnectionStrings["cadenaLocal"].ConnectionString;

        [HttpGet]
        public ActionResult SubidaArchivos(int idFactura)
        {
            ViewBag.IdFactura = idFactura;
            ViewBag.Guias = ObtenerGuias(idFactura);
            return View();
        }

        [HttpPost]
        public ActionResult SubidaArchivos(int idFactura, GuiaRemision model)
        {
            if (ModelState.IsValid &&
                model.ArchivoGuiaRemision != null && model.ArchivoGuiaRemision.ContentLength > 0)
            {
                try
                {
                    using (var binaryReader = new BinaryReader(model.ArchivoGuiaRemision.InputStream))
                    {
                        model.GuiaRemisionPDF = binaryReader.ReadBytes(model.ArchivoGuiaRemision.ContentLength);
                    }

                    using (SqlConnection conexion = new SqlConnection(cadenaSQL))
                    {
                        conexion.Open();
                        string sql = "INSERT INTO GuiaRemision (IdFactura, GuiaPDF) VALUES (@idFactura, @guiaRemisionPDF)";
                        SqlCommand comando = new SqlCommand(sql, conexion);
                        comando.Parameters.AddWithValue("@idFactura", idFactura);
                        comando.Parameters.AddWithValue("@guiaRemisionPDF", model.GuiaRemisionPDF);
                        comando.ExecuteNonQuery();

                        ViewBag.Mensaje = "Guía de remisión subida correctamente";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Mensaje = "Error al subir la guía de remisión: " + ex.Message;
                }
            }
            else
            {
                ViewBag.Mensaje = "Por favor, seleccione un archivo válido.";
            }

            ViewBag.IdFactura = idFactura;
            ViewBag.Guias = ObtenerGuias(idFactura);
            return View();
        }

        private List<GuiaRemision> ObtenerGuias(int idFactura)
        {
            List<GuiaRemision> guias = new List<GuiaRemision>();

            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
            {
                conexion.Open();
                string sql = "SELECT IdGuia, GuiaPDF FROM GuiaRemision WHERE IdFactura = @idFactura";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.AddWithValue("@idFactura", idFactura);

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        guias.Add(new GuiaRemision
                        {
                            IdGuia = reader.GetInt32(0),
                            GuiaRemisionPDF = reader[1] as byte[]
                        });
                    }
                }
            }

            return guias;
        }
    }
}



//namespace RecepcionFacturas.Controllers
//{
//    public class GuiaRemisionController : Controller
//    {
//        string cadenaSQL = ConfigurationManager.ConnectionStrings["cadenaLocal"].ConnectionString;

//        [HttpGet]
//        public ActionResult SubidaArchivos(int idOrden)
//        {
//            ViewBag.IdOrden = idOrden;
//            ViewBag.Guias = ObtenerGuias(idOrden);
//            return View();
//        }

//        [HttpPost]
//        public ActionResult SubidaArchivos(GuiaRemision model)
//        {
//            if (ModelState.IsValid &&
//                model.ArchivoGuiaRemision != null && model.ArchivoGuiaRemision.ContentLength > 0)
//            {
//                try
//                {
//                    using (var binaryReader = new BinaryReader(model.ArchivoGuiaRemision.InputStream))
//                    {
//                        model.GuiaRemisionPDF = binaryReader.ReadBytes(model.ArchivoGuiaRemision.ContentLength);
//                    }

//                    using (SqlConnection conexion = new SqlConnection(cadenaSQL))
//                    {
//                        conexion.Open();
//                        string sql = "INSERT INTO GuiaRemision (IdOrden, GuiaRemisionPDF) VALUES (@idOrden, @guiaRemisionPDF)";
//                        SqlCommand comando = new SqlCommand(sql, conexion);
//                        comando.Parameters.AddWithValue("@idOrden", model.IdOrden);
//                        comando.Parameters.AddWithValue("@guiaRemisionPDF", model.GuiaRemisionPDF);
//                        comando.ExecuteNonQuery();

//                        ViewBag.Mensaje = "Guía de remisión subida correctamente";
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ViewBag.Mensaje = "Error al subir la guía de remisión: " + ex.Message;
//                }
//            }
//            else
//            {
//                ViewBag.Mensaje = "Por favor, seleccione un archivo válido.";
//            }

//            ViewBag.Guias = ObtenerGuias(model.IdOrden);
//            return View();
//        }

//        private List<GuiaRemision> ObtenerGuias(int idOrden)
//        {
//            List<GuiaRemision> guias = new List<GuiaRemision>();

//            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
//            {
//                conexion.Open();
//                string sql = "SELECT IdGuiaRemision, GuiaRemisionPDF FROM GuiaRemision WHERE IdOrden = @idOrden";
//                SqlCommand comando = new SqlCommand(sql, conexion);
//                comando.Parameters.AddWithValue("@idOrden", idOrden);

//                using (SqlDataReader reader = comando.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        guias.Add(new GuiaRemision
//                        {
//                            IdGuia = reader.GetInt32(0),
//                            GuiaRemisionPDF = reader[1] as byte[]
//                        });
//                    }
//                }
//            }

//            return guias;
//        }
//    }
//}

//public class GuiaRemisionController : Controller
//{
//    // GET: GuiaRemision
//    public ActionResult Index()
//    {
//        return View();
//    }
//}
