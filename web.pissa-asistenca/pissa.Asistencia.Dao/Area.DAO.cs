using pissa.Asistencia.Entities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Dao
{
    public class Area
    {
        public List<proyecto> getProyectos() {
            using (DataContext db = new DataContext())
            {
                //var proyectosLst = (from p in db.proyecto
                //                    select p)
                //                    .Where(p => p.estatus == 1).ToList();

                var proyectosLst = db.proyecto.Where(p => p.estatus == 1).ToList();

                return proyectosLst;                                                   
            }
        }

        
        public int insertarProyecto(proyecto proyecto) 
        {             
            using (DataContext db = new DataContext()) 
            { 
                db.proyecto.Add(proyecto);
                var created = db.SaveChanges();

                return created;
            }            
        }

        public int actualizarProyecto(proyecto proyecto) 
        {
            using (DataContext db = new DataContext())
            {
                int res=0;
                var entry = db.proyecto.Where(p => p.id == proyecto.id).FirstOrDefault();
                if (entry != null) {
                    entry.nombre_proyecto = proyecto.nombre_proyecto;
                    res = db.SaveChanges();
                }

                return res;
            }
        }

        public int eliminarProyecto(int idProyecto)         
        {
            int res = 0;

            using (DataContext db = new DataContext()) 
            {                
                var proyecto = db.proyecto.Where(p => p.id == idProyecto).FirstOrDefault();

                if (proyecto != null) 
                {
                    proyecto.estatus = 0;
                    res = db.SaveChanges();
                }                
            }

            if (res > 0) 
            {
                desactivarTodosLosIngenieros(idProyecto);
            }
           
            return res;
        }

        public List<ingeniero> getIngenieros(int idProyecto)
        {
            using (DataContext db = new DataContext()) 
            {
                var ingenierosLst = (from i in db.ingeniero
                                     select i).Where(ingeniero => ingeniero.id_proyecto == idProyecto && ingeniero.status == "1").ToList();

                return ingenierosLst;            
            }
        }

        public int desactivarIngeniero(int idIngeniero) 
        {
            using (DataContext db = new DataContext()) 
            {
                int res = 0;

                var entry = db.ingeniero.Where(i => i.id_ingeniero == idIngeniero).FirstOrDefault();

                if (entry != null) 
                {
                    entry.status = "0";
                    res= db.SaveChanges();
                }

                return res;
            }
        }

        public int desactivarTodosLosIngenieros(int idProyecto) 
        {
            using (DataContext db = new DataContext())
            {
                var res = db.Database.ExecuteSqlCommand("UPDATE ingeniero SET status = '0' WHERE id_proyecto = {0}", idProyecto);
                return res;
            }
        }

    }
}
