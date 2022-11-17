using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	public class ScheduleRepository : MainMSSQLServerRepository<Schedule>
	{
		IManyToManyRepository<Session> _sessionRepository;
		public ScheduleRepository(string connectionString, IManyToManyRepository<Session> sessionRepository)
			: base(connectionString,
				  "Schedules",
				  $"INSERT INTO Schedules (Id,HallId,ShowsDate) VALUES(@Id,@HallId,@ShowsDate)",
				  $"UPDATE Schedules SET HallId=@HallId, ShowsDate=@ShowsDate WHERE Id=@Id",
				  $"SELECT Id,HallId,ShowsDate FROM Schedules",
				  $"SELECT Id,HallId,ShowsDate FROM Schedules WHERE Id=@id")
		{
			_sessionRepository = sessionRepository;
		}

		protected override Schedule GetReader(SqlDataReader sqlDataReader)
			=> new Schedule()
			{
				Id = (int)sqlDataReader["Id"],
				Hall = new Hall()
				{
					Id = (int)sqlDataReader["HallId"]
				},
				Date = DateTime.Parse((string)sqlDataReader["ShowsDate"]),
				Sessions = _sessionRepository.GetManyToManyAsync((int)sqlDataReader["Id"]).Result
			};

		protected override void InsertCommand(SqlCommand sqlCommand, Schedule entity)
		{
			sqlCommand.Parameters.Add("@HallId", SqlDbType.NVarChar).Value = entity.Hall.Id;
			sqlCommand.Parameters.Add("@ShowsDate", SqlDbType.NVarChar).Value = entity.Date;
			_sessionRepository.SetManyToMany(entity.Id, entity.Sessions);
		}
	}
}
