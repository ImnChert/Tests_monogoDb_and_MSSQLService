using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Interfaces.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace Infrastructure.Data.MSSQLServerRepository.Connection
{
	internal abstract class MainMSSQLServerRepository<T>
		 : IRepository<T> where T : EntityBase
	{
		protected readonly string _connectionString;
		protected readonly string _tableName;
		protected readonly string _deleteQuery;
		protected readonly string _insertQuery;
		protected readonly string _updateQuery;
		protected readonly string _getAllQuery;
		protected readonly string _getByIdQuery;

		delegate Task<G> SqlCommandDelegate<G, S>(SqlCommand sqlCommand, S entity);

		public MainMSSQLServerRepository(string connectionString, string tableName, 
			string insertQuery, string updateQuery, string getAllQuery, string getByIdQuery)
		{
			_connectionString = connectionString;
			_tableName = tableName;
			_deleteQuery = $@"DELETE FROM {_tableName} WHERE Id = @id";
			_insertQuery = insertQuery;
			_updateQuery = updateQuery;
			_getAllQuery = getAllQuery;
			_getByIdQuery = getByIdQuery;
		}

		private async Task<F> Connection<F, S>(S entity, SqlCommandDelegate<F, S> @delegate, string query)
		{
			using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
			{
				await sqlConnection.OpenAsync();

				using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
				{
					return await @delegate(sqlCommand, entity);
				}
			}
		}

		private async Task<bool> DeleteSqlCommand(SqlCommand sqlCommand, T entity)
		{
			sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
			await sqlCommand.ExecuteNonQueryAsync();

			return true;
		}

		protected abstract Task<bool> InsertSqlCommand(SqlCommand sqlCommand, T entity);

		protected abstract Task<bool> UpdateSqlCommand(SqlCommand sqlCommand, T entity);

		protected abstract Task<List<T>> GetAllSqlCommand(SqlCommand sqlCommand, T entity);

		protected abstract Task<T> GetByIdSqlCommand(SqlCommand sqlCommand, int id);

		public async Task<bool> DeleteAsync(T entity)
			=> await Connection<bool, T>(entity, DeleteSqlCommand, _deleteQuery);

		public async Task<bool> InsertAsync(T entity)
			=> await Connection<bool, T>(entity, InsertSqlCommand, _insertQuery);

		public async Task<bool> UpdateAsync(T entity)
			=> await Connection<bool, T>(entity, InsertSqlCommand, _updateQuery);

		public async Task<List<T>> GetAllAsync()
			=> await Connection<List<T>, T>(null, GetAllSqlCommand, _getAllQuery);

		public async Task<T> GetById(int id)
			=> await Connection<T, int>(id, GetByIdSqlCommand, _getByIdQuery);
	}
}
