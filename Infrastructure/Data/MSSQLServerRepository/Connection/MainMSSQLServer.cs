using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Connection
{
	public class MainMSSQLServer
	{
		protected readonly string _connectionString;

		protected delegate Task<G> SqlCommandDelegate<G, S>(SqlCommand sqlCommand, S entity);

		public MainMSSQLServer(string connectionString)
		{
			_connectionString = connectionString;
		}

		protected async Task<F> Connection<F, S>(S entity, SqlCommandDelegate<F, S> @delegate, string query)
		{
			using (var sqlConnection = new SqlConnection(_connectionString))
			{
				await sqlConnection.OpenAsync();

				using (var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					return await @delegate(sqlCommand, entity);
				}
			}
		}
	}
}
