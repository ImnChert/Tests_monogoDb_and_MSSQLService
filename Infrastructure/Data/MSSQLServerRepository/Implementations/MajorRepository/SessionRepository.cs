using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Interfaces;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MongoRepository.Implementations;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	internal class SessionRepository : MainMSSQLServerRepository<Session>
	{
		private IGetAllById<Ticket> _ticketGetAllById;
		private IRepository<Ticket> _ticketRepository;
		private IRepository<Film> _filmRepository;

		public SessionRepository(string connectionString, IRepository<Ticket> ticketRepository, IGetAllById<Ticket> ticketGetAllById, IRepository<Film> filmRepository)
			: base(connectionString,
				  "SessionsOfFilms",
				  $"INSERT INTO SessionsOfFilms (Id,FilmId,StartTime) VALUES(@Id,@FilmId,@StartTime)",
				  $"UPDATE SessionsOfFilms SET FilmId=@FilmId, StartTime=@StartTime WHERE Id=@Id",
				  $"SELECT Id, FilmId, StartTime FROM SessionsOfFilms",
				  $"SELECT Id, FilmId, StartTime FROM SessionsOfFilms WHERE Id=@id")
		{
			_ticketGetAllById = ticketGetAllById;
			_ticketRepository = ticketRepository;
			_filmRepository = filmRepository;
		}

		protected override Session GetReader(SqlDataReader sqlDataReader)
		  => new Session()
		  {
			  Id = (int)sqlDataReader["Id"],
			  Film = _filmRepository.GetById((int)sqlDataReader["FilmId"]).Result,
			  StartTime = DateTime.Parse(sqlDataReader["StartTime"].ToString()),
			  Tickets = _ticketGetAllById.GetAllById((int)sqlDataReader["Id"]).Result
		  };

		protected override void InsertCommand(SqlCommand sqlCommand, Session entity)
		{
			sqlCommand.Parameters.Add("@FilmId", SqlDbType.NVarChar).Value = entity.Film.Id;
			sqlCommand.Parameters.Add("@StartTime", SqlDbType.Decimal).Value = entity.StartTime;

			foreach (Ticket item in entity.Tickets)
			{
				_ticketRepository.InsertAsync(item);
			}
		}
	}
}
