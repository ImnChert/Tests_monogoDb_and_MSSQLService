using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	internal class ScoreRepository : MainMSSQLServerRepository<Score>
	{
		private readonly Film _film;
		private readonly IRepository<RegisteredUser> _registerUserRepository;

		public ScoreRepository(string connectionString, Film film, IRepository<RegisteredUser> registerUserRepository)
			: base(connectionString,
				  "Scores",
				  $@"INSERT INTO Scores (Id,FilmId,RegisteredUsersId,Raiting) 
					VALUES(@Id,@FilmId,@RegisteredUsersId,@Raiting)",
				  $@"UPDATE Scores SET FilmId=@FilmId, RegisteredUsersId=@RegisteredUsersId, Raiting=@Raiting 
					WHERE Id=@Id",
				  $"SELECT Id,FilmId,RegisteredUsersId,Raiting FROM Scores",
				  $"SELECT Id,FilmId,RegisteredUsersId,Raiting FROM Scores WHERE Id=@id",
				  $"SELECT Id,FilmId,RegisteredUsersId,Raiting FROM Scores WHERE FilmsId=@FilmsId",
				  "@FilmsId")
		{
			_film = film;
			_registerUserRepository = registerUserRepository;
		}

		protected override Score GetReader(SqlDataReader sqlDataReader)
			=> new Score()
			{
				Id = (int)sqlDataReader["Id"],
				RegisteredUser = _registerUserRepository.GetById((int)sqlDataReader["RegisteredUsersId"]).Result,
				Raiting = (int)sqlDataReader["Raiting"]
			};

		protected override void InsertCommand(SqlCommand sqlCommand, Score entity)
		{
			sqlCommand.Parameters.Add("@RegisteredUsersId", SqlDbType.NVarChar).Value = entity.RegisteredUser.Id;
			sqlCommand.Parameters.Add("@FilmId", SqlDbType.NVarChar).Value = _film.Id;
			sqlCommand.Parameters.Add("@Raiting", SqlDbType.Decimal).Value = entity.Raiting;
		}
	}
}
