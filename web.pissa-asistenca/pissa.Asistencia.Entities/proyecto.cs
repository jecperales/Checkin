using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Entities
{
   public class proyecto
    {
        [Key]
        public int id { get; set; }
        public string nombre_proyecto { get; set; }
        public int estatus { get; set; }
    }
}
