using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Entities
{
   public class genera_asistencias
    {
        public string Nombre { get; set; }
        public string last_name_p { get; set; }
        public string last_name_m { get; set; }
        public string pais { get; set; }
        public string Area { get; set; }
        public string fecha_hora_registro { get; set; }      
        public string fecha_hora_salida { get; set; }
        public string hora_entrada { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public string hora_salida { get; set; }
        public double? s_latitud { get; set; }
        public double? s_longitud { get; set; }

    }
}
