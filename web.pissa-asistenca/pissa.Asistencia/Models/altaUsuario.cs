using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pissa.Asistencia.Models
{
    public class altaUsuario
    {
        public string nombre { get; set; }
        public string apPaterno { get; set; }
        public string apMaterno { get; set; }
        public string correo { get; set; }
        public string psw { get; set; }
    }
}