using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	internal class SessionRepository : MainMSSQLServerRepository<Session>
	{
		private IRepository<Ticket> _ticketRepository;

		public SessionRepository(string connectionString, IRepository<Ticket> ticketRepository)
			: base(connectionString,
				  "SessionsOfFilms",
				  insertQuery,
				  updateQuery,
				  getAllQuery,
				  getByIdQuery)
		{
			_ticketRepository = ticketRepository;
		}

		protected override Session GetReader(SqlDataReader sqlDataReader)
		{
			throw new NotImplementedException();
		}

		protected override void InsertCommand(SqlCommand sqlCommand, Session entity)
		{
			throw new NotImplementedException();
		}
	}
}
