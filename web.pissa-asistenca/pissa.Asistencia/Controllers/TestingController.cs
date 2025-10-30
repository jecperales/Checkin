using pissa.Asistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace pissa.Asistencia.Controllers
{
    public class TestingController : Controller
    {
        // GET: Testing
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult saveDateTime(control_asistencia asistencia) {

            var res = new Bussines.Testing().saveDateTime(asistencia);

            return Json(res);
        }
    }
}