using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Interfaces;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	internal class TicketRepository : MainMSSQLServerRepository<Ticket>, IGetAllById<Ticket>
	{
		private Session _session;
		private IRepository<Employee> _employeeRepository;
		private IRepository<RegisteredUser> _registeredUserRepository;
		private IRepository<Seat> _seatRepository;
		private string _getAllByIdQuery;

		public TicketRepository(string connectionString, Session session, IRepository<Employee> employeeRepository, IRepository<RegisteredUser> registeredUserRepository, IRepository<Seat> seatRepository)
			: base(connectionString,
				  "Tickets",
				  $"INSERT INTO Tickets (Id,SeatId,SessionId,RegisteredUserId,EmployeeId) VALUES(@Id,@SeatId,@SessionId,@RegisteredUserId,@EmployeeId)",
				  $@"UPDATE Tickets SET SeatId=@SeatId, SessionId=@SessionId, RegisteredUserId=@RegisteredUserId, 
					EmployeeId=@EmployeeId WHERE Id=@Id",
				  $"SELECT Id,SeatId,SessionId,RegisteredUserId,EmployeeId FROM Tickets",
				  $@"SELECT Id,SeatId,SessionId,RegisteredUserId,EmployeeId 
					FROM Tickets 
					WHERE Id=@Id")
		{
			_session = session;
			_employeeRepository = employeeRepository;
			_registeredUserRepository = registeredUserRepository;
			_seatRepository = seatRepository;
			_getAllByIdQuery = $"SELECT Id,SeatId,SessionId,RegisteredUserId,EmployeeId FROM Tickets WHERE SessionId=@SessionId";
		}

		protected override Ticket GetReader(SqlDataReader sqlDataReader)
		 => new Ticket()
		 {
			 Id = (int)sqlDataReader["Id"],
			 Seat = _seatRepository.GetById((int)sqlDataReader["SeatId"]).Result,
			 RegisteredUser = _registeredUserRepository.GetById((int)sqlDataReader["RegisteredUserId"]).Result,
			 Cashier = _employeeRepository.GetById((int)sqlDataReader["EmployeeId"]).Result
		 };

		protected override void InsertCommand(SqlCommand sqlCommand, Ticket entity)
		{
			sqlCommand.Parameters.Add("@Id", SqlDbType.NVarChar).Value = entity.Id;
			sqlCommand.Parameters.Add("@SeatId", SqlDbType.Decimal).Value = entity.Seat.Id;
			sqlCommand.Parameters.Add("@SessionId", SqlDbType.Decimal).Value = _session.Id;
			sqlCommand.Parameters.Add("@RegisteredUserId", SqlDbType.Decimal).Value = entity.RegisteredUser.Id;
			sqlCommand.Parameters.Add("@EmployeeId", SqlDbType.Decimal).Value = entity.Cashier.Id;
		}

		public async Task<List<Ticket>> GetAllById(int id)
			=> await Connection<List<Ticket>, int>(id, GetAllByIdSqlCommand, _getAllByIdQuery);

		private async Task<List<Ticket>> GetAllByIdSqlCommand(SqlCommand sqlCommand, int id)
		{
			var username = new SqlParameter
			{
				ParameterName = "@SessionId",
				Value = id,
				SqlDbType = SqlDbType.Int,
				Direction = ParameterDirection.Input
			};

			sqlCommand.Parameters.Add(username);

			var tickets = new List<Ticket>();

			using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
			{
				while (await sqlDataReader.ReadAsync())
				{
					tickets.Add(GetReader(sqlDataReader));
				}

				if (tickets.Count > 0)
					return tickets;
				else
					return null;
			}
		}
	}
}
