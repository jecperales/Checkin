using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Entities
{
    public class perfil
    {
        [Key]
        public int id_profile { get; set; }
        public string name { get; set; }
        public string rol { get; set; }
    }
}
