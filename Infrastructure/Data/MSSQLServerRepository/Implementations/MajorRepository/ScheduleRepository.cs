using ApplicationCore.Domain.Core.Models.Cinema;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
    internal class ScheduleRepository : MainMSSQLServerRepository<Schedule>
    {
        public ScheduleRepository(string connectionString, string tableName, string insertQuery, string updateQuery, string getAllQuery, string getByIdQuery)
            : base(connectionString,
                  tableName,
                  insertQuery,
                  updateQuery,
                  getAllQuery,
                  getByIdQuery)
        {
        }

        protected override Schedule GetReader(SqlDataReader sqlDataReader)
        {
            throw new NotImplementedException();
        }

        protected override void InsertCommand(SqlCommand sqlCommand, Schedule entity)
        {
            throw new NotImplementedException();
        }
    }
}
