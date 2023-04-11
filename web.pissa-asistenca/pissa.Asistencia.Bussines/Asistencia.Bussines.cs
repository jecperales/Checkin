using pissa.Asistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Bussines
{
    public class Asistencia
    {
        public control_asistencia getAsistenciaToday(int ingenieroId) 
        {
            return new Dao.Asistencia().getAsistenciaTodayByIngenieroId(ingenieroId);
        }
        public List<genera_asistencias> generaAsistencias(string fecha1, string fecha2, int proyectos, int[] idUser)
        {
            return new Dao.Asistencia().generaAsistencias(fecha1, fecha2, proyectos,idUser);
        }
        public List<genera_asistencias> GeneraListaAsistenciaAdmin(DateTime fecha1, DateTime fecha2, int proyectos, int[] idUser)
        {
            return new Dao.Asistencia().GeneraListaAsistenciaAdmin(fecha1, fecha2, proyectos, idUser);
        }
        public List<genera_asistencias> GeneraListaAsistenciaIngeniero(DateTime fecha1, DateTime fecha2, int[] idUser)
        {
            return new Dao.Asistencia().GeneraListaAsistenciaIngeniero(fecha1, fecha2, idUser);
        }
        public List<genera_asistencias> GeneraListaAsistenciaSupervisorLatam(DateTime fecha1, DateTime fecha2, int proyectos, int[] idUser, string paisReporte)
        {
            return new Dao.Asistencia().GeneraListaAsistenciaSupervisorLatam(fecha1, fecha2, proyectos, idUser, paisReporte);
        }

        public control_asistencia regasistencias(control_asistencia control)
        {
            return new Dao.Asistencia().regasistencias(control);
        }

        public string registraSalida(control_asistencia control)
        {
            return new Dao.Asistencia().registraSalida(control);
        }

        public List<catalogo_celulares> get_catalogo_celulares()
        {
            return new Dao.Asistencia().get_catalogo_celulares();
        }

        public List<country> GetCountries()
        {
            return new Dao.Asistencia().GetCountries();
        }

        public paginaPersonal paginado_personal(int pageIndex, int pageSize)
        {

            paginaPersonal pag = new paginaPersonal();
            pag.Total = new Dao.Asistencia().getTotalPersonal();
            pag.Lista = new Dao.Asistencia().get_lista_personal(pageIndex, pageSize);
            return pag;
        }
    }
}
