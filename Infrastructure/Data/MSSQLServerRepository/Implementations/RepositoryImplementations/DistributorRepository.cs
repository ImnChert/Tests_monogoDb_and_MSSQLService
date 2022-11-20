using ApplicationCore.Domain.Core.Models.Cinema.Films;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using Infrastructure.Data.MSSQLServerRepository.Connection.Extensions;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	public class DistributorRepository : MSSQLRepository<Distributor>
	{
		public DistributorRepository(string connectionString)
			: base(connectionString,
				  "Distributors",
				  $"INSERT INTO Distributors (Id,NameCompany) VALUES(@Id,@NameCompany)",
				  $"UPDATE Distributors SET NameCompany=@NameCompany WHERE Id=@Id",
				  $"SELECT Id,NameCompany FROM Distributors",
				  $"SELECT Id,NameCompany FROM Distributors WHERE Id=@id")
		{
		}

		protected override Distributor GetReader(SqlDataReader sqlDataReader)
			=> new Distributor()
			{
				Id = (int)sqlDataReader["Id"],
				NameCompany = sqlDataReader["Name"] as string ?? "Undefined",
			};

		protected override void InsertCommand(SqlCommand sqlCommand, Distributor entity)
		{
			sqlCommand.Parameters.Add("@NameCompany", SqlDbType.NVarChar).Value = entity.NameCompany;
		}
	}
}
