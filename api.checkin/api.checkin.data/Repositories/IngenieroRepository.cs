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
    public class IngenieroRepository : IIngenieroRepository
    {
        private readonly MySQLConfiguration _connectionString;
        public IngenieroRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection() 
        {
            return new MySqlConnection(_connectionString.ConectionString);
        }


        public async Task<IEnumerable<Ingeniero>> GetAllIngenieros()
        {
            var db = dbConnection();

            var sql = @"SELECT * FROM ingeniero ORDER BY id_ingeniero";

            return await db.QueryAsync<Ingeniero>(sql, new { });
        }

        public async Task<Ingeniero> GetIngeniero(int id)
        {
            var db = dbConnection();

            var sql = @"SELECT 
                        id_ingeniero, engineer_id, name, last_name_p, last_name_m, street,
                        suburb, town, state, cell_phone, user, password, id_profile, id_proyecto,
                        pais, pais_reporte FROM ingeniero WHERE id_ingeniero = @id";

            //var result = db.QueryFirstOrDefaultAsync<Ingeniero>(sql, new { id });
            //return result;

            return await db.QueryFirstOrDefaultAsync<Ingeniero>(sql, new { id });
        }

        public async Task<Ingeniero> Authenticate(string uid, string pwd)
        {
            var db = dbConnection();

            var sql = @"SELECT * FROM ingeniero WHERE user = @user AND password = @password";

            return await db.QueryFirstOrDefaultAsync<Ingeniero>(sql, new { user = uid, password = pwd });
        }

        public async Task<bool> InsertIngeniero(Ingeniero i)
        {
            var db = dbConnection();

            var sql = @"INSERT INTO ingeniero 
                        (name, last_name_p, last_name_m, street,
                        suburb, town, state, cell_phone, user, password, id_profile, id_proyecto,
                        pais, pais_reporte) VALUES 
                        (@name, @last_name_p, @last_name_m, @street,
                        @suburb, @town, @state, @cell_phone, @user, @password, @id_profile, @id_proyecto,
                        @pais, @pais_reporte)";

            var result = await db.ExecuteAsync(sql, new { 
                i.name, i.last_name_p, i.last_name_m, i.street, i.suburb, i.town, i.state, i.cell_phone, i.user, i.password, i.id_profile, i.id_proyecto,
                i.pais, i.pais_reporte
            });

            return result > 0;
            
        }

        public async Task<bool> UpdateIngeniero(Ingeniero i)
        {
            var db = dbConnection();

            var sql = @"UPDATE ingeniero 
                        SET 
                        name = @name, 
                        last_name_p = @last_name_p, 
                        last_name_m = @last_name_m, 
                        street = @street,
                        suburb = @suburb, 
                        town = @town, 
                        state = @state, 
                        cell_phone = @cell_phone, 
                        user = @user, 
                        password = @password, 
                        id_profile = @id_profile, 
                        id_proyecto = @id_proyecto,
                        pais = @pais, 
                        pais_reporte = @pais_reporte
                        WHERE id_ingeniero = @Id";

            var result = await db.ExecuteAsync(sql, new
            {
                Id = i.id_ingeniero,
                i.name,
                i.last_name_p,
                i.last_name_m,
                i.street,
                i.suburb,
                i.town,
                i.state,
                i.cell_phone,
                i.user,
                i.password,
                i.id_profile,
                i.id_proyecto,
                i.pais,
                i.pais_reporte
            });

            return result > 0;
        }

        public async Task<bool> DeleteIngeniero(Ingeniero i)
        {
            var db = dbConnection();

            var sql = @"DELETE FROM ingeniero WHERE id_ingeniero=@id";

            var result = await db.ExecuteAsync(sql, new { id = i.id_ingeniero });

            return result > 0;

        }

        
    }
}
