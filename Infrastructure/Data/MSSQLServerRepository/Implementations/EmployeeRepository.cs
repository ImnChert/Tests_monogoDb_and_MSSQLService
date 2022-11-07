using ApplicationCore.Domain.Core.Models.Roles.Staff;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations
{
    internal class EmployeeRepository : MainMSSQLServerRepository<Employee>
    {
        public EmployeeRepository(string connectionString, string tableName, string insertQuery, string updateQuery, string getAllQuery, string getByIdQuery) 
            : base(connectionString, 
                  tableName, 
                  insertQuery, 
                  updateQuery, 
                  getAllQuery, 
                  getByIdQuery)
        {
        }

        protected override Employee GetReader(SqlDataReader sqlDataReader)
        {
            throw new NotImplementedException();
        }

        protected override void InsertCommand(SqlCommand sqlCommand, Employee entity)
        {
            throw new NotImplementedException();
        }
    }
}
