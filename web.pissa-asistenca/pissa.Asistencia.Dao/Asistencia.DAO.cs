using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pissa.Asistencia.Entities;
using MySql.Data.Entity;

namespace pissa.Asistencia.Dao
{
    public class Asistencia
    {
        public control_asistencia getAsistenciaTodayByIngenieroId(int ingenieroId) 
        {
            control_asistencia ca = new control_asistencia();

            try
            {
                using (DataContext db = new DataContext()) 
                {
                    string sqlQuery = "SELECT * FROM control_asistencia WHERE id_ingeniero=" + ingenieroId + " AND date_format(fecha_hora_movil, '%Y-%m-%d') = date_format(now(), '%Y-%m-%d') ORDER BY id_asistencia DESC Limit 1";
                    ca = db.Database.SqlQuery<control_asistencia>(sqlQuery).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ca;
        }
        public List<genera_asistencias> generaAsistencias(string fecha1, string fecha2, int proyectos, int[] idUser)
        {
            List<genera_asistencias> Lista = new List<genera_asistencias>();
            List<genera_asistencias> Lista1 = new List<genera_asistencias>();
            DateTime f1 = DateTime.Parse(fecha1);
            DateTime f2 = DateTime.Parse(fecha2);

            string cadenaUser = "";
            string cadenaProye = "";

            cadenaUser = idUser[0].ToString();
            if (idUser.Length>1) {
                for (var puser = 1; puser < idUser.Length; puser++) {
                    cadenaUser += "," + idUser[puser].ToString();
                }
            }

            //cadenaProye = proyectos[0].ToString();
            //if (proyectos.Length>1) {
            //    for (var pos = 1; pos < proyectos.Length; pos++)
            //    {
            //        cadenaProye += ","+ proyectos[pos].ToString();
            //    }
            //}
           
            try {

                using (DataContext db = new DataContext())
                {
                   
                    //string cadena = @" select  concat(name,' ',last_name_p,' ',last_name_m) as Nombre,p.nombre_proyecto as Area,date_format(cc.fecha_hora_movil, '%Y-%m-%d %r') as fecha_hora_registro,
                    //                     date_format((select max(c.fecha_hora_registro) from assistence_management.control_asistencia c where 
                    //                     c.id_ingeniero=c.id_ingeniero and date_format(c.fecha_hora_movil, '%Y-%m-%d')=date_format(cc.fecha_hora_movil, '%Y-%m-%d')),'%Y-%m-%d %r') as fecha_salida
                    //                     from assistence_management.control_asistencia cc
                    //                     join assistence_management.ingeniero i on cc.id_ingeniero=i.id_ingeniero
                    //                     join assistence_management.proyecto p on i.id_proyecto=p.id
                    //                     where cc.id_ingeniero in (" + cadenaUser + ") and p.id in (" + cadenaProye + ")"+
                    //                     " and date_format(cc.fecha_hora_movil, '%H:%i:%s') between '06:00:00' and '17:00:00' "+
                    //                     " and date_format(cc.fecha_hora_movil, '%Y-%m-%d') between '" + fecha1 + "' and '" + fecha2 + "'" +
                    //                     " order by Nombre,cc.fecha_hora_registro desc";

                    string cadena = @"SELECT 
	                                    CA.id_asistencia, CA.latitud, CA.longitud, CA.fecha_hora_movil, CA.fecha_hora_registro, CA.id_ingeniero,
                                        CA.fecha_hora_salida, CA.s_latitud, CA.s_longitud,
                                        CONCAT(I.name, ' ', I.last_name_p, ' ', I.last_name_m, id_proyecto) AS nombre, I.id_proyecto,
                                        P.nombre_proyecto
                                    FROM control_asistencia CA
	                                    JOIN ingeniero I ON CA.id_ingeniero = I.id_ingeniero
	                                    JOIN proyecto P ON I.id_proyecto = P.id
                                    WHERE I.id_proyecto = "+ proyectos + @"  AND 
	                                      date_format(CA.fecha_hora_movil, '%H:%i:%s') BETWEEN '06:00:00' AND '17:00:00'  AND 
	                                      date_format(CA.fecha_hora_movil, '%Y-%m-%d') BETWEEN '"+ fecha1 + "' AND '" + fecha2 + @"' 	  
                                    ORDER BY fecha_hora_movil, nombre_proyecto, nombre DESC";

                    Lista = db.Database.SqlQuery<genera_asistencias>(cadena).ToList();

                    //Lista = (from a in db.control_asistencia
                    //         join i in db.ingeniero on a.id_ingeniero equals i.id_ingeniero
                    //         join p in db.proyecto on i.id_proyecto equals p.id
                    //         where 
                    //         (a.fecha_hora_registro >= f1) &&
                    //               ( a.fecha_hora_registro <= f2 ) &&
                    //           proyectos.Contains(p.id) && idUser.Contains(i.id_ingeniero)
                    //         orderby a.fecha_hora_registro
                    //         select new genera_asistencias
                    //         {
                    //             name = i.name+" "+i.last_name_p+" "+ i.last_name_m,
                    //             last_name_p = i.last_name_p,
                    //             last_name_m = i.last_name_m,
                    //             area = p.nombre_proyecto,
                    //             fecha_hora_registro = a.fecha_hora_registro.ToString()
                    //         }).ToList();

                    

                    foreach (var item in Lista)
                    {
                        genera_asistencias ga = new genera_asistencias();

                        ga.Nombre = item?.Nombre ?? "";
                        ga.Area = item?.Area ?? "";
                       
                        ga.fecha_hora_salida = item.fecha_hora_salida;
                        ga.s_latitud = item?.s_latitud ?? 0;
                        ga.s_longitud = item?.s_longitud ?? 0;

                        Lista1.Add(ga);

                        //Lista1.Add(new genera_asistencias
                        //{
                        //    Nombre = item?.Nombre ?? "",
                        //    Area = item?.Area ?? "",
                        //    fecha_hora_registro = item?.fecha_hora_registro.ToString() ?? "",
                        //    latitud = item?.latitud.ToString() ?? "",
                        //    longitud = item?.longitud.ToString() ?? "",
                        //    retardo = new Utilerias().validarRetardo(DateTime.Parse(item.fecha_hora_registro))?.es_retardo ?? "",
                        //    hora_retardo = new Utilerias().validarRetardo(DateTime.Parse(item.fecha_hora_registro))?.diferencia_retardo ?? "",
                        //    fecha_hora_salida=item?.fecha_hora_salida.ToString() ?? "",
                        //    s_latitud = item?.s_latitud ?? 0,
                        //    s_longitud = item?.s_longitud ?? 0
                           
                        //});
                    }

                }

            } 
            catch (Exception ex) 
            {
                return Lista1;
            }           

            return Lista1;
        }

        public List<genera_asistencias> GeneraListaAsistenciaAdmin(DateTime fecha1, DateTime fecha2, int proyectos, int[] idUser) 
        {      

            using (DataContext db = new DataContext())
            {
                if (proyectos > 0)
                {
                    return (from a in db.control_asistencia
                            join i in db.ingeniero on a.id_ingeniero equals i.id_ingeniero
                            join p in db.proyecto on i.id_proyecto equals p.id
                            where
                            (a.fecha_hora_registro >= fecha1) &&
                            (a.fecha_hora_registro <= fecha2) &&
                            p.id == proyectos
                            select new genera_asistencias
                            {
                                Nombre = i.name,
                                last_name_p = i.last_name_p,
                                last_name_m = i.last_name_m,
                                pais = i.pais,
                                Area = p.nombre_proyecto,
                                fecha_hora_registro = a.fecha_hora_registro.ToString(),
                                hora_entrada = a.fecha_hora_registro.ToString(),
                                fecha_hora_salida = a.fecha_hora_salida.ToString(),
                                latitud = a.latitud,
                                longitud = a.longitud,
                                s_latitud = a.s_latitud,
                                s_longitud = a.s_longitud
                            }).ToList();

                }
                else                
                {
                    return (from a in db.control_asistencia
                            join i in db.ingeniero on a.id_ingeniero equals i.id_ingeniero
                            join p in db.proyecto on i.id_proyecto equals p.id
                            where
                            (a.fecha_hora_registro >= fecha1) &&
                            (a.fecha_hora_registro <= fecha2)
                            select new genera_asistencias
                            {
                                Nombre = i.name,
                                last_name_p = i.last_name_p,
                                last_name_m = i.last_name_m,
                                pais = i.pais,
                                Area = p.nombre_proyecto,
                                fecha_hora_registro = a.fecha_hora_registro.ToString(),
                                hora_entrada = a.fecha_hora_registro.ToString(),                                
                                fecha_hora_salida = a.fecha_hora_salida.ToString(),
                                latitud = a.latitud,
                                longitud = a.longitud,
                                s_latitud = a.s_latitud,
                                s_longitud = a.s_longitud
                            }).ToList();
                }                
            }
        }

        public List<genera_asistencias> GeneraListaAsistenciaSupervisorLatam(DateTime fecha1, DateTime fecha2, int proyectos, int[] idUser, string paisReporte)
        {
            int idU = idUser[0];

            using (DataContext db = new DataContext()) 
            {
                //Obtenemos los nombres de paises a partir de su codigo
                var paises = (from c in db.country
                              where paisReporte.Contains(c.code)
                              select c.name
                              ).ToList();

                //Concatenamos la lista de paises en un solo string separado por comas
                string listaPaises = String.Join(",", paises);                                

                if (proyectos > 0)
                {
                    return (from a in db.control_asistencia
                            join i in db.ingeniero on a.id_ingeniero equals i.id_ingeniero
                            join p in db.proyecto on i.id_proyecto equals p.id
                            where
                            (a.fecha_hora_registro >= fecha1 &&
                            (a.fecha_hora_registro <= fecha2) &&
                            paises.Contains(i.pais) &&
                            //i.id_ingeniero == idU &&
                            p.id == proyectos
                            )
                            select new genera_asistencias
                            {
                                Nombre = i.name,
                                last_name_p = i.last_name_p,
                                last_name_m = i.last_name_m,
                                pais = i.pais,
                                Area = p.nombre_proyecto,
                                fecha_hora_registro = a.fecha_hora_registro.ToString(),
                                hora_entrada = a.fecha_hora_registro.ToString(),
                                fecha_hora_salida = a.fecha_hora_salida.ToString(),
                                latitud = a.latitud,
                                longitud = a.longitud,
                                s_latitud = a.s_latitud,
                                s_longitud = a.s_longitud
                            }).ToList();


                }
                else                 
                {
                    return (from a in db.control_asistencia
                            join i in db.ingeniero on a.id_ingeniero equals i.id_ingeniero
                            join p in db.proyecto on i.id_proyecto equals p.id
                            where
                            (a.fecha_hora_registro >= fecha1 &&
                            (a.fecha_hora_registro <= fecha2) &&
                            paises.Contains(i.pais) 
                            //i.id_ingeniero == idU
                            )
                            select new genera_asistencias
                            {
                                Nombre = i.name,
                                last_name_p = i.last_name_p,
                                last_name_m = i.last_name_m,
                                pais = i.pais,
                                Area = p.nombre_proyecto,
                                fecha_hora_registro = a.fecha_hora_registro.ToString(),
                                hora_entrada = a.fecha_hora_registro.ToString(),
                                fecha_hora_salida = a.fecha_hora_salida.ToString(),
                                latitud = a.latitud,
                                longitud = a.longitud,
                                s_latitud = a.s_latitud,
                                s_longitud = a.s_longitud
                            }).ToList();
                }

                
            }
                
        }

        public List<genera_asistencias> GeneraListaAsistenciaIngeniero(DateTime fecha1, DateTime fecha2, int[] idUser) 
        {
            int idU = idUser[0];
            using (DataContext db = new DataContext())
            {
                return (from a in db.control_asistencia
                        join i in db.ingeniero on a.id_ingeniero equals i.id_ingeniero
                        join p in db.proyecto on i.id_proyecto equals p.id
                        where
                            (a.fecha_hora_registro >= fecha1 &&
                            (a.fecha_hora_registro <= fecha2) &&                        
                            i.id_ingeniero == idU
                        )
                        select new genera_asistencias
                        {
                            Nombre = i.name,
                            last_name_p = i.last_name_p,
                            last_name_m = i.last_name_m,
                            pais = i.pais,
                            Area = p.nombre_proyecto,
                            fecha_hora_registro = a.fecha_hora_registro.ToString(),  
                            hora_entrada = a.fecha_hora_registro.ToString(),
                            fecha_hora_salida = a.fecha_hora_salida.ToString(),
                            latitud = a.latitud,
                            longitud = a.longitud,
                            s_latitud = a.s_latitud,
                            s_longitud = a.s_longitud
                        }).ToList();
            }
        }

        public List<datos_paginado> get_lista_personal(int pageIndex,int pageSize)
        {
            using (DataContext db=new DataContext())
            {
                if (pageIndex > 0) { pageIndex = pageIndex - 1; }

                return (from i in db.ingeniero
                        join m in db.movil on i.id_ingeniero equals m.id_ingeniero
                        join p in db.proyecto on i.id_proyecto equals p.id
                        select new datos_paginado {
                            id_ingeniero=i.id_ingeniero,
                            nombre=i.name,
                            appaterno=i.last_name_p,
                            apmaterno=i.last_name_m,
                            area=p.nombre_proyecto,
                            telefono=i.cell_phone,
                            user=i.user,
                            psw=i.password,
                            id_mov = m.id_movil,
                            imei=m.no_imei,
                            marca_telefono=m.modelo,
                            pais = i.pais,
                            id_perfil=i.id_profile
                        }).ToList().Skip(pageSize * pageIndex).Take(pageSize).ToList();


                //return db.ingeniero.ToList().Skip(pageSize * pageIndex).Take(pageSize).ToList();
            }
        }

        public int getTotalPersonal()
        {
            using (DataContext db=new DataContext())
            {
                return db.ingeniero.Count();
            }
        }

        public List<catalogo_celulares> get_catalogo_celulares()
        {
            using (DataContext db=new DataContext())
            {
                return db.catalogo_celulares.Where(x => x.status == 1).ToList();
            }
        }

        public List<country> GetCountries()
        {
            using (DataContext db = new DataContext()) 
            {
                return db.country.ToList();
            }
        }

        public control_asistencia regasistencias(control_asistencia control)
        {
            using (DataContext db=new DataContext())
            {
                db.control_asistencia.Add(control);
                db.SaveChanges();
            }
            return control;
        }

        public string registraSalida(control_asistencia control)
        {
            int result = 0;
            try
            {
                using (DataContext db = new DataContext())
                {                    
                    int lastId=0;                   

                    string lastIdQuery = "SELECT id_asistencia FROM control_asistencia  WHERE id_ingeniero="+ control.id_ingeniero +
                                         " AND date_format(fecha_hora_movil,'%Y-%m-%d')= date_format('" + control.fecha_hora_salida.ToString("yyyy-MM-dd") + "','%Y-%m-%d')" +
                                         " ORDER BY id_asistencia DESC LIMIT 1";
                    lastId = db.Database.SqlQuery<int>(lastIdQuery).FirstOrDefault();

                    if (lastId > 0)
                    {
                        string updateAsistenciaQuery = @"UPDATE control_asistencia SET fecha_hora_salida='" + control.fecha_hora_salida.ToString("yyyy-MM-dd HH:mm:ss") + "', s_latitud=" + control.s_latitud + ", s_longitud=" + control.s_longitud + " WHERE id_asistencia=" + lastId;
                        db.Database.ExecuteSqlCommand(updateAsistenciaQuery);

                        result = 1;                                              
                    }                                      

                }
            }
            catch (Exception ex)
            {
                result = 2;
            }

            return result.ToString();

        }
    }
}
