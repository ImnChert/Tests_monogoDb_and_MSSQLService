using ApplicationCore.Domain.Core.Models.Cinema.Films;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
    internal class FilmRepository : MainMSSQLServerRepository<Film>
    {
        public FilmRepository(string connectionString, string tableName, string insertQuery, string updateQuery, string getAllQuery, string getByIdQuery)
            : base(connectionString,
                  tableName,
                  insertQuery,
                  updateQuery,
                  getAllQuery,
                  getByIdQuery)
        {
        }

        protected override Film GetReader(SqlDataReader sqlDataReader)
        {
            throw new NotImplementedException();
        }

        protected override void InsertCommand(SqlCommand sqlCommand, Film entity)
        {
            throw new NotImplementedException();
        }
    }
}
