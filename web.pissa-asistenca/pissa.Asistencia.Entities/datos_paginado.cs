using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Entities
{
   public class datos_paginado
    {
        public int id_ingeniero { get; set; }
        public string nombre { get; set; }
        public string appaterno  { get; set; }
        public string apmaterno  { get; set; }
        public string celular  { get; set; }
        public string user  { get; set; }
        public string psw  { get; set; }
        public string area  { get; set; }
        public int id_mov { get; set; }
        public string imei  { get; set; }
        public string telefono  { get; set; }
        public string marca_telefono { get; set; }
        public string pais { get; set; }
        public int id_perfil { get; set; }
    }
}
