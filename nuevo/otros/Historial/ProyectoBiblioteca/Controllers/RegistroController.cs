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
using OfficeOpenXml;
using System.Data.Entity;

namespace ProyectoBiblioteca.Controllers
{
    public class RegistroController : Controller
    {
        // GET: reencauche

        public ActionResult Scraps()
        {
            return View();
        }

        public ActionResult Crear()
        {
            return View();
        }

        public ActionResult Reencauche()
        {
            return View();
        }

        public ActionResult ReporteMarca()
        {
            return View();
        }

        // Scraps -------------------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public JsonResult ListarScraps()
        {
            List<Scraps> oLista = new List<Scraps>();
            oLista = ScrapsLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarScraps(string objeto, HttpPostedFileBase imagenArchivo)
        {

            Response oresponse = new Response() { resultado = true, mensaje = "" };

            try
            {
                Scraps oScraps = new Scraps();
                oScraps = JsonConvert.DeserializeObject<Scraps>(objeto);

                string GuardarEnRuta = ConfigurationManager.AppSettings["ruta_imagenes_scraps"];

                oScraps.RutaPortada = GuardarEnRuta;
                oScraps.NombrePortada = "";

                if (!Directory.Exists(GuardarEnRuta))
                    Directory.CreateDirectory(GuardarEnRuta);

                if (oScraps.idscraps == 0)
                {
                    int id = ScrapsLogica.Instancia.Registrar(oScraps);
                    oScraps.idscraps = id;
                    oresponse.resultado = oScraps.idscraps == 0 ? false : true;

                }

                if (imagenArchivo != null && oScraps.idscraps != 0)
                {
                    string extension = Path.GetExtension(imagenArchivo.FileName);
                    GuardarEnRuta = Path.Combine(GuardarEnRuta, oScraps.idscraps.ToString() + extension);
                    oScraps.NombrePortada = oScraps.idscraps.ToString() + extension;

                    imagenArchivo.SaveAs(GuardarEnRuta);

                    oresponse.resultado = ScrapsLogica.Instancia.ActualizarRutaImagen(oScraps);
                }

            }
            catch (Exception e)
            {
                oresponse.resultado = false;
                oresponse.mensaje = e.Message;
            }

            return Json(oresponse, JsonRequestBehavior.AllowGet);
        }

        // Listar Stocks de llantas ---------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public JsonResult ListarAlmacenTaller()
        {
            List<Vehiculo> oLista = new List<Vehiculo>();
            oLista = VehiculoLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarAlmacenScraps()
        {
            List<Vehiculo> oLista = new List<Vehiculo>();
            oLista = VehiculoLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        //Creacion de los resgistros ---------------------------------------------------------------------------------------------------------------


        public JsonResult ListarRemanentes(int id = 0)
        {


            List<CrearRegisto> oListaReencauche = CrearRegistroLogica.Listar();

            if (oListaReencauche != null)
            {
                oListaReencauche = oListaReencauche.Where(x => x.oVehiculo.id == id).ToList();

            }

            return Json(oListaReencauche, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GuardarRemanentes(string Vxml)
        {

            bool respuesta = CrearRegistroLogica.Registrar(Vxml.ToString());

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);

        }

        // Reencauches ---------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public JsonResult ListarReencauche()
        {
            List<Reencauche> oLista = new List<Reencauche>();
            oLista = ReencaucheLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarReencauche(ReencaucheMod Objeto)
        {
            bool respuesta = false;
            ReencaucheLogica.Instancia.Modificar(Objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarReencauche(int id)
        {
            bool respuesta = false;
            respuesta = ReencaucheLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        // Reportes ---------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public JsonResult ReporteRemanentes()
        {
            ReporteR objDT_Reporte = new ReporteR();

            List<ReporteRemanente> objLista = objDT_Reporte.RetornarRemanente();

            return Json(objLista, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ReporteporMarcas()
        {
            ReporteR objDT_Reporte = new ReporteR();

            List<ReporteporMarcas> objLista = objDT_Reporte.RetornarMarca();

            return Json(objLista, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ReporteporEstado()
        {
            ReporteR objDT_Reporte = new ReporteR();

            List<ReporteporEstado> objLista = objDT_Reporte.RetornarEstado();

            return Json(objLista, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ReporteMarcas()
        {
            List<ReporteMarcas> oLista = new List<ReporteMarcas>();
            oLista = ReporteR.Instancia.ListarReporteMarcas();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        // Exportar a Excel ---------------------------------------------------------------------------------------------------------------

        [HttpPost]
        public FileResult ExportarInspecciones(string fechainicio, string fechafin)
        {

            DataTable dt = ReencaucheLogica.Instancia.ExpInspecciones(fechainicio, fechafin);

            dt.TableName = "Datos";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte de Inspecciones " + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }

        // Notificaciones ---------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public JsonResult ListarNotificaciones()
        {
            List<Notificaciones> oLista = new List<Notificaciones>();
            oLista = StockLogica.Instancia.Notificaciones();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        // Importar a SQL ---------------------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Import(FormCollection formCollection)
        {
            var remList = new List<REMANENTE>();
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var reman = new REMANENTE();
                            reman.codllanta = workSheet.Cells[rowIterator, 1].Value.ToString();
                            reman.fechainspeccion = workSheet.Cells[rowIterator, 2].Value.ToString();
                            reman.kminspeccion = Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value);
                            reman.remanenteactual = Convert.ToInt32(workSheet.Cells[rowIterator, 4].Value);
                            reman.estadooperacion = workSheet.Cells[rowIterator, 5].Value.ToString();
                            reman.observaciones = workSheet.Cells[rowIterator, 6].Value.ToString();
                            reman.estado = true;
                            reman.fechacreacion = DateTime.Now;
                            remList.Add(reman);
                        }
                    }
                }
            }
            using (DB_NEUMATICOSEntities DB_NEUMATICOSEntities = new DB_NEUMATICOSEntities())
            {
                foreach (var item in remList)
                {
                    DB_NEUMATICOSEntities.REMANENTE.Add(item);

                    var otraTabla = DB_NEUMATICOSEntities.LLANTA.Where(x => x.codllanta == item.codllanta).FirstOrDefault();
                    if (otraTabla != null)
                    {
                        otraTabla.remanente = item.remanenteactual;
                        DB_NEUMATICOSEntities.Entry(otraTabla).State = EntityState.Modified;
                    }

                }
                int result = DB_NEUMATICOSEntities.SaveChanges();
                if (result > 0)
                {
                    TempData["mensaje"] = "Los registros han sido importados exitosamente";
                    return RedirectToAction("Crear");
                }
            }
            return View("Crear");
        }

    }
}