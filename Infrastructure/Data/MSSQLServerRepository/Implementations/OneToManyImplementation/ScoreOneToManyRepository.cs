using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using Infrastructure.Data.MSSQLServerRepository.Connection.Extensions;
using Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.ShortRepositoryImplementation
{
	public class ScoreOneToManyRepository : MSSQLRepository<OneToMany<Score>>
	{

		private readonly IRepository<RegisteredUser> _registerUserRepository;

		public ScoreOneToManyRepository(string connectionString, IRepository<RegisteredUser> registerUserRepository)
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
			_registerUserRepository = registerUserRepository;
		}

		public ScoreOneToManyRepository(string connectionString)
			: this(connectionString,
				  new UserRepository(connectionString))
		{
		}

		protected override OneToMany<Score> GetReader(SqlDataReader sqlDataReader)
			=> new OneToMany<Score>()
			{
				Value = new Score
				{
					Id = (int)sqlDataReader["Id"],
					RegisteredUser = _registerUserRepository.GetById((int)sqlDataReader["RegisteredUsersId"]).Result,
					Raiting = (int)sqlDataReader["Raiting"]
				}
			};

		protected override void InsertCommand(SqlCommand sqlCommand, OneToMany<Score> entity)
		{
			sqlCommand.Parameters.Add("@RegisteredUsersId", SqlDbType.NVarChar).Value = entity.Value.RegisteredUser.Id;
			sqlCommand.Parameters.Add("@FilmId", SqlDbType.NVarChar).Value = entity.AdditionId;
			sqlCommand.Parameters.Add("@Raiting", SqlDbType.Decimal).Value = entity.Value.Raiting;
		}
	}
}
