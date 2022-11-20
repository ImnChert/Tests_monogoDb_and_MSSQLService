using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Interfaces;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using Infrastructure.Data.MSSQLServerRepository.Connection.Extensions;
using Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.LowerRepository
{
	public class SessionRepository : MSSQLManyToManyRepository<Session>
	{
		private IGetAllById<Ticket> _ticketGetAllById;
		private IRepository<Ticket> _ticketRepository;
		private IRepository<Film> _filmRepository;

		public SessionRepository(string connectionString, IRepository<Ticket> ticketRepository,
			IGetAllById<Ticket> ticketGetAllById, IRepository<Film> filmRepository)
			: base(connectionString,
				 $@"SELECT SessionsOfFilms.Id, SessionsOfFilms.FilmId, SessionsOfFilms.StartTime
					FROM Schedules
					JOIN ScheduleSessions ON SchedulesId = Schedules.Id
					JOIN SessionsOfFilms ON SessionsOfFilmsId = SessionsOfFilms.Id
					WHERE Schedules.Id=@Id",
				  $@"INSERT INTO ScheduleSessions (SchedulesId,SessionsOfFilmsId) VALUES(@SchedulesId,@SessionsOfFilmsId)",
				  $"INSERT INTO SessionsOfFilms (Id,FilmId,StartTime) VALUES(@Id,@FilmId,@StartTime)")
		{
			_ticketGetAllById = ticketGetAllById;
			_ticketRepository = ticketRepository;
			_filmRepository = filmRepository;
		}

		public SessionRepository(string connectionString)
			: this(connectionString,
				  new TicketRepository(connectionString),
				  new TicketRepository(connectionString),
				  new FilmRepository(connectionString))
		{
		}

		protected override Session GetCommand(SqlDataReader sqlDataReader)
			=> new Session()
			{
				Id = (int)sqlDataReader["Id"],
				Film = _filmRepository.GetById((int)sqlDataReader["FilmId"]).Result,
				StartTime = DateTime.Parse(sqlDataReader["StartTime"].ToString()),
				Tickets = _ticketGetAllById.GetAllByIdOneToMany((int)sqlDataReader["Id"]).Result
			};

		//protected override void InsertCommand(SqlCommand sqlCommand, Session entity)
		//{
		//	sqlCommand.Parameters.Add("@FilmId", SqlDbType.NVarChar).Value = entity.Film.Id;
		//	sqlCommand.Parameters.Add("@StartTime", SqlDbType.Decimal).Value = entity.StartTime;

		//	foreach (Ticket item in entity.Tickets)
		//	{
		//		_ticketRepository.InsertAsync(item);
		//	}
		//}

		protected override async Task SetCommand(SqlCommand sqlCommand, ManyToMany<Session> value)
			=> value.ManyList.ForEach(item =>
			{
				sqlCommand.Parameters.Add("@SchedulesId", SqlDbType.NVarChar).Value = value.Id;
				sqlCommand.Parameters.Add("@SessionsOfFilmsId", SqlDbType.NVarChar).Value = item.Id;
			});
	}
}
