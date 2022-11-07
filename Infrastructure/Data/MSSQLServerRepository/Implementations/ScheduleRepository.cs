using ApplicationCore.Domain.Core.Models.Cinema;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations
{
    internal class ScheduleRepository : MainMSSQLServerRepository<Schedule>
    {
        protected override Schedule GetReader(SqlDataReader sqlDataReader)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> InsertSqlCommand(SqlCommand sqlCommand, Schedule entity)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> UpdateSqlCommand(SqlCommand sqlCommand, Schedule entity)
        {
            throw new NotImplementedException();
        }
    }
}
