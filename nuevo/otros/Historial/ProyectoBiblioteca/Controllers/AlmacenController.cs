using ClosedXML.Excel;
using Newtonsoft.Json;
using ProyectoBiblioteca.Logica;
using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoBiblioteca.Controllers
{
    public class AlmacenController : Controller
    {
        // GET: Almacen
        
        public ActionResult StockTaller()
        {
            return View();
        }
        public ActionResult StockScraps()
        {
            return View();
        }

        public ActionResult StockAuxilio()
        {
            return View();
        }

        public ActionResult Estadollanta()
        {
            return View();
        }

        // Listar Stocks de llantas ---------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public JsonResult ListarAlmacenTaller()
        {
            List<Llantas> oLista = new List<Llantas>();
            oLista = StockLogica.Instancia.ListarTaller();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarAlmacenScraps()
        {
            List<Llantas> oLista = new List<Llantas>();
            oLista = StockLogica.Instancia.ListarScraps();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarAlmacenAuxilio()
        {
            List<Llantas> oLista = new List<Llantas>();
            oLista = StockLogica.Instancia.ListarAuxilio();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarEstadoLlanta()
        {
            List<Llantas> oLista = new List<Llantas>();
            oLista = StockLogica.Instancia.ListarEstado();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetImage(int id)
        {
            string imagePath = @"C:/Imagenes/Scraps/" + id + ".jpg";
            return File(imagePath, "image/jpeg");
        }

        [HttpGet]
        public JsonResult ListarCodllantas()
        {
            List<Codllanta> oLista = new List<Codllanta>();
            oLista = StockLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult Exportarllantastaller()
        {

            DataTable dt = StockLogica.Instancia.Expllantastaller();

            dt.TableName = "Datos";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte de LLantas en Operación " + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }

        [HttpPost]
        public FileResult Exportarllantasauxilio()
        {

            DataTable dt = StockLogica.Instancia.Expllantasauxilio();

            dt.TableName = "Datos";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte de LLantas para Auxilio " + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }

        [HttpPost]
        public FileResult Exportarllantasscraps()
        {

            DataTable dt = StockLogica.Instancia.Expllantasscraps();

            dt.TableName = "Datos";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte de LLantas para Auxilio " + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }

        [HttpPost]
        public FileResult ExportarEstado()
        {

            DataTable dt = StockLogica.Instancia.ExpEstado();

            dt.TableName = "Datos";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte de LLantas cambio o reencauche " + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }
    }
}