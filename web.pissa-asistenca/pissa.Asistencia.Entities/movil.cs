using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Entities
{
   public class movil
    {
        //[Key]
        //public int id_asistencia { get; set; }
        //public double latitud { get; set; }
        //public double longitud { get; set; }
        //public DateTime fecha_hora_movil { get; set; }
        //public DateTime fecha_hora_registro { get; set; }
        //public int id_ingeniero { get; set; }
        [Key]
        public int id_movil { get; set; }
        public string no_imei  { get; set; }
        public string   modelo   { get; set; }
        public int id_ingeniero  { get; set; }

    }
}
