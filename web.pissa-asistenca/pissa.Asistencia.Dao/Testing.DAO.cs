using pissa.Asistencia.Entities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Dao
{
    public class Testing {

        public int saveDateTime(control_asistencia asistencia) 
        {
            using (DataContext db = new DataContext())
            {
                db.control_asistencia.Add(asistencia);
                var created = db.SaveChanges();

                return created;
            }

        }
    }
    
    
}
