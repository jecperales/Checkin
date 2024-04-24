using pissa.Asistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Bussines
{
    public class Perfil
    {
        public List<perfil> getPerfilList() 
        {
            return new Dao.Perfil().getPerfilList();
        }
    }
}
