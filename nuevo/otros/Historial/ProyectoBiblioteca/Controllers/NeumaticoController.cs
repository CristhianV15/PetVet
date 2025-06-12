using Newtonsoft.Json;
using ProyectoBiblioteca.Logica;
using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoBiblioteca.Controllers
{
    public class NeumaticoController : Controller
    {
        // GET: Biblioteca
        
        public ActionResult Vehiculodet()
        {
            return View();
        }

        public ActionResult Vehiculo()
        {
            return View();
        }

        public ActionResult Llanta()
        {
            return View();
        }

        public ActionResult Marca()
        {
            return View();
        }

        public ActionResult Modelo()
        {
            return View();
        }

        public ActionResult Medida()
        {
            return View();
        }

        public ActionResult Almacen()
        {
            return View();
        }
        // Vehiculo ---------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public JsonResult ListarVehiculos()
        {
            List<Vehiculo> oLista = new List<Vehiculo>();
            oLista = VehiculoLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarVehiculo(Vehiculo objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.id == 0) ? VehiculoLogica.Instancia.Registrar(objeto) : VehiculoLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarVehiculo(int id)
        {
            bool respuesta = false;
            respuesta = VehiculoLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        // Vehiculo Detalle ---------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public JsonResult ListarVehiculodet()
        {
            List<Vehiculodet> oLista = new List<Vehiculodet>();
            oLista = VehiculodetLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarVehiculodet(Vehiculodet objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.iddet == 0) ? VehiculodetLogica.Instancia.Registrar(objeto) : VehiculodetLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarVehiculodet(int id)
        {
            bool respuesta = false;
            respuesta = VehiculodetLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        // Llantas ---------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public JsonResult ListarLlanta()
        {
            List<Llantas> oLista = new List<Llantas>();
            oLista = LlantaLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarLlanta(Llantas objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.idllanta == 0) ? LlantaLogica.Instancia.Registrar(objeto) : LlantaLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarLlanta(int id)
        {
            bool respuesta = false;
            respuesta = LlantaLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        // Marca ---------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public JsonResult ListarMarca()
        {
            List<Marca> oLista = new List<Marca>();
            oLista = MarcaLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.idmarca == 0) ? MarcaLogica.Instancia.Registrar(objeto) : MarcaLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = false;
            respuesta = MarcaLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        // Modelo ---------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public JsonResult ListarModelo()
        {
            List<Modelo> oLista = new List<Modelo>();
            oLista = ModeloLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarModelo(Modelo objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.idmodelo == 0) ? ModeloLogica.Instancia.Registrar(objeto) : ModeloLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarModelo(int id)
        {
            bool respuesta = false;
            respuesta = ModeloLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        // Medida ---------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public JsonResult ListarMedida()
        {
            List<Medida> oLista = new List<Medida>();
            oLista = MedidaLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarMedida(Medida objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.idmedida == 0) ? MedidaLogica.Instancia.Registrar(objeto) : MedidaLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarMedida(int id)
        {
            bool respuesta = false;
            respuesta = MedidaLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        // Almacen ---------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public JsonResult ListarAlmacen()
        {
            List<Almacen> oLista = new List<Almacen>();
            oLista = AlmacenLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarAlmacen(Almacen objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.idalmacen == 0) ? AlmacenLogica.Instancia.Registrar(objeto) : AlmacenLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarAlmacen(int id)
        {
            bool respuesta = false;
            respuesta = AlmacenLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }


        // Tipo Persona -----------------------------------------------------------------------------------------------


        [HttpGet]
        public JsonResult ListarTipoPersona()
        {
            List<TipoPersona> oLista = new List<TipoPersona>();
            oLista = TipoPersonaLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListarPersona()
        {
            List<Persona> oLista = new List<Persona>();

            oLista = PersonaLogica.Instancia.Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarPersona(Persona objeto)
        {
            bool respuesta = false;
            objeto.Clave = objeto.Clave == null ? "" : objeto.Clave;
            respuesta = (objeto.IdPersona == 0) ? PersonaLogica.Instancia.Registrar(objeto) : PersonaLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarPersona(int id)
        {
            bool respuesta = false;
            respuesta = PersonaLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }


    }
    public class Response
    {

        public bool resultado { get; set; }
        public string mensaje { get; set; }
    }
}