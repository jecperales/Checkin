using api.checkin.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.checkin.data.Repositories
{
    public interface IControlAsistenciaRepository
    {
        Task<IEnumerable<control_asistencia>> GetAllAsistencias();
        Task<control_asistencia> GetAsistencia(DateTime fecha, int id);
        Task<bool> InsertAsistencia(control_asistencia asistencia);
        Task<bool> UpdateAsistencia(control_asistencia asistencia);
        Task<bool> DeleteAsistencia(control_asistencia asistencia);
    }
}
