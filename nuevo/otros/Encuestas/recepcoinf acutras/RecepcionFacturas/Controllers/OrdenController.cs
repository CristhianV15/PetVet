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
    public class OrdenController : Controller
    {
        string cadenaSQL = ConfigurationManager.ConnectionStrings["cadenaLocal"].ConnectionString;

        [HttpGet]
        public ActionResult SubidaArchivos(int idFactura)
        {
            ViewBag.IdFactura = idFactura;
            ViewBag.Ordenes = ObtenerOrdenes(idFactura);
            return View();
        }

        [HttpPost]
        public ActionResult SubidaArchivos(Orden model)
        {
            if (ModelState.IsValid &&
                model.ArchivoOrdenDeServicio != null && model.ArchivoOrdenDeServicio.ContentLength > 0)
            {
                try
                {
                    using (var binaryReader = new BinaryReader(model.ArchivoOrdenDeServicio.InputStream))
                    {
                        model.OrdenDeServicioPDF = binaryReader.ReadBytes(model.ArchivoOrdenDeServicio.ContentLength);
                    }

                    using (SqlConnection conexion = new SqlConnection(cadenaSQL))
                    {
                        conexion.Open();
                        string sql = "INSERT INTO Orden (IdFactura, OrdenDeServicioPDF) VALUES (@idFactura, @ordenDeServicioPDF)";

                        SqlCommand comando = new SqlCommand(sql, conexion);
                        comando.Parameters.AddWithValue("@idFactura", model.IdFactura);
                        comando.Parameters.AddWithValue("@ordenDeServicioPDF", model.OrdenDeServicioPDF);
                        comando.ExecuteNonQuery();

                        ViewBag.Mensaje = "Orden subida correctamente";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Mensaje = "Error al subir la orden: " + ex.Message;
                }
            }
            else
            {
                ViewBag.Mensaje = "Por favor, seleccione un archivo válido.";
            }

            ViewBag.Ordenes = ObtenerOrdenes(model.IdFactura);
            return View();
        }

        private List<Orden> ObtenerOrdenes(int idFactura)
        {
            List<Orden> ordenes = new List<Orden>();

            using (SqlConnection conexion = new SqlConnection(cadenaSQL))
            {
                conexion.Open();
                string sql = "SELECT IdOrden, IdFactura FROM Orden WHERE IdFactura = @idFactura";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.AddWithValue("@idFactura", idFactura);

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ordenes.Add(new Orden
                        {
                            IdOrden = reader.GetInt32(0),
                            IdFactura = reader.GetInt32(1)
                        });
                    }
                }
            }

            return ordenes;
        }
    }
}

//public class OrdenController : Controller
//{
//    // GET: Orden
//    public ActionResult Index()
//    {
//        return View();
//    }
//}
