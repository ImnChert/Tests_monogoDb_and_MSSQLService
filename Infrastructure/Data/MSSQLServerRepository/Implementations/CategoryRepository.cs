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

        protected override async Task<List<Category>> GetAllSqlCommand(SqlCommand sqlCommand, Category entity)
        {
			List<Category> categories = new List<Category>();

			using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
			{
				while (await sqlDataReader.ReadAsync())
				{
					categories.Add(new Category()
					{
						Id = (int)sqlDataReader["Id"],
						Name = sqlDataReader["NameCategory"] as string ?? "Undefined",
						Price = (decimal)sqlDataReader["Price"]
					});
				}

				if (categories.Count > 0)
					return categories;
				else
					return null;
			}
		}

        protected override async Task<Category> GetByIdSqlCommand(SqlCommand sqlCommand, int id)
        {
			SqlParameter username = new SqlParameter
			{
				ParameterName = "@id",
				Value = id,
				SqlDbType = SqlDbType.Int,
				Direction = ParameterDirection.Input
			};

			sqlCommand.Parameters.Add(username);

			using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
			{
				if (await sqlDataReader.ReadAsync())
					return new Category()
					{
						Id = (int)sqlDataReader["Id"],
						Name = sqlDataReader["Name"] as string ?? "Undefined",
						Price = (decimal)sqlDataReader["Price"]
					};
				else
					return null;
			}
		}

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
