using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecepcionFacturas.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        //public ActionResult TestConexion()
        //{
        //    return View("~/Views/Test/TestConexion.cshtml"); // Ruta completa a la vista
        //}

        public ActionResult Login() {
            ViewBag.Message = "Logeo de usuarios";
            //return View("~/Views/Factura/SubirFactura.cshtml");
            return View();
        }

        public ActionResult ListadoSociedades() {
            ViewBag.Message = "Listado de usuarios";
            return View();
        }

        //public ActionResult SubidaArchivos()
        //{
        //    //ViewBag.Message = "Formulario";
        //    return View("~/Views/Formulario/SubidaArchivos.cshtml"); // Ruta completa a la vista
        //}
       

    }
}