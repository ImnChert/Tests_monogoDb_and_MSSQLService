using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Core.Models.Roles.Staff.Positions;
using ApplicationCore.Services.Implementations.Repositories;
using Infrastructure.Data.MongoRepository.Implementations;
using Infrastructure.Data.MongoRepository.Implementations.RepositoryImplementetions;

namespace IntegratedTests
{
	[TestCaseOrderer("IntegratedTests.Configuration", "IntegratedTests")]
	public class ScheduleFunctionIntegratedTest
	{
		private string _connectionString = "mongodb://localhost:27017";
		//@"Data Source=DESKTOP-CTBUCT0\SQLEXPRESS;Initial Catalog=CP;Integrated Security=True";

		private List<RegisteredUser> GetTestUsers()
			=> new List<RegisteredUser>()
			{
				new RegisteredUser()
				{
					Username = "test3",
					Password = "test3",
					FirstName = "test3",
					MiddleName = "test3",
					LastName = "test3",
					DateOfBirthday = DateTime.Now,
					Phone = "+375444444444"
				},
				new RegisteredUser()
				{
					Username = "test3",
					Password = "test3",
					FirstName = "test3",
					MiddleName = "test3",
					LastName = "test3",
					DateOfBirthday = DateTime.Now,
					Phone = "+375444444444"
				},
				new RegisteredUser()
				{
					Username = "test3",
					Password = "test3",
					FirstName = "test3",
					MiddleName = "test3",
					LastName = "test3",
					DateOfBirthday = DateTime.Now,
					Phone = "+375444444444"
				}
			};

		private Category GetTestCategory()
			=> new Category()
			{
				Name = "test",
				Price = 100
			};

		private Film GetTestFilm()
			=> new Film()
			{
				Name = "test",
				Duration = 120,
				FilmCrew = new List<Person>()
				{
					new Person()
					{
						FirstName= "testFirstName",
						MiddleName = "testMiddleName",
						LastName = "testLastName",
						Post = "Режисер"
					}
				},
				Reviews = new List<Review>()
				{
					new Review()
					{
						RegisteredUser = GetTestUsers()[0],
						Description = "test"
					}
				},
				Scores = new List<Score>()
				{
					new Score()
					{
						RegisteredUser = GetTestUsers()[0],
						Raiting = 1
					}
				},
				Description = "test",
				Year = 2001,
				LicensExpirationDate = DateTime.Now,
				Distributor = new Distributor()
				{
					NameCompany = "test"
				},
				BasePrice = 100
			};

		private Employee GetTestEmployee()
			=> new Employee()
			{
				Username = "test3",
				Password = "test3",
				FirstName = "test3",
				MiddleName = "test3",
				LastName = "test3",
				Positions = new List<Position>()
				{
					new Cashier()
				}
			};

		private List<Ticket> GetTestTicket()
			=> new List<Ticket>()
					{
						new Ticket()
						{
							Seat = new Seat()
							{
								NumberRow = 1,
								NumberColumn = 1,
								Category =  null
							},
							RegisteredUser = null,
							Cashier = null
						},
						new Ticket()
						{
							Seat = new Seat()
							{
								NumberRow = 2,
								NumberColumn = 1,
								Category = null
							},
							RegisteredUser = null,
							Cashier = null
						}
					};

		private List<Session> GetTestListSession()
			=> new List<Session>()
			{
				new Session()
				{
					Film = null,
					StartTime= DateTime.Now,
					Tickets =  null
				}
			};

		private Schedule GetTestSchedule()
			=> new Schedule
			{
				Hall = new Hall { Id = 1 },
				Date = new DateTime(),
				Sessions = GetTestListSession()
			};

