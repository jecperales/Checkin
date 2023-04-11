using pissa.Asistencia.Entities;
using pissa.Asistencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pissa.Asistencia.Controllers
{
    public class AcountController : Controller
    {
        // GET: Acount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult logear(string correo,string psw)
        {

            var result = new Bussines.Acount().logear(correo,psw);

           
            if (result!=null)
            {
                Session["idSesion"] = result.id;
                Session["correo"] = result.username;
                Session["departa"] = result.Listaproyecto;
                Session["Profile"] = result.id_profile;
                Session["nombre"] = result.username;
                Session["pais_reporte"] = result.pais_reporte;
                return RedirectToAction("Index", "Asistencia");
            }
            else
            {
                return RedirectToAction("Index", "Acount");
            }
            
        }

        public ActionResult salir()
        {
            Session["idSesion"] = null;
            Session["correo"] = null;
            Session["departa"] = null;
            Session["Profile"] = null;
            Session["nombre"] = null;
            return RedirectToAction("Index", "Acount");
        }


        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public JsonResult altaRegistro(altaUsuario usuer)
        {
            if (!string.IsNullOrWhiteSpace(usuer.nombre) && !string.IsNullOrWhiteSpace(usuer.apPaterno) && !string.IsNullOrWhiteSpace(usuer.apMaterno)
                && !string.IsNullOrWhiteSpace(usuer.correo) && !string.IsNullOrWhiteSpace(usuer.psw))
            {


                bool result = new Bussines.Acount().regIngeniero(new preRegistro {
                    nombre=usuer.nombre,
                    apMaterno=usuer.apMaterno,
                    apPaterno=usuer.apPaterno,
                    correo=usuer.correo,
                    psw=usuer.psw
                });

                //return RedirectToAction("Index", "Asistencia");
                var cadena = Url.Action("Index", "Acount");
                return Json(cadena);
            }
            else
            {
                // return RedirectToAction("Index", "Acount");
                var cadena1 = Url.Action("Index", "Acount");
                return Json(cadena1);
            }
            
        }



    }
}