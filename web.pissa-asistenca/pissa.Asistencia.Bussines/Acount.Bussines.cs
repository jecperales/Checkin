using pissa.Asistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Bussines
{
   public class Acount
    {

        public usuarioLogeado logear(string correo, string psw)
        {
            return new Dao.Acount().logear(correo,psw);
        }
        public bool regIngeniero(preRegistro registro)
        {
            return new Dao.Acount().regIngeniero(registro);
        }

        public mi_perfil getPerfil(int id_user)
        {
            return new Dao.Acount().getPerfil(id_user);
        }

        public int getValidaProyecto(int id_user)
        {
            return new Dao.Acount().getValidaProyecto(id_user);
        }

        public List<proyecto> getProyectos()
        {
            return new Dao.Acount().getProyectos();
        }

        public List<proyecto> getProyectosPrivilegios(int id_user)
        {
            return new Dao.Acount().getProyectosPrivilegios(id_user);
        }

        public bool actualizaproyecto(int id_proyecto, int id_user)
        {
            return new Dao.Acount().actualizaproyecto(id_proyecto, id_user);
        }

        public List<ingeniero> getIngenieros()
        {
            return new Dao.Acount().getIngenieros();
        }

        public ingeniero getIngeniero(int id_user)
        {
            return new Dao.Acount().getIngeniero(id_user);
        }

        public movil getMobil(int id_user)
        {
            return new Dao.Acount().getMobil( id_user);

        }

        public ingeniero altaIngeniero(ingeniero temp_inge)
        {
            return new Dao.Acount().altaIngeniero(temp_inge);
        }

        public bool registra_personal(regitraPersonal altapersonal)
        {
            bool status = false;
            var inge = new Dao.Acount().altaIngeniero(altapersonal.ing);
            movil reg_movil=null;
            if (inge.id_ingeniero!=0) {

                altapersonal.mov.id_ingeniero = altapersonal.ing.id_ingeniero;
                var existe = new Dao.Acount().getmovil(altapersonal.ing.id_ingeniero);
                if (existe==null || existe.no_imei!=altapersonal.mov.no_imei || existe.modelo != altapersonal.mov.modelo) {
                    //if (altapersonal.mov.modelo == null)
                    //{
                    //    altapersonal.mov.modelo = "";
                    //    altapersonal
                    //}
                    reg_movil = new Dao.Acount().alta_movil(altapersonal.mov);
                }
                
            }

            if (inge.id_ingeniero!=0 ) {
                status = true;
            }
            else { status = false; }

            return status;
        }

        public movil altaMovil(movil temp_movil)
        {
            return new Dao.Acount().alta_movil(temp_movil);
        }
    }
}
