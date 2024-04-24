using pissa.Asistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace pissa.Asistencia.Controllers
{
    public class AreaController : Controller
    {
        // GET: Area
        public ActionResult Index()
        {
            if (Session["correo"] != null)
            {
                ViewBag.profile = int.Parse(Session["Profile"].ToString());
                ViewBag.pais_reporte = Session["pais_reporte"];
                int id_user = int.Parse(Session["idSesion"].ToString());
              
                return View();
            }
            else
            {
                //Si no hay una sesion iniciada se direcciona al login
                return RedirectToAction("Index", "Acount");
            }
        }

        [HttpGet]
        public ActionResult getProyectos() {
            
            List<proyecto> listaProyectos = new List<proyecto>();

            listaProyectos = new Bussines.Area().getProyectos();

            return Json(listaProyectos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult insertarProyecto(proyecto proyecto) 
        {
            int created = new Bussines.Area().insertarProyecto(proyecto);

            return Json(created);
        }

        [HttpPost]
        public ActionResult actualizarProyecto(proyecto proyecto) 
        {
            int updated = new Bussines.Area().actualizarProyecto(proyecto);

            return Json(updated);
        }

        [HttpPost]
        public ActionResult eliminarProyecto(int idProyecto)
        {
            int res = new Bussines.Area().eliminarProyecto(idProyecto);

            return Json(res); 
        }

        [HttpGet]
        public ActionResult getIngenieros(int idProyecto) 
        {
            List<ingeniero> ingenieroLst = new List<ingeniero>();

            ingenieroLst =  new Bussines.Area().getIngenieros(idProyecto);

            return Json(ingenieroLst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult desactivarIngeniero(int idIngeniero) 
        {
            var res = new Bussines.Area().desactivarIngeniero(idIngeniero);
            return Json(res);   
        }

        [HttpPost]
        public ActionResult desactivarTodosLosIngenieros(int idProyecto)
        {
            var res = new Bussines.Area().desactivarTodosLosIngenieros(idProyecto);
            return Json(res);
        }
    }
}