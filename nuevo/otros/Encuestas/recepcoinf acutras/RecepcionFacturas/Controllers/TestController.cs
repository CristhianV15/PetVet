//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Dapper;


//namespace RecepcionFacturas.Controllers
//{
//    public class ProbandoController : Controller
//    {
//        // Definir la cadena de conexión a la base de datos
//        private readonly string connectionString = "cadenaLocal"; // Reemplaza con tu cadena de conexión

//        // Método para probar la conexión a la base de datos
//        public ActionResult ProbarConexion()
//        {
//            try
//            {
//                // Abrir una conexión a la base de datos
//                using (var connection = new SqlConnection(connectionString))
//                {
//                    // Ejecutar una consulta simple para verificar la conexión
//                    connection.Open();
//                    var resultado = connection.QueryFirstOrDefault<string>("SELECT 1");

//                    // Si la consulta se ejecuta correctamente, mostrar mensaje de éxito
//                    if (resultado != null)
//                    {
//                        ViewBag.Message = "Conexión a la base de datos exitosa.";
//                    }
//                    else
//                    {
//                        ViewBag.Message = "Error al ejecutar la consulta.";
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                // Capturar y mostrar cualquier excepción relacionada con la conexión
//                ViewBag.Message = "Error de conexión: " + ex.Message;
//            }

//            return View();
//        }
//    }
//}




using System;
using System.Data.SqlClient;
using System.Web.Mvc;
namespace RecepcionFacturas.Controllers
{
    public class TestController : Controller
    {
        public ActionResult TestConexion()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["cadenaPublica"].ConnectionString))
                {
                    connection.Open();
                    ViewBag.Message = "Success connect to the database ";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Fail in the connection" + ex;
            }
            return View("~/Views/Test/TestConexion.cshtml"); // Ruta completa a la vista
        }
    }
}