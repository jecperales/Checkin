using pissa.Asistencia.Entities;
using pissa.Asistencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pissa.Asistencia.ViewModels
{
    public class IndexViewModel : BaseModelo
    {
        public List<datosPagina> movAsistencia { get; set; }
    }
}