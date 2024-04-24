using pissa.Asistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


namespace pissa.Asistencia.Dao
{
    public class Perfil
    {
        public List<perfil> getPerfilList() {

            using (DataContext db = new DataContext()) 
            {
                var perfilLst = (from profile in db.perfil
                                 select profile).ToList();

                return perfilLst;
                                
            }
        }
    }
}
