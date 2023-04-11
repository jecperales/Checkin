using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Entities
{
   public class usuarioLogeado
    {
        public int id_profile;

        public int id { get; set; }
        public List<privilegios> Listaproyecto { get; set; }
        public string username { get; set; }

        public string pais_reporte { get; set; }
        public int id_proyecto { get; set; }
    }

 
    public class proy
    {
        public int id_proyecto { get; set; }
    }


}
