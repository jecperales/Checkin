using pissa.Asistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Bussines
{
    public class Area
    {
        public List<proyecto> getProyectos()
        {
            return new Dao.Area().getProyectos();
        }

        public int insertarProyecto(proyecto proyecto)
        {
            return new Dao.Area().insertarProyecto(proyecto);
        }

        public int actualizarProyecto(proyecto proyecto)
        {
            return new Dao.Area().actualizarProyecto(proyecto);
        }

        public int eliminarProyecto(int idProyecto) 
        {
            return new Dao.Area().eliminarProyecto(idProyecto);
        }

        public List<ingeniero> getIngenieros(int idProyecto)
        {
            return new Dao.Area().getIngenieros(idProyecto);
        }

        public int desactivarIngeniero(int idIngeniero) 
        {
            return new Dao.Area().desactivarIngeniero(idIngeniero);
        }

        public int desactivarTodosLosIngenieros(int idProyecto) 
        {
            return new Dao.Area().desactivarTodosLosIngenieros(idProyecto);
        }
    }
}
