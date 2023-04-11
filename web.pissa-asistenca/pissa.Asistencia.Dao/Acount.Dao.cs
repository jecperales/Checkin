using pissa.Asistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.Entity;

namespace pissa.Asistencia.Dao
{
   public  class Acount
    {

        public bool regIngeniero(preRegistro registro)
        {
            bool status = false;
            try {
                using (DataContext db = new DataContext())
                {
                    var existe = db.ingeniero.Where(x=> x.user==registro.correo && x.password==registro.psw).FirstOrDefault();
                    if (existe == null)
                    {
                        var max = db.ingeniero.Max(x=> x.id_ingeniero)+1;

                        db.ingeniero.Add(new ingeniero
                        {
                            id_ingeniero = max,
                            engineer_id = max.ToString(),
                            name = registro.nombre,
                            last_name_p = registro.apPaterno,
                            last_name_m = registro.apMaterno,
                            state = "",
                            street = "",
                            suburb = "",
                            cell_phone = "",
                            town = "",
                            user = registro.correo,
                            password = registro.psw,
                            id_proyecto = 0,
                            id_profile = 2,
                           
                        });
                        db.SaveChanges();
                    }

                }
                status = true;
            } catch (Exception e1) { }
           
                return status;
        }

        public List<ingeniero> getIngenieros()
        {
            using (DataContext db=new DataContext())
            {
                return db.ingeniero.Where(x=> x.id_proyecto!=0).ToList();
            }
        }

        public List<proyecto> getProyectosPrivilegios(int id_user)
        {
            List<proyecto> lista = new List<proyecto>();

            using (DataContext db=new DataContext())
            {
                lista = (from p in db.proyecto
                        join pr in db.privilegios on p.id equals pr.id_proyecto
                        where pr.id_user==id_user
                        select new  {
                            id=p.id,
                            nombre_proyecto =p.nombre_proyecto,
                            estatus =p.estatus
                        }).ToList().Select( x=> new proyecto {
                            id = x.id,
                            nombre_proyecto = x.nombre_proyecto,
                            estatus = x.estatus
                        }).ToList();
            }
            return lista;
        }

        public movil getMobil(int id_user)
        {
            using (DataContext db=new DataContext())
            {
                return db.movil.Where(x=> x.id_ingeniero==id_user).FirstOrDefault();
            }
        }

        public ingeniero getIngeniero(int id_user)
        {
            using (DataContext db=new DataContext())
            {
                return db.ingeniero.Where(x=> x.id_ingeniero==id_user).FirstOrDefault();
            }
        }

        public mi_perfil getPerfil(int id_user)
        {
            mi_perfil per = new mi_perfil();
            using (DataContext db = new DataContext())
            {

                per = (from i in db.ingeniero
                           join m in db.movil on i.id_ingeniero equals m.id_ingeniero
                           where i.id_ingeniero==id_user
                           select new mi_perfil {
                               nombre=i.name,
                               appaterno=i.last_name_p,
                               apmaterno=i.last_name_m,
                               emei=m.no_imei,
                               psw=i.password
                           }).FirstOrDefault();
            }
            return per;
        }

       

        public movil getmovil(int id_ingeniero)
        {
            using (DataContext db=new DataContext()) {
                return db.movil.Where(x=> x.id_ingeniero==id_ingeniero).FirstOrDefault();
            }
        }

        public movil alta_movil(movil mov)
        {          
            using (DataContext db=new DataContext())
            {
                //Validar que mov.id_movil no venga vacio

                
                var existe = db.movil.Where(x => x.id_movil == mov.id_movil).FirstOrDefault();
                if (existe==null)
                {
                    db.movil.Add(mov);
                    db.SaveChanges();
                }
                else
                {
                    //db.Entry(existe).CurrentValues.SetValues(mov.no_imei);
                    //db.Entry(existe).CurrentValues.SetValues(mov.modelo);
                    
                    existe.no_imei = mov.no_imei;
                    existe.modelo = mov.modelo;
                    db.SaveChanges();
                }
            }
                return mov;
        }

        public bool actualizaproyecto(int id_proyecto, int id_user)
        {
            bool status = false;
            try {
                using (DataContext db = new DataContext())
                {
                    var existe = db.ingeniero.Where(x => x.id_ingeniero == id_user).FirstOrDefault();
                    if (existe != null)
                    {
                        existe.id_proyecto = id_proyecto;
                    }
                    db.SaveChanges();
                }
                status = true;
            } catch (Exception e1) { status = false; }
            return status;
        }

        public List<proyecto> getProyectos()
        {
            using (DataContext db=new DataContext())
            {
                return db.proyecto.Where(x=> x.estatus==1).ToList();
            }
        }

        public int getValidaProyecto(int id_user)
        {
            using (DataContext db=new DataContext())
            {
                return db.ingeniero.Where(x => x.id_ingeniero == id_user).Select(x=> x.id_proyecto).FirstOrDefault();
            }
        }

        public usuarioLogeado logear(string correo, string psw)
        {
            usuarioLogeado logeado = new usuarioLogeado();

            using (DataContext db=new DataContext())
            {
                var existe_user = db.ingeniero.Where(x=> x.user==correo && x.password==psw).FirstOrDefault();

                if (existe_user!=null) {
                    logeado.id = existe_user.id_ingeniero;
                    logeado.username = existe_user.name+" "+existe_user.last_name_p+" "+ existe_user.last_name_m;
                    logeado.id_proyecto = existe_user.id_proyecto;
                    logeado.id_profile = existe_user.id_profile;
                    logeado.pais_reporte = existe_user.pais_reporte;
                    logeado.Listaproyecto = db.privilegios.Where(x=> x.id_user==existe_user.id_ingeniero).ToList();
                }
               
            }
            return logeado;
        }

        public ingeniero altaIngeniero(ingeniero ing)
        {
            try {
                using (DataContext db = new DataContext())
                {
                    var existe = db.ingeniero.Where(x => x.id_ingeniero == ing.id_ingeniero).FirstOrDefault();
                    if (existe == null)
                    {
                        db.ingeniero.Add(ing);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(existe).CurrentValues.SetValues(ing);
                        db.SaveChanges();
                    }
                }
                return ing;
            } catch (Exception e1) { return null; }
            
        }

    }
}