		[Fact, TestPriority(0)]
		public async Task CreateTestData()
		{
			try
			{
				// Arrange
				var userRepository = new UserRepository(_connectionString);
				var userService = new UserService(userRepository);

				var employeeRepository = new EmployeeRepository(_connectionString);
				var employeeService = new EmployeeService(employeeRepository);

				var filmRepositpry = new FilmRepository(_connectionString);
				var filmService = new FilmService(filmRepositpry);

				var categoryRepositpry = new CategoryRepository(_connectionString);
				var categoryService = new CategoryService(categoryRepositpry);

				var scheduleRepository = new ScheduleRepository(_connectionString);
				var scheduleService = new ScheduleService(scheduleRepository);

				List<RegisteredUser> collectionUsers = GetTestUsers();
				List<Ticket> collectionTickets = GetTestTicket();
				Category category = GetTestCategory();
				Employee employee = GetTestEmployee();
				Schedule schedule = GetTestSchedule();
				Film film = GetTestFilm();

				// Act

				await categoryService.InsertAsync(category);
				Category categoryData = categoryService.GetAllAsync().Result.Data[0];

				await employeeService.InsertAsync(employee);
				Employee employeeData = employeeService.GetAllAsync().Result.Data[0];

				// TODO: тестил сотрудника траблы с должностью

				collectionUsers.ForEach(async user => await userService.InsertAsync(user));
				List<RegisteredUser> usersData = userService.GetAllAsync().Result.Data;

				await filmService.InsertAsync(film);
				Film filmData = filmService.GetAllAsync().Result.Data[0];

				foreach (Ticket ticket in collectionTickets)
				{
					ticket.RegisteredUser = usersData[0];
					ticket.Seat.Category = category;
					ticket.Cashier = employee;
				}

				schedule.Sessions[0].Tickets = collectionTickets;
				schedule.Sessions[0].Film = film;

				await scheduleService.InsertAsync(schedule);
				var scheduleData = await scheduleService.GetById(schedule.Id);

				Assert.True(true);
			}
			catch (Exception)
			{

				Assert.True(false);
			}
		}

		//[Fact, TestPriority(1)]
		//public async Task TestCalculation()
		//{
		//	// Arrange
		//	ITripDetailService tripDetailService = new TripDetailService(new TripDetailRepositoryMS(_stringConnection),
		//		new TripRepositoryMS(_stringConnection), new UserRepositoryMS(_stringConnection));
		//	ITripTreatmentService tripTreatmentService = new TripTreatmentService();

		//	// Act
		//	var data = await tripDetailService.GetDetailsByTripId(_foundTrip.Id);

		//	var availablePlacesResponse = tripTreatmentService.GetAvailablePlaces(data.Data, _foundTrip.Capacity);
		//	var aavailablePlaces = availablePlacesResponse.Data;
		//	// Assert

		//	Assert.Equal(_foundTrip.Capacity - data.Data.Count(), aavailablePlaces.Count());
		//}

		[Fact, TestPriority(1)]
		public async Task TestDeleteAllData()
		{
			try
			{
				// Arrange
				var userRepository = new UserRepository(_connectionString);
				var userService = new UserService(userRepository);

				//var ticketRepositpry = new TicketRepository(_connectionString);
				var filmRepositpry = new FilmRepository(_connectionString);


				var scheduleRepository = new ScheduleRepository(_connectionString);
				var scheduleService = new ScheduleService(scheduleRepository);

				// Act
				//var details = await tripDetailService.GetDetailsByTripId(_foundTrip.Id);
				//await DeleteTripDetails(details.Data);
				//await tripService.DeleteById(_foundTrip.Id);
				//await planeService.DeleteById(_foundTrip.PlaneId);

				//var country = await cityService.GetById(_foundTrip.StartCityId);

				//await cityService.DeleteById(_foundTrip.StartCityId);
				//await cityService.DeleteById(_foundTrip.EndCityId);
				//await countryService.DeleteById(country.Data.CountryId);
				var s = userService.GetById(1).Result;
				//await userService.DeleteAsync(GetTestUser());
				//await scheduleService.DeleteAsync(GetTestSchedule());

				Assert.True(true);
			}
			catch (Exception)
			{

				Assert.True(false);
			}
		}
	}
}
