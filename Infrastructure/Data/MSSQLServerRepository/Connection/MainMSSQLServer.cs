using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Connection
{
	public abstract class MainMSSQLServer
	{
		protected readonly string _connectionString;

		protected delegate Task<G> SqlCommandDelegate<G, T>(SqlCommand sqlCommand, T entity);

		public MainMSSQLServer(string connectionString)
		{
			_connectionString = connectionString;
		}

		protected async Task<F> Connection<F, T>(T entity, SqlCommandDelegate<F, T> @delegate, string query)
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
