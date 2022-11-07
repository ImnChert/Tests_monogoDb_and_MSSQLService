using ApplicationCore.Domain.Core.Models.Cinema;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations
{
    internal class CategoryRepository : MainMSSQLServerRepository<Category>
    {
		public CategoryRepository(string connectionString) 
            : base(connectionString, "Categores", 
                  $"INSERT INTO Categores (Id,NameCategory,Price) VALUES(@Id,@NameCategory,@Price)",
				  $"UPDATE Categores SET NameCategory=@Name, Price=@Price WHERE Id=@Id",
				  $"SELECT Id, NameCategory, Price FROM Categores",
                  $"SELECT Id, NameCategory, Price FROM Categores WHERE Id=@id")
        {
        }

		protected override Category GetReader(SqlDataReader sqlDataReader)
			=> new Category()
			{
				Id = (int)sqlDataReader["Id"],
				Name = sqlDataReader["Name"] as string ?? "Undefined",
				Price = (decimal)sqlDataReader["Price"]
			};
		

		protected override async Task<bool> InsertSqlCommand(SqlCommand sqlCommand, Category entity)
        {
			sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = entity.Id;
			UpdateCommand(sqlCommand, entity);

			await sqlCommand.ExecuteNonQueryAsync();

            return true;
		}

        protected override async Task<bool> UpdateSqlCommand(SqlCommand sqlCommand, Category entity)
        {

            UpdateCommand(sqlCommand, entity);

			await sqlCommand.ExecuteNonQueryAsync();

			return true;
		}

		private void UpdateCommand(SqlCommand sqlCommand, Category entity)
        {
			sqlCommand.Parameters.Add("@NameCategory", SqlDbType.DateTime).Value = entity.Name;
			sqlCommand.Parameters.Add("@Price", SqlDbType.NVarChar).Value = entity.Price;
		}
	}
}
