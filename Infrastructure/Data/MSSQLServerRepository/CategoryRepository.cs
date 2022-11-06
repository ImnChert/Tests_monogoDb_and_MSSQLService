using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository
{
	internal class CategoryRepository : IRepository<Category>
	{
		private readonly string _connectionString;

		public CategoryRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public Task<bool> DeleteAsync(Category entity)
		{
			throw new NotImplementedException();
		}

		public Task<List<Category>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Category> GetById(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> InsertAsync(Category entity)
		{
			string query = $"INSERT INTO DeletedUsers (UserId,DateTime,Reason) VALUES(@UserId,@DateTime,@Reason)";

			using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
			{
				await sqlConnection.OpenAsync();

				using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = entity.UserId;
					sqlCommand.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = entity.DateTime;
					sqlCommand.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = entity.Reason;
					await sqlCommand.ExecuteNonQueryAsync();
				}

				return true;
			}
		}

		public Task<bool> UpdateAsync(Category entity)
		{
			throw new NotImplementedException();
		}
	}
}
