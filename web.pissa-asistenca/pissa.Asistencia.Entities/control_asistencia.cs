using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Entities
{
   public class control_asistencia
    {
        [Key]
        public int id_asistencia { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public DateTime fecha_hora_movil { get; set; }
        public DateTime fecha_hora_registro { get; set; }
        public int id_ingeniero { get; set; }

        //Agregadas por José Enrique Cruz Perales  2022/03/08
        public DateTime fecha_hora_salida { get; set; }
        public double s_latitud { get; set; }
        public double s_longitud { get; set; }
    }
}
