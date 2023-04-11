using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace pissa.Asistencia.Controllers
{
    public class PerfilController : Controller
    {
        // GET: Perfil
        public ActionResult Index()
        {
            if (Session["correo"] != null)
            {
                var serializer = new JavaScriptSerializer();
                ViewBag.profile = int.Parse(Session["Profile"].ToString());
                int id_user = int.Parse(Session["idSesion"].ToString());
                ViewBag.misdatos = serializer.Serialize( new Bussines.Acount().getPerfil(id_user));



                return View();
            }
            else
            {
                return RedirectToAction("Index", "Acount");
            }
        }

        public ActionResult modificar_perfil(string emei, string psw)
        {
            if (Session["correo"] != null)
            {
                bool status = false;
                int id_user = int.Parse(Session["idSesion"].ToString());

                var temp_inge = new Bussines.Acount().getIngeniero(id_user);
                var temp_movil = new Bussines.Acount().getMobil(id_user);
                try {
                    if (temp_inge.password != psw)
                    {
                        temp_inge.password = psw;
                        new Bussines.Acount().altaIngeniero(temp_inge);
                    }
                    if (temp_movil.no_imei != emei)
                    {
                        temp_movil.no_imei = emei;
                        new Bussines.Acount().altaMovil(temp_movil);
                    }

                    status = true;
                }
                catch(Exception e1)
                {
                    status = false;
                }
               

                return Json(status);
            }
            else
            {
                return RedirectToAction("Index", "Acount");
            }
        }



    }
}