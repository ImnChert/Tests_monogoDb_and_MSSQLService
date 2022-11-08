using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace Infrastructure.Data.MSSQLServerRepository.Connection
{
	internal abstract class MainMSSQLServerManyToManyRepository<T>
		: MainMSSQLServer, IManyToManyRepository<T> where T : EntityBase
	{
		protected string _getManyToManyQuery;
		protected string _setManyToManyQuery;

		public MainMSSQLServerManyToManyRepository(string connectionString, string getManyToManyQuery, string setManyToManyQuery) 
			: base(connectionString)
		{
			_getManyToManyQuery = getManyToManyQuery;
			_setManyToManyQuery = setManyToManyQuery;
		}

		protected abstract T GetReader(SqlDataReader sqlDataReader);

		private async Task<List<T>> GetSqlCommand(SqlCommand sqlCommand, int id)
		{
			sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;
			var list = new List<T>();

			using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
			{
				while (await sqlDataReader.ReadAsync())
				{
					list.Add(GetReader(sqlDataReader));
				}

				if (list.Count > 0)
					return list;
				else
					return null;
			}
		}

		private async Task<T> SetSqlCommand(SqlCommand sqlCommand, ManyToMany<T> value)
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

		public async Task<List<T>> GetManyToManyAsync(int id)
			=> await Connection<List<T>, int>(id, GetSqlCommand, _getManyToManyQuery);

		public async Task SetManyToMany(int id, List<T> values)
		=> await Connection<T, ManyToMany<T>>(new ManyToMany<T> { Id = id, ManyList = values}, SetSqlCommand, _setManyToManyQuery);
	}
}
