using ClosedXML.Excel;
using pissa.Asistencia.Entities;
using pissa.Asistencia.Models;
using pissa.Asistencia.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
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
            if (Session["correo"] !=null)
            {                
                ViewBag.profile = int.Parse(Session["Profile"].ToString());
                ViewBag.pais_reporte = Session["pais_reporte"];
                int id_user = int.Parse(Session["idSesion"].ToString());
                List<int> proyect = new List<int>();                

                var lista = new Bussines.Acount().getProyectos();
                var proyecto_x_usuario = (List<privilegios>)Session["departa"];
              
                if (proyecto_x_usuario.Count()>=2) {
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
                        lista.Where(p=> p.id==id_proyect).Select(p=> new { Value = p.id.ToString(), Text = p.nombre_proyecto }).ToList()
                        );

                }
                              

                if(ViewBag.selectProyecto==0)
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
                return RedirectToAction("Index","Acount");
            }


            
        }


        public ActionResult chekAsistencia()
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

        public ActionResult registra_personal(regitraPersonal altapersonal)
        {
            altapersonal.ing.engineer_id = " ";
            altapersonal.ing.state = " ";
            altapersonal.ing.street = " ";
            altapersonal.ing.suburb = " ";
            altapersonal.ing.town = " ";

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
            control.fecha_hora_registro = DateTime.Now;

            var resul = new Bussines.Asistencia().regasistencias(control);
            if (resul.id_ingeniero!=0) {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
            
        }

        public string registraSalida(control_asistencia control)
        {
            int id_user = int.Parse(Session["idSesion"].ToString());
            control.id_ingeniero = id_user;
            control.fecha_hora_salida = DateTime.Now;

            ViewBag.ingenieroId = id_user;
            ViewBag.fecha_hora_salida = control.fecha_hora_salida;
            
            var resul = new Bussines.Asistencia().registraSalida(control);

            return resul.ToString();
        }

        public ActionResult generaAsistencias(filtro_asistencia filtro)
        {
            List<genera_asistencias> result = new List<genera_asistencias>();            

            if (Session["correo"] != null)
            {
                int profile = int.Parse(Session["Profile"].ToString());
                int id_user = int.Parse(Session["idSesion"].ToString());
                string pais_reporte = filtro.paisReporte;
                int[] _idUser=null;
               
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

                    case 3:

                        _idUser = new int[1];
                        _idUser[0] = id_user;

                        result = new Bussines.Asistencia().GeneraListaAsistenciaSupervisorLatam(filtro.fecha1, filtro.fecha2, filtro.proyectos, _idUser, filtro.paisReporte);

                        break;
                }

                string fecha;
                string hora;
                string fecha_s="";
                string hora_s="";


                foreach (var a in result) 
                {
                    fecha = a.fecha_hora_registro.Substring(0, 10);
                    hora = a.fecha_hora_registro.Substring(11, 14);

                    fecha_s = Convert.ToDateTime(a.fecha_hora_salida) < DateTime.Parse("01/01/1900") ? "No hay registro" : a.fecha_hora_salida.Substring(0, 10);
                    hora_s = Convert.ToDateTime(a.fecha_hora_salida) < DateTime.Parse("01/01/1900") ? "No hay registro" : a.fecha_hora_salida.Substring(11, 14); 

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

                case 3:

                    _idUser = new int[1];
                    _idUser[0] = id_user;

                    result = new Bussines.Asistencia().GeneraListaAsistenciaSupervisorLatam(filtro.fecha1, filtro.fecha2, filtro.proyectos, _idUser, filtro.paisReporte);

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
            ws.Cell(1, 5).Value = "Hora de salida";
            ws.Cell(1, 6).Value = "Ubicacion de salida";
            ws.Cell(1, 7).Value = "Area";
            ws.Cell(1, 8).Value = "Pais";         

            foreach (var a in result)
            {
                nombre = a.Nombre + " " + a.last_name_p + " " + a.last_name_m;
                fecha = a.fecha_hora_registro.Substring(0, 10);
                hora = a.fecha_hora_registro.Substring(11, 14);
                
                fecha_s = Convert.ToDateTime(a.fecha_hora_salida) < DateTime.Parse("01/01/1900") ? "No hay registro" : a.fecha_hora_salida.Substring(0, 10);
                hora_s = Convert.ToDateTime(a.fecha_hora_salida) < DateTime.Parse("01/01/1900") ? "No hay registro" : a.fecha_hora_salida.Substring(11, 14);
                
                link_e = "http://maps.google.com/maps?z=12&t=m&q=loc:" + a.latitud.ToString() + "+" + a.longitud.ToString();

                if (a.s_latitud > 0)
                {
                    link_s = "http://maps.google.com/maps?z=12&t=m&q=loc:" + a.s_latitud.ToString() + "+" + a.s_longitud.ToString();
                }
                else 
                {
                    link_s = "";
                }

                ws.Cell(row, col).Value = a.Nombre + " " + a.last_name_p + " " + a.last_name_m ;
                ws.Cell(row, ++col).Value = fecha;

                ws.Cell(row, ++col).SetDataType(XLDataType.DateTime);                              
                ws.Cell(row, col).Style.DateFormat.Format = "HH:mm:ss";
                ws.Cell(row, col).Value = hora;

                ws.Cell(row, ++col).Value = "Ver ubicación";
                ws.Cell(row, col).Hyperlink = new XLHyperlink(link_e);

                ws.Cell(row, ++col).SetDataType(XLDataType.DateTime);
                ws.Cell(row, col).Style.DateFormat.Format = "HH:mm:ss";
                ws.Cell(row, col).Value = hora_s;

                if (String.IsNullOrEmpty(link_s))
                {
                    ws.Cell(row, ++col).Value = "No hay registro";
                }
                else
                {
                    ws.Cell(row, ++col).Value = "Ver ubicación";
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


    }
}