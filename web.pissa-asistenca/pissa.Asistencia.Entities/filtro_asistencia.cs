using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Entities
{
    public class filtro_asistencia
    {
        public int profile { get; set; }
        public DateTime fecha1 { get; set; }
        public DateTime fecha2 { get; set; }
        public int proyectos { get; set; }
        public string paisReporte { get; set; }
    }
}
