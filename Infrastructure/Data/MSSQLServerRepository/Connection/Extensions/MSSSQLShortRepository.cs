using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace Infrastructure.Data.MSSQLServerRepository.Connection.Extensions
{
	public abstract class MainMSSSQLServerUpdateInsert<T>
		: MainMSSQLServer, IShortRepository<T> where T : EntityBase
	{
		protected readonly string _tableName;
		protected readonly string _deleteQuery;
		protected readonly string _insertQuery;
		protected readonly string _updateQuery;

		public MainMSSSQLServerUpdateInsert(string connectionString) : base(connectionString)
		{
		}

		public async Task<bool> DeleteAsync(T entity)
			=> await Connection(entity, DeleteSqlCommand, _deleteQuery);

		private async Task<bool> DeleteSqlCommand(SqlCommand sqlCommand, T entity)
		{
			sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
			await sqlCommand.ExecuteNonQueryAsync();

			return true;
		}

		public async Task<bool> InsertAsync(T entity)
			=> await Connection(entity, InsertSqlCommand, _insertQuery);

		protected abstract Task<bool> InsertSqlCommand(SqlCommand sqlCommand, T entity);

		public async Task<bool> UpdateAsync(T entity)
			=> await Connection(entity, UpdateSqlCommand, _updateQuery);

		protected abstract Task<bool> UpdateSqlCommand(SqlCommand sqlCommand, T entity);
	}
}
