using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Interfaces.Interfaces;
using System.Data.SqlClient;
using System.Data;
using ApplicationCore.Domain.Interfaces;
using ApplicationCore.Domain.Core.Models.Cinema;

namespace Infrastructure.Data.MSSQLServerRepository.Connection
{
	public abstract class MainMSSQLServerRepository<T>
		 : MainMSSQLServer, IRepository<T>, IGetAllById<T> where T : EntityBase
	{
		protected readonly string _tableName;
		protected readonly string _deleteQuery;
		protected readonly string _insertQuery;
		protected readonly string _updateQuery;
		protected readonly string _getAllQuery;
		protected readonly string _getByIdQuery;
		protected readonly string _getAllByIdQuery = null;
		protected readonly string _parameterName = null;

		public MainMSSQLServerRepository(string connectionString, string tableName,
			string insertQuery, string updateQuery, string getAllQuery, string getByIdQuery)
			: base(connectionString)
		{
			_tableName = tableName;
			_deleteQuery = $@"DELETE FROM {_tableName} WHERE Id = @id";
			_insertQuery = insertQuery;
			_updateQuery = updateQuery;
			_getAllQuery = getAllQuery;
			_getByIdQuery = getByIdQuery;
		}

		public MainMSSQLServerRepository(string connectionString, string tableName,
			string insertQuery, string updateQuery, string getAllQuery, string getByIdQuery,
			string getAllByIdQuery, string parameterName)
			: this(connectionString, tableName, insertQuery, updateQuery, getAllQuery, getByIdQuery)
		{
			_getAllByIdQuery = getAllByIdQuery;
			_parameterName = parameterName;
		}

		private async Task<bool> DeleteSqlCommand(SqlCommand sqlCommand, T entity)
		{
			sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
			await sqlCommand.ExecuteNonQueryAsync();

			return true;
		}

		private async Task<bool> InsertSqlCommand(SqlCommand sqlCommand, T entity)
		{
			InsertCommand(sqlCommand, entity);

			await sqlCommand.ExecuteNonQueryAsync();

			return true;
		}

		private async Task<bool> UpdateSqlCommand(SqlCommand sqlCommand, T entity)
		{
			sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = entity.Id;
			InsertCommand(sqlCommand, entity);

			await sqlCommand.ExecuteNonQueryAsync();

			return true;
		}

		protected abstract void InsertCommand(SqlCommand sqlCommand, T entity);

		protected abstract T GetReader(SqlDataReader sqlDataReader);

		protected async Task<List<T>> GetAllSqlCommand(SqlCommand sqlCommand, T entity)
		{
			var collection = new List<T>();

			using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
			{
				while (await sqlDataReader.ReadAsync())
				{
					collection.Add(GetReader(sqlDataReader));
				}

				if (collection.Count > 0)
					return collection;
				else
					return null;
			}
		}

		private async Task<T> GetByIdSqlCommand(SqlCommand sqlCommand, int id)
		{
			var username = new SqlParameter
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
					return GetReader(sqlDataReader);
				else
					return null;
			}
		}

		public async Task<bool> DeleteAsync(T entity)
			=> await Connection<bool, T>(entity, DeleteSqlCommand, _deleteQuery);

		public async Task<bool> InsertAsync(T entity)
			=> await Connection<bool, T>(entity, InsertSqlCommand, _insertQuery);

		public async Task<bool> UpdateAsync(T entity)
			=> await Connection<bool, T>(entity, UpdateSqlCommand, _updateQuery);

		public async Task<List<T>> GetAllAsync()
			=> await Connection<List<T>, T>(null, GetAllSqlCommand, _getAllQuery);

		public async Task<T> GetById(int id)
			=> await Connection<T, int>(id, GetByIdSqlCommand, _getByIdQuery);

		public async Task<List<T>> GetAllByIdOneToMany(int id)
		{
			if (_getAllByIdQuery == null)
				return null;

			return await Connection<List<T>, int>(id, GetAllByIdSqlCommand, _getAllByIdQuery);
		}

		private async Task<List<T>> GetAllByIdSqlCommand(SqlCommand sqlCommand, int id)
		{
			if (_parameterName == null)
				return null;

			var username = new SqlParameter
			{
				ParameterName = _parameterName,
				Value = id,
				SqlDbType = SqlDbType.Int,
				Direction = ParameterDirection.Input
			};

			sqlCommand.Parameters.Add(username);

			var collection = new List<T>();

			using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
			{
				while (await sqlDataReader.ReadAsync())
				{
					collection.Add(GetReader(sqlDataReader));
				}

				if (collection.Count > 0)
					return collection;
				else
					return null;
			}
		}
	}
}
