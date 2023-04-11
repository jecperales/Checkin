using api.checkin.model;
using Dapper;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Operators;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.checkin.data.Repositories
{
    public class ControlAsistenciaRepository : IControlAsistenciaRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public ControlAsistenciaRepository(MySQLConfiguration connectionString) 
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConectionString);
        }

        public async Task<IEnumerable<control_asistencia>> GetAllAsistencias()
        {
            var db = dbConnection();

            var sql = @"SELECT * FROM control_asistencia";

            return await db.QueryAsync<control_asistencia>(sql, new { });
        }

        public async Task<control_asistencia> GetAsistencia(DateTime fecha, int id)
        {
            var db = dbConnection();

            var sql = @"SELECT * FROM control_asistencia 
                        WHERE  date_format(fecha_hora_registro, '%Y-%m-%d') = @fecha AND 
                        id_ingeniero = @id";

            return await db.QueryFirstOrDefaultAsync<control_asistencia>(sql, new { fecha, id }); 

        }

        public async Task<bool> InsertAsistencia(control_asistencia a)
        {
            var db = dbConnection();

            var sql = @"INSERT INTO control_asistencia 
                       (latitud, longitud, fecha_hora_movil, fecha_hora_registro, id_ingeniero, fecha_hora_salida, s_latitud, s_longitud)
                       VALUES 
                       (@latitud, @longitud, @fecha_hora_movil, @fecha_hora_registro, @id_ingeniero, @fecha_hora_salida, @s_latitud, @s_longitud)";

            var result = await db.ExecuteAsync(sql, new { 
                                                            a.latitud, a.longitud, a.fecha_hora_movil, a.fecha_hora_registro, 
                                                            a.id_ingeniero, a.fecha_hora_salida, a.s_latitud, a.s_longitud
                                                        });

            return result > 0;
        }

        public async Task<bool> UpdateAsistencia(control_asistencia a)
        {
            var db = dbConnection();

            var sql = @"UPDATE control_asistencia SET
                            fecha_hora_salida = @fecha_hora_salida,
                            s_latitud = @s_latitud,
                            s_longitud = @s_longitud
                        WHERE
                            id_asistencia = @id_asistencia";

            var result = await db.ExecuteAsync(sql, new { a.fecha_hora_salida, a.s_latitud, a.s_longitud, a.id_asistencia,});

            return result > 0;

        }

        public async Task<bool> DeleteAsistencia(control_asistencia a)
        {
            var db = dbConnection();

            var sql = "DELETE FROM control_asistencia WHERE id_asistencia = @Id";

            var result = await db.ExecuteAsync(sql, new { Id = a.id_asistencia});

            return result > 0;  
        }
    }
}
