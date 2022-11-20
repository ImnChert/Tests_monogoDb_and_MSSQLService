using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace Infrastructure.Data.MSSQLServerRepository.Connection.Extensions
{
	public abstract class MSSQLManyToManyRepository<T>
		: MainMSSQLServer, IManyToManyRepository<T> where T : EntityBase
	{
		protected string _getManyToManyQuery;
		protected string _setManyToManyQuery;

		public MSSQLManyToManyRepository(string connectionString, string getManyToManyQuery, string setManyToManyQuery)
			: base(connectionString)
		{
			_getManyToManyQuery = getManyToManyQuery;
			_setManyToManyQuery = setManyToManyQuery;
		}

		public MSSQLManyToManyRepository(string connectionString, string getManyToManyQuery,
			string setManyToManyQuery, string insertQuery)
			: this(connectionString, getManyToManyQuery, setManyToManyQuery)
		{
			//_insertQuery = insertQuery;
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
			=> await Connection(id, GetSqlCommand, _getManyToManyQuery);

		public async Task<bool> SetManyToMany(int id, List<T> values)
		=> await Connection(new ManyToMany<T> { Id = id, ManyList = values }, SetSqlCommand, _setManyToManyQuery);
	}
}
