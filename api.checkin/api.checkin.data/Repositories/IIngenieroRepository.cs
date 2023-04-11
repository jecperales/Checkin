using api.checkin.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.checkin.data.Repositories
{
    public interface IIngenieroRepository
    {
        Task<IEnumerable<Ingeniero>> GetAllIngenieros();
        Task<Ingeniero> GetIngeniero(int id);
        Task<bool> InsertIngeniero(Ingeniero ingeniero);
        Task<bool> UpdateIngeniero(Ingeniero ingeniero);
        Task<bool> DeleteIngeniero(Ingeniero ingeniero);
        Task<Ingeniero> Authenticate(string uid, string pwd);
        
    }
}
