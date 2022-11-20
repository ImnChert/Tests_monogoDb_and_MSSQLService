using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.RepositoryImplementations
{
	public class SeatRepository : MainMSSQLServerRepository<Seat>
	{
		private IRepository<Category> _categoryRepository;

		public SeatRepository(string connectionString, IRepository<Category> categoryRepository)
			: base(connectionString,
				  "Seats",
				  $@"INSERT INTO Seats (Id,NumberRow,NumberColumn,CategoryId) 
					VALUES(@Id,@NumberRow,@NumberColumn,@CategoryId)",
				  $@"UPDATE Seats SET NumberRow=@NumberRow, NumberColumn=@NumberColumn, CategoryId=@CategoryId
					 WHERE Id=@Id",
				   $"SELECT Id,NumberRow,NumberColumn,CategoryId FROM Seats",
				  $@"SELECT Id,NumberRow,NumberColumn,CategoryId
					FROM Seats 
					WHERE Id=@Id")
		{
			_categoryRepository = categoryRepository;
		}
		public SeatRepository(string connectionString)
			: this(connectionString,
					new CategoryRepository(connectionString))
		{
		}

		protected override Seat GetReader(SqlDataReader sqlDataReader)
			=> new Seat()
			{
				Id = (int)sqlDataReader["Id"],
				NumberColumn = (int)sqlDataReader["NumberColumn"],
				NumberRow = (int)sqlDataReader["NumberRow"],
				Category = _categoryRepository.GetById((int)sqlDataReader["CategoryId"]).Result
			};

		protected override void InsertCommand(SqlCommand sqlCommand, Seat entity)
		{
			sqlCommand.Parameters.Add("@Id", SqlDbType.NVarChar).Value = entity.Id;
			sqlCommand.Parameters.Add("@NumberColumn", SqlDbType.Decimal).Value = entity.NumberColumn;
			sqlCommand.Parameters.Add("@NumberRow", SqlDbType.Decimal).Value = entity.NumberRow;
			sqlCommand.Parameters.Add("@CategoryId", SqlDbType.Decimal).Value = entity.Category.Id;
		}
	}
}
