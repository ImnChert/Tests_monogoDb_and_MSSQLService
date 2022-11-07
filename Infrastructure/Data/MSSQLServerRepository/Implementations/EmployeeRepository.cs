using ApplicationCore.Domain.Core.Models.Roles.Staff;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations
{
    internal class EmployeeRepository : MainMSSQLServerRepository<Employee>
    {
        protected override Employee GetReader(SqlDataReader sqlDataReader)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> InsertSqlCommand(SqlCommand sqlCommand, Employee entity)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> UpdateSqlCommand(SqlCommand sqlCommand, Employee entity)
        {
            throw new NotImplementedException();
        }
    }
}
