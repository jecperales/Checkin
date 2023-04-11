using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Entities
{
   public class ingeniero
    {
                
        [Key]
        public int id_ingeniero { get; set; }
        public string engineer_id { get; set; }
        public string name { get; set; }
        public string last_name_p { get; set; }
        public string last_name_m { get; set; }
        public string street { get; set; }
        public string suburb { get; set; }
        public string town { get; set; }
        public string state { get; set; }
        public string cell_phone { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string pais { get; set; }
        public string pais_reporte { get; set; }
        public int id_profile { get; set; }
        public int id_proyecto { get; set; }
        

    }
}
