using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Interfaces;
using System.Data.SqlClient;
using System.Data;
using SharpCompress.Common;

namespace Infrastructure.Data.MSSQLServerRepository.Connection
{
	public abstract class MainMSSQLServerManyToManyRepository<T>
		: MainMSSQLServer, IManyToManyRepository<T> where T : EntityBase
	{
		protected string _getManyToManyQuery;
		protected string _setManyToManyQuery;
		protected string _insertQuery = null;

		public MainMSSQLServerManyToManyRepository(string connectionString, string getManyToManyQuery, string setManyToManyQuery)
			: base(connectionString)
		{
			_getManyToManyQuery = getManyToManyQuery;
			_setManyToManyQuery = setManyToManyQuery;
		}

		public MainMSSQLServerManyToManyRepository(string connectionString, string getManyToManyQuery,
			string setManyToManyQuery, string insertQuery)
			: this(connectionString, getManyToManyQuery, setManyToManyQuery)
		{
			_insertQuery = insertQuery;
		}

		protected abstract T GetCommand(SqlDataReader sqlDataReader);

		private async Task<List<T>> GetSqlCommand(SqlCommand sqlCommand, int id)
		{
			sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;
			var list = new List<T>();

			using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
			{
				while (await sqlDataReader.ReadAsync())
				{
					list.Add(GetCommand(sqlDataReader));
				}

				if (list.Count > 0)
					return list;
				else
					return null;
			}
		}

		protected abstract Task SetCommand(SqlCommand sqlCommand, ManyToMany<T> value);

		private async Task<bool> SetSqlCommand(SqlCommand sqlCommand, ManyToMany<T> value)
		{
			await SetCommand(sqlCommand, value);

			await sqlCommand.ExecuteNonQueryAsync();

			return true;
		}

		public async Task<List<T>> GetManyToManyAsync(int id)
			=> await Connection<List<T>, int>(id, GetSqlCommand, _getManyToManyQuery);

		public async Task<bool> SetManyToMany(int id, List<T> values)
		=> await Connection<bool, ManyToMany<T>>(new ManyToMany<T> { Id = id, ManyList = values }, SetSqlCommand, _setManyToManyQuery);

		public async Task<bool> InsertAsync(T entity)
		{
			if (_insertQuery == null)
				return false;

			return await Connection<bool, T>(entity, InsertSqlCommand, _insertQuery);
		}

		private async Task<bool> InsertSqlCommand(SqlCommand sqlCommand, T entity)
		{
			InsertCommand(sqlCommand, entity);

			await sqlCommand.ExecuteNonQueryAsync();

			return true;
		}

		protected abstract void InsertCommand(SqlCommand sqlCommand, T entity);
	}
}
