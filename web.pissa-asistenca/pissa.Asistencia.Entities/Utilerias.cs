using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Entities
{
   public class Utilerias
    {
            public validaRetardo validarRetardo(DateTime fecha_registro)
            {
            validaRetardo valida = new validaRetardo();
            TimeSpan retardo = new TimeSpan(09, 15, 0);
            TimeSpan hora_entrada = new TimeSpan(08, 30, 0);
            //TimeSpan hora_entrada = new TimeSpan(09, 15, 0);
            DateTime hora_chek = fecha_registro;
            DateTime horaEntrada = fecha_registro.Date + hora_entrada;
            TimeSpan span = (hora_chek - horaEntrada);

            var validar=fecha_registro.ToString("HH:mm").Split(':');

                if(int.Parse(validar[0])<=8 || int.Parse(validar[0]) <= 9)
                {
                    if (span.Minutes >= 15 || span.Hours >= 1)
                    {
                        valida.es_retardo = "Retardo";
                        valida.diferencia_retardo = span.Hours + ":" + span.Minutes + ":" + span.Seconds;
                    }
                    else
                    {
                        valida.es_retardo = "No aplica";
                        valida.diferencia_retardo = "";
                    }
                }
                else
                {
                    valida.es_retardo = "No aplica";
                    valida.diferencia_retardo = "";
                }
                    
            return valida;
             }


        public string primer_dia_mes()
        {
            DateTime date = DateTime.Now;
            //Asi obtenemos el primer dia del mes actual
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
        
            return oPrimerDiaDelMes.ToString("yyyy-MM-dd");
        }

        public string ultimo_dia_mes()
        {
            //Primero obtenemos el día actual
            DateTime date = DateTime.Now;
            //Asi obtenemos el primer dia del mes actual
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            //Y de la siguiente forma obtenemos el ultimo dia del mes
            //agregamos 1 mes al objeto anterior y restamos 1 día.
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);
            return oUltimoDiaDelMes.ToString("yyyy-MM-dd");
        }


    }


    public class validaRetardo
    {
        public string es_retardo { get; set; }
        public string diferencia_retardo { get; set; }
    }

}
