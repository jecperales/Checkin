using pissa.Asistencia.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pissa.Asistencia.Models
{
    public class tipoProyecto
    {
        public int id { get; set; }
        public string label { get; set; }
    }

    public class Modelos
    {
        public tipoProyecto tipoProyecto { get; set; }
        public IndexViewModel IndexViewModel { get; set; }

    }


}