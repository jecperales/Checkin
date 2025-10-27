using api.checkin.model;
using Dapper;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
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

        public async Task<string> InsertAsistencia(control_asistencia a)
        {
            var db = dbConnection();
            var result="";
            
            
            var sqlEntrada = "SELECT * FROM control_asistencia WHERE id_ingeniero IN (" + a.id_ingeniero.ToString() + 
                             ") AND date_format(fecha_hora_registro,'%Y-%m-%d') = date_format('" + a.fecha_hora_registro.ToString("yyyy-MM-dd") + "','%Y-%m-%d');";

            var sql = @"INSERT INTO control_asistencia 
                       (latitud, longitud, fecha_hora_movil, fecha_hora_registro, id_ingeniero, fecha_hora_salida, s_latitud, s_longitud)
                       VALUES 
                       (@latitud, @longitud, @fecha_hora_movil, @fecha_hora_registro, @id_ingeniero, @fecha_hora_salida, @s_latitud, @s_longitud)";

            var hayEntrada = await db.QueryAsync<control_asistencia>(sqlEntrada);

            if (hayEntrada.Count() > 0)
            {
                // Result = -1 cuando ya existe un registro existente 
                result = "-1";
            }
            else
            {
                var asistencia = await db.ExecuteAsync(sql, new
                {
                    a.latitud,
                    a.longitud,
                    a.fecha_hora_movil,
                    a.fecha_hora_registro,
                    a.id_ingeniero,
                    a.fecha_hora_salida,
                    a.s_latitud,
                    a.s_longitud
                });
                // Result =  1 cuando la insercion es exitosa
                // Result =  0 cuando no se inserto el record
                result = "1";

            }

            return result;
        }

        public async Task<int> UpdateAsistencia(control_asistencia a)
        {
            var db = dbConnection();

            var sql = @"UPDATE control_asistencia SET
                            fecha_hora_salida = @fecha_hora_salida,
                            s_latitud = @s_latitud,
                            s_longitud = @s_longitud
                        WHERE
                            id_asistencia = @id_asistencia";

            var result = await db.ExecuteAsync(sql, new { a.fecha_hora_salida, a.s_latitud, a.s_longitud, a.id_asistencia,});

            return result;

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
