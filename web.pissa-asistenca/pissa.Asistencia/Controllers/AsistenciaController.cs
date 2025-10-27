using ClosedXML.Excel;
using Microsoft.Ajax.Utilities;
using pissa.Asistencia.Entities;
using pissa.Asistencia.Models;
using pissa.Asistencia.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace pissa.Asistencia.Controllers
{
    public class AsistenciaController : Controller
    {
        // GET: Asistencia
        public ActionResult Index(int pagina = 1)
        {
            if (Session["correo"] != null)
            {
                ViewBag.profile = int.Parse(Session["Profile"].ToString());
                ViewBag.pais_reporte = Session["pais_reporte"];
                int id_user = int.Parse(Session["idSesion"].ToString());
                List<int> proyect = new List<int>();

                var lista = new Bussines.Acount().getProyectos();
                var proyecto_x_usuario = (List<privilegios>)Session["departa"];

                if (proyecto_x_usuario.Count() >= 2) {
                    for (var pos = 0; pos < proyecto_x_usuario.Count(); pos++)
                    {
                        proyect.Add(proyecto_x_usuario[pos].id_proyecto);
                    }
                    //ViewBag.tipo = new JavaScriptSerializer().Serialize(
                    //    lista.Where(x => proyect.Contains(x.id)).Select(p => new SelectListItem() { Value = p.id.ToString(), Text = p.nombre_proyecto }).ToList<SelectListItem>());
                    ViewBag.tipo = new JavaScriptSerializer().Serialize(
                     lista.Where(x => proyect.Contains(x.id)).Select(p => new { Value = p.id.ToString(), Text = p.nombre_proyecto }).ToList()
                     );
                }
                else
                {
                    ViewBag.selectProyecto = new Bussines.Acount().getValidaProyecto(id_user);
                    int id_proyect = ViewBag.selectProyecto;
                    //ViewBag.tipo = new JavaScriptSerializer().Serialize(
                    //    lista.Where(x => x.id == id_proyect).Select(p => new SelectListItem() { Value = p.id.ToString(), Text = p.nombre_proyecto }).ToList<SelectListItem>());

                    ViewBag.tipo = new JavaScriptSerializer().Serialize(
                        lista.Where(p => p.id == id_proyect).Select(p => new { Value = p.id.ToString(), Text = p.nombre_proyecto }).ToList()
                        );

                }


                if (ViewBag.selectProyecto == 0)
                {
                    ViewBag.tipo = lista.Select(p => new SelectListItem() { Value = p.id.ToString(), Text = p.nombre_proyecto }).ToList<SelectListItem>();
                }



                ViewBag.fecha_inicio = new Utilerias().primer_dia_mes();
                ViewBag.fecha_fin = new Utilerias().ultimo_dia_mes();

                return View();
            }
            else
            {
                //Si no hay una sesion iniciada se direcciona al login
                return RedirectToAction("Index", "Acount");
            }
        }

        public ActionResult chekAsistencia()
        {
            try
            {
                control_asistencia ca = new control_asistencia();

                int ingenieroId = int.Parse(Session["idSesion"].ToString());
                //control.id_ingeniero = id_user;
                //control.fecha_hora_registro = DateTime.Now;

                if (Session["correo"] != null)
                {
                    //Agregado por Jose Enrique Cruz Perales el 9 de Marzo de 2022
                    ViewBag.profile = int.Parse(Session["Profile"].ToString());

                    ca = new Bussines.Asistencia().getAsistenciaToday(ingenieroId);
                    ViewBag.id_asistencia = ca?.id_asistencia ?? 0;
                    ViewBag.latitud = ca?.latitud ?? 0;
                    ViewBag.longitud = ca?.longitud ?? 0;
                    ViewBag.fecha_hora_movil = ca?.fecha_hora_movil.ToString() ?? "";
                    ViewBag.fecha_hora_registro = ca?.fecha_hora_registro.ToString() ?? "";
                    ViewBag.id_ingeniero = ca?.id_ingeniero ?? 0;
                    ViewBag.fecha_hora_salida = ca?.fecha_hora_salida.ToString() ?? "";
                    ViewBag.s_latitud = ca?.s_latitud ?? 0;
                    ViewBag.s_longitud = ca?.s_longitud ?? 0;

                    return View(ca);
                }
                else
                {
                    return RedirectToAction("Index", "Acount");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Acount");
            }
            
               
        }

        public ActionResult altapersonal(int pagina = 1)
        {
            if (Session["correo"] != null)
            {
                ViewBag.profile = int.Parse(Session["Profile"].ToString());
                int id_user = int.Parse(Session["idSesion"].ToString());
                var lista = new Bussines.Acount().getProyectos();

                
                ViewBag.tipo = new JavaScriptSerializer().Serialize(
                    lista.Select(p => new { id = p.id.ToString(), label = p.nombre_proyecto }).ToList());

                List<catalogo_celulares> listaMarcas = new Bussines.Asistencia().get_catalogo_celulares();
                List<country> countriesLst = new Bussines.Asistencia().GetCountries();
            

                ViewBag.marcatelefono = new JavaScriptSerializer().Serialize(
                    listaMarcas.Select(p=> new  { id=p.marca,label=p.marca}).ToList());

                ViewBag.country = new JavaScriptSerializer().Serialize(
                    countriesLst.Select(c => new {
                        id = c.id, 
                        code = c.code,
                        name = c.name }).ToList()
                    );              
               
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Acount");
            }
        }

        public ActionResult Charts() 
        {
            if (Session["correo"] != null)
            {
                var perfilId = int.Parse(Session["Profile"].ToString());
                var proyectoId = int.Parse(Session["Proyecto"].ToString());                

                ViewBag.profile = perfilId;
                ViewBag.proyectoId = proyectoId;
                ViewBag.pais_reporte = Session["pais_reporte"];

                ViewBag.fecha_inicio = new Utilerias().primer_dia_mes();
                ViewBag.fecha_fin = new Utilerias().ultimo_dia_mes();
               
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Acount");
            }
        }

        public ActionResult registra_personal(regitraPersonal altapersonal)
        {
            altapersonal.ing.status = "1";
            altapersonal.ing.state = "";
            altapersonal.ing.street = "";
            altapersonal.ing.suburb = "";
            altapersonal.ing.town = "";

            var alta = new Bussines.Acount().registra_personal(altapersonal);
           

            return Json(alta);
        }

        public ActionResult paginado_personal(int pageIndex,int pageSize)
        {
            var result = new Bussines.Asistencia().paginado_personal(pageIndex, pageSize);

            return Json(result);
        }

        public ActionResult regasistencias(control_asistencia control)
        {
            int id_user = int.Parse(Session["idSesion"].ToString());           
            control.id_ingeniero = id_user;
            ViewBag.fecha_hora_movil = control.fecha_hora_registro.ToString("dd/MM/yyyy HH:mm:ss");

            var result = new Bussines.Asistencia().regasistencias(control);
            return Json(result);
            
        }

        public ActionResult registraSalida(control_asistencia control)
        {

            int id_user = int.Parse(Session["idSesion"].ToString());
            control.id_ingeniero = id_user;

            ViewBag.ingenieroId = id_user;
            ViewBag.fecha_hora_salida = control.fecha_hora_salida.ToLongDateString();
            
            var result = new Bussines.Asistencia().registraSalida(control);

            return Json(result);

        }

        [HttpPost]
        public ActionResult generaAsistencias(filtro_asistencia filtro)
        {
            List<genera_asistencias> result = new List<genera_asistencias>();
            int profile = int.Parse(Session["Profile"].ToString());
            int id_user = int.Parse(Session["idSesion"].ToString());
            int idProyecto = int.Parse(Session["Proyecto"].ToString());
            string pais_reporte = filtro.paisReporte;
            int[] _idUser = null;

            if (Session["correo"] != null)
            {                               
                switch (profile) 
                {
                    case 1:                    

                        var contiene_ingenieros = new Bussines.Acount().getIngenieros().Select(x => x.id_ingeniero).ToList();
                        _idUser = new int[contiene_ingenieros.Count()];

                        for (var pos = 0; pos < contiene_ingenieros.Count(); pos++)
                        {
                            _idUser[pos] = contiene_ingenieros[pos];
                        }

                        result = new Bussines.Asistencia().GeneraListaAsistenciaAdmin(filtro.fecha1, filtro.fecha2, filtro.proyectos, _idUser);

                        break;

                    case 2:

                        _idUser = new int[1];
                        _idUser[0] = id_user;

                        result = new Bussines.Asistencia().GeneraListaAsistenciaIngeniero(filtro.fecha1, filtro.fecha2, _idUser);

                        break;

                    case 30:

                        _idUser = new int[1];
                        _idUser[0] = id_user;

                        result = new Bussines.Asistencia().GeneraListaAsistenciaSupervisorLatam(filtro.fecha1, filtro.fecha2, filtro.proyectos, _idUser, filtro.paisReporte);

                        break;

                    //PERFIL CLIENTE: Puede ver todos los ingenieros que pertenecen al mismo proyecto que el cliente (Ex: Si el cliente esta asigando a Talento Humano)
                    //se deben ver todos los ingenieros que pertenecen a Talento Humano
                    case 3:
                    case 4:

                        result = new Bussines.Asistencia().GeneraListaAsistenciaCliente(filtro.fecha1, filtro.fecha2, idProyecto);

                        break;
                }

                string fecha;
                string hora;                
                string fecha_s="";
                string hora_s="";


                foreach (var a in result) 
                {
                    fecha = a.fecha_hora_registro.Substring(0, 10);
                    //hora = a.fecha_hora_registro.Substring(11, 14);
                    hora = Convert.ToDateTime(a.fecha_hora_registro).ToString("HH:mm:ss");

                    fecha_s = Convert.ToDateTime(a.fecha_hora_salida) < DateTime.Parse("01/01/1900") ? "No hay registro" : a.fecha_hora_salida.Substring(0, 10);
                    //hora_s = Convert.ToDateTime(a.fecha_hora_salida) < DateTime.Parse("01/01/1900") ? "No hay registro" : a.fecha_hora_salida.Substring(11, 14); 
                    hora_s = Convert.ToDateTime(a.fecha_hora_salida) < DateTime.Parse("01/01/1900") ? "No hay registro" : Convert.ToDateTime(a.fecha_hora_salida).ToString("HH:mm:ss");

                    a.fecha_hora_registro = fecha;
                    a.hora_entrada = hora;
                    a.fecha_hora_salida = fecha_s;
                    a.hora_salida = hora_s;

                }

                return Json(result);
            }
            else
            {
                return Json(null);
            }
            
        }

        public ActionResult exporta_excel_asistencias(filtro_asistencia filtro) //string fecha1, string fecha2,int cade
        {
            List<genera_asistencias> result = new List<genera_asistencias>();

            int profile = int.Parse(Session["Profile"].ToString());
            int id_user = int.Parse(Session["idSesion"].ToString());
            int idProyecto = int.Parse(Session["Proyecto"].ToString());
            string pais_reporte = filtro.paisReporte;
            int[] _idUser = null;

            switch (profile)
            {
                case 1:

                    var contiene_ingenieros = new Bussines.Acount().getIngenieros().Select(x => x.id_ingeniero).ToList();
                    _idUser = new int[contiene_ingenieros.Count()];

                    for (var pos = 0; pos < contiene_ingenieros.Count(); pos++)
                    {
                        _idUser[pos] = contiene_ingenieros[pos];
                    }

                    result = new Bussines.Asistencia().GeneraListaAsistenciaAdmin(filtro.fecha1, filtro.fecha2, filtro.proyectos, _idUser);

                    break;

                case 2:

                    _idUser = new int[1];
                    _idUser[0] = id_user;

                    result = new Bussines.Asistencia().GeneraListaAsistenciaIngeniero(filtro.fecha1, filtro.fecha2, _idUser);

                    break;

                case 31:

                    _idUser = new int[1];
                    _idUser[0] = id_user;

                    result = new Bussines.Asistencia().GeneraListaAsistenciaSupervisorLatam(filtro.fecha1, filtro.fecha2, filtro.proyectos, _idUser, filtro.paisReporte);

                    break;

                case 3:
                case 4:

                    result = new Bussines.Asistencia().GeneraListaAsistenciaCliente(filtro.fecha1, filtro.fecha2, idProyecto);

                    break;
            }

            string nombre = "";
            string fecha;
            string hora;
            string fecha_s = "";
            string hora_s = "";
            string link_e = "";
            string link_s = "";

            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Reporte de asistencia");

            int row = 2;
            int col = 1;

            ws.Cell(1, 1).Value = "Nombre";
            ws.Cell(1, 2).Value = "Fecha Registro";
            ws.Cell(1, 3).Value = "Hora de entrada";
            ws.Cell(1, 4).Value = "Ubicacion de entrada";
            ws.Cell(1, 5).Value = "Fecha Salida";
            ws.Cell(1, 6).Value = "Hora de salida";
            ws.Cell(1, 7).Value = "Ubicacion de salida";
            ws.Cell(1, 8).Value = "Area";
            ws.Cell(1, 9).Value = "Pais";         

            foreach (var a in result)
            {
                nombre = a.Nombre + " " + a.last_name_p + " " + a.last_name_m;
                fecha = a.fecha_hora_registro.Substring(0, 10);
                hora = Convert.ToDateTime(a.fecha_hora_registro).ToString("HH:mm:ss");

                
                fecha_s = Convert.ToDateTime(a.fecha_hora_salida) < DateTime.Parse("01/01/1900") ? "No hay registro" : a.fecha_hora_salida.Substring(0, 10);
                //hora_s = Convert.ToDateTime(a.fecha_hora_salida) < DateTime.Parse("01/01/1900") ? "No hay registro" : a.fecha_hora_salida.Substring(11, 14);
                hora_s = Convert.ToDateTime(a.fecha_hora_salida) < DateTime.Parse("01/01/1900") ? "No hay registro" : Convert.ToDateTime(a.fecha_hora_salida).ToString("HH:mm:ss");

                link_e = "http://maps.google.com/maps?z=12&t=m&q=loc:" + a.latitud.ToString() + "+" + a.longitud.ToString();

                if (a.s_latitud != 0)
                {
                    link_s = "http://maps.google.com/maps?z=12&t=m&q=loc:" + a.s_latitud.ToString() + "+" + a.s_longitud.ToString();
                }
                else 
                {
                    link_s = "";
                }

                ws.Cell(row, col).Value = a.Nombre + " " + a.last_name_p + " " + a.last_name_m ;
                ws.Cell(row, ++col).Value = fecha;

                ws.Cell(row, ++col).SetDataType(XLDataType.TimeSpan);                              
                ws.Cell(row, col).Style.DateFormat.Format = "HH:mm:ss";
                ws.Cell(row, col).SetValue(hora);

                ws.Cell(row, ++col).Value = a.latitud.ToString() + " , " + a.longitud.ToString();
                ws.Cell(row, col).Hyperlink = new XLHyperlink(link_e);

                ws.Cell(row, ++col).Value = fecha_s;

                ws.Cell(row, ++col).SetDataType(XLDataType.TimeSpan);
                ws.Cell(row, col).Style.DateFormat.Format = "HH:mm:ss";                
                ws.Cell(row, col).SetValue(hora_s);

                if (String.IsNullOrEmpty(link_s))
                {
                    ws.Cell(row, ++col).Value = "No hay registro";
                }
                else
                {
                    ws.Cell(row, ++col).Value = a.s_latitud.ToString() + " , " + a.s_longitud.ToString();
                    ws.Cell(row, col).Hyperlink = new XLHyperlink(link_s);
                }

                ws.Cell(row, ++col).Value = a.Area;
                ws.Cell(row, ++col).Value = a.pais;

                row++;
                col = 1;             
            }

            ws.Columns().AdjustToContents();
            ws.Range("A1", "H1").SetAutoFilter();

            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "HistorialAsistencia" + DateTime.Today.ToShortDateString() + ".xlsx");
            }           
        }

        [HttpPost]
        public JsonResult actualizaproyecto(string id_proyecto)
        {
            bool status = false;
            if (Session["idSesion"]!=null)
            {
                int id_user = int.Parse(Session["idSesion"].ToString());
                status = new Bussines.Acount().actualizaproyecto(int.Parse(id_proyecto), id_user);
                return Json(status);
            }
            else
            {
                return Json(status);
            }
            
        }
       
        public ActionResult getProyectos() 
        {
            var perfilId = int.Parse(Session["Profile"].ToString());
            var proyectoId = int.Parse(Session["Proyecto"].ToString());
            var lstProyectos = new Bussines.Acount().getProyectos();            

            if (perfilId == 1)
            {
                var res = lstProyectos.Select(p => new SelectListItem() { Value = p.id.ToString(), Text = p.nombre_proyecto }).ToList<SelectListItem>();
                return Json(res);
            }
            else if (perfilId == 4)
            {
                var res = lstProyectos.Select(p => new SelectListItem() { Value = p.id.ToString(), Text = p.nombre_proyecto }).Where(p => p.Value == proyectoId.ToString()).ToList<SelectListItem>();
                return Json(res);
            }

            return Json("Error");
            
        }

        public ActionResult getPerfilList()
        {
            var res = new Bussines.Perfil().getPerfilList();

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult eliminarIngeniero(int id) 
        {
            var res = new Bussines.Asistencia().eliminarIngeniero(id);

            return Json(res);
        }


    }
}