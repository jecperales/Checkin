using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Entities
{
   public class privilegios
    {
        [Key]
        public int id { get; set; }
        public int id_user { get; set; }
        public int id_proyecto { get; set; }
        public int status { get; set; }
    }
}
