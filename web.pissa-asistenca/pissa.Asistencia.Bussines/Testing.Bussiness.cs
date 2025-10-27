using pissa.Asistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Bussines
{
    public class Testing
    {
        public int saveDateTime(control_asistencia asistencia)
        {
            return new Dao.Testing().saveDateTime(asistencia);
            
        }
    }
}
