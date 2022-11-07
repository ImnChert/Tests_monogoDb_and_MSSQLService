using ApplicationCore.Domain.Core.Models.Cinema.Films;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations
{
    internal class FilmRepository : MainMSSQLServerRepository<Film>
    {
        protected override Film GetReader(SqlDataReader sqlDataReader)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> InsertSqlCommand(SqlCommand sqlCommand, Film entity)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> UpdateSqlCommand(SqlCommand sqlCommand, Film entity)
        {
            throw new NotImplementedException();
        }
    }
}
