using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Entities
{
   public class paginaPersonal
    {
        public int Total { get; set; }
        public List<datos_paginado> Lista { get; set; }
    }
}
