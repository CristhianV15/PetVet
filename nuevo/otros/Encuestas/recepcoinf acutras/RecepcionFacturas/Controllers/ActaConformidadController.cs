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
    public class ActaConformidadController : Controller
    {
        string cadenaSQL = ConfigurationManager.ConnectionStrings["cadenaLocal"].ConnectionString;

        [HttpGet]
        public ActionResult SubidaArchivos(int idOrden)
        {
            ViewBag.IdOrden = idOrden;
            ViewBag.Actas = ObtenerActas(idOrden);
            return View();
        }

        [HttpPost]
        public ActionResult SubidaArchivos(int idOrden, ActaConformidad model)
        {
            if (ModelState.IsValid &&
                model.ArchivoActaConformidad != null && model.ArchivoActaConformidad.ContentLength > 0)
            {
                try
                {
                    using (var binaryReader = new BinaryReader(model.ArchivoActaConformidad.InputStream))
                    {
                        model.ActaConformidadPDF = binaryReader.ReadBytes(model.ArchivoActaConformidad.ContentLength);
                    }

                    using (SqlConnection conexion = new SqlConnection(cadenaSQL))
                    {
                        conexion.Open();
                        string sql = "INSERT INTO ActaConformidad (ActaPDF) OUTPUT INSERTED.IdActa VALUES (@actaConformidadPDF)";
                        SqlCommand comando = new SqlCommand(sql, conexion);
                        comando.Parameters.AddWithValue("@actaConformidadPDF", model.ActaConformidadPDF);
                        int idActa = (int)comando.ExecuteScalar();

                        string sqlRelacion = "INSERT INTO OrdenActa (IdOrden, IdActa) VALUES (@idOrden, @idActa)";
                        SqlCommand comandoRelacion = new SqlCommand(sqlRelacion, conexion);
                        comandoRelacion.Parameters.AddWithValue("@idOrden", idOrden);
                        comandoRelacion.Parameters.AddWithValue("@idActa", idActa);
                        comandoRelacion.ExecuteNonQuery();

                        ViewBag.Mensaje = "Acta de conformidad subida correctamente";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Mensaje = "Error al subir el acta de conformidad: " + ex.Message;
                }
            }
            else
            {
                ViewBag.Mensaje = "Por favor, seleccione un archivo válido.";
            }

            ViewBag.IdOrden = idOrden;
            ViewBag.Actas = ObtenerActas(idOrden);
            return View();
        }

        private List<ActaConformidad> ObtenerActas(int idOrden)
        {
            List<ActaConformidad> actas = new List<ActaConformidad>();

            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
            {
                conexion.Open();
                string sql = "SELECT ac.IdActa, ac.ActaPDF FROM ActaConformidad ac " +
                             "INNER JOIN OrdenActa oa ON ac.IdActa = oa.IdActa " +
                             "WHERE oa.IdOrden = @idOrden";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.AddWithValue("@idOrden", idOrden);

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        actas.Add(new ActaConformidad
                        {
                            IdActa = reader.GetInt32(0),
                            ActaConformidadPDF = reader[1] as byte[]
                        });
                    }
                }
            }

            return actas;
        }
    }
}

//namespace RecepcionFacturas.Controllers
//{
//    public class ActaConformidadController : Controller
//    {
//        string cadenaSQL = ConfigurationManager.ConnectionStrings["cadenaLocal"].ConnectionString;

//        [HttpGet]
//        public ActionResult SubidaArchivos(int idOrden)
//        {
//            ViewBag.IdOrden = idOrden;
//            ViewBag.Actas = ObtenerActas(idOrden);
//            return View();
//        }

//        [HttpPost]
//        public ActionResult SubidaArchivos(ActaConformidad model)
//        {
//            if (ModelState.IsValid &&
//                model.ArchivoActaConformidad != null && model.ArchivoActaConformidad.ContentLength > 0)
//            {
//                try
//                {
//                    using (var binaryReader = new BinaryReader(model.ArchivoActaConformidad.InputStream))
//                    {
//                        model.ActaConformidadPDF = binaryReader.ReadBytes(model.ArchivoActaConformidad.ContentLength);
//                    }

//                    using (SqlConnection conexion = new SqlConnection(cadenaSQL))
//                    {
//                        conexion.Open();
//                        string sql = "INSERT INTO ActaConformidad (ActaConformidadPDF) OUTPUT INSERTED.IdActa VALUES (@actaConformidadPDF)";
//                        SqlCommand comando = new SqlCommand(sql, conexion);
//                        comando.Parameters.AddWithValue("@actaConformidadPDF", model.ActaConformidadPDF);
//                        int idActa = (int)comando.ExecuteScalar();

//                        string sqlRelacion = "INSERT INTO OrdenActa (IdOrden, IdActa) VALUES (@idOrden, @idActa)";
//                        SqlCommand comandoRelacion = new SqlCommand(sqlRelacion, conexion);
//                        comandoRelacion.Parameters.AddWithValue("@idOrden", model.IdOrden);
//                        comandoRelacion.Parameters.AddWithValue("@idActa", idActa);
//                        comandoRelacion.ExecuteNonQuery();

//                        ViewBag.Mensaje = "Acta de conformidad subida correctamente";
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ViewBag.Mensaje = "Error al subir el acta de conformidad: " + ex.Message;
//                }
//            }
//            else
//            {
//                ViewBag.Mensaje = "Por favor, seleccione un archivo válido.";
//            }

//            ViewBag.Actas = ObtenerActas(model.IdOrden);
//            return View();
//        }

//        private List<ActaConformidad> ObtenerActas(int idOrden)
//        {
//            List<ActaConformidad> actas = new List<ActaConformidad>();

//            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
//            {
//                conexion.Open();
//                string sql = "SELECT ac.IdActa, ac.ActaConformidadPDF FROM ActaConformidad ac " +
//                             "INNER JOIN OrdenActa oa ON ac.IdActa = oa.IdActa " +
//                             "WHERE oa.IdOrden = @idOrden";
//                SqlCommand comando = new SqlCommand(sql, conexion);
//                comando.Parameters.AddWithValue("@idOrden", idOrden);

//                using (SqlDataReader reader = comando.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        actas.Add(new ActaConformidad
//                        {
//                            IdActa = reader.GetInt32(0),
//                            ActaConformidadPDF = reader[1] as byte[]
//                        });
//                    }
//                }
//            }

//            return actas;
//        }
//    }
//}

//public class ActaConformidadController : Controller
//{
//    // GET: ActaConformidad
//    public ActionResult Index()
//    {
//        return View();
//    }
//}
