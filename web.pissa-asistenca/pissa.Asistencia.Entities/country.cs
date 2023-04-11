using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Entities
{
    public class country
    {
        [Key]
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }
}
