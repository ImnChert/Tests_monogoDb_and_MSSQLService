using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Core.Models.Roles.Staff.Positions;
using ApplicationCore.Services.Implementations.FunctionalEntities.Implementations;
using ApplicationCore.Services.Implementations.FunctionalEntities.Services;
using ApplicationCore.Services.Implementations.Repositories;
using ApplicationCore.Services.Implementations.Validations;
using Infrastructure.Data.MongoRepository.Implementations.RepositoryImplementetions;

namespace IntegratedTests
{
	[TestCaseOrderer("IntegratedTests.Configuration", "IntegratedTests")]
	public class ScheduleFunctionIntegratedTest
	{
		private string _connectionString = "mongodb://localhost:27017";

		[Fact, TestPriority(0)]
		public void CreateTestData()
		{
			Assert.True(CreateData().Result);
		}

		[Fact, TestPriority(1)]
		public void TestCalculation()
		{
			Assert.True(Calculation().Result);
		}

		[Fact, TestPriority(2)]
		public void TestDeleteAllData()
		{
			Assert.True(DeleteAllData().Result);
		}

		//[Fact, TestPriority(0)]
		public async Task<bool> CreateData()
		{
			try
			{
				// Arrange

				var userRepository = new UserRepository(_connectionString);
				var userService = new UserRepositoryService(userRepository);

				var employeeRepository = new EmployeeRepository(_connectionString);
				var employeeService = new EmployeeRepositoryService(employeeRepository);

				var filmRepositpry = new FilmRepository(_connectionString);
				var filmService = new FilmRepositoryService(filmRepositpry);

				var categoryRepositpry = new CategoryRepository(_connectionString);
				var categoryService = new CategoryService(categoryRepositpry);

				var scheduleRepository = new ScheduleRepository(_connectionString);
				var scheduleService = new ScheduleRepositoryService(scheduleRepository);

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

				collectionUsers.ForEach(async user => await userService.InsertAsync(user));
				List<RegisteredUser> usersData = userService.GetAllAsync().Result.Data;

				film.Reviews.ForEach(review => review.RegisteredUser = usersData[0]);
				film.Scores.ForEach(score => score.RegisteredUser = usersData[0]);
				await filmService.InsertAsync(film);
				Film filmData = filmService.GetAllAsync().Result.Data[0];

				foreach (Ticket ticket in collectionTickets)
				{
					ticket.RegisteredUser = usersData[0];
					ticket.Seat.Category = categoryData;
					ticket.Cashier = employeeData;
				}

				schedule.Sessions[0].Tickets = collectionTickets;
				schedule.Sessions[0].Film = filmData;

				await scheduleService.InsertAsync(schedule);
				BaseResponse<Schedule> scheduleData = await scheduleService.GetById(schedule.Id);

				//Assert.True(true);
				return true;
			}
			catch (Exception)
			{
				return false;
				//Assert.True(false);
			}
		}

		//[Fact, TestPriority(1)]
		public async Task<bool> Calculation()
		{
			try
			{
				// Arrange
				var scheduleRepository = new ScheduleRepository(_connectionString);
				var scheduleService = new ScheduleRepositoryService(scheduleRepository);

				var testUser = new RegisteredUser()
				{
					Username = "test3",
					Password = "test3",
					FirstName = "test3",
					MiddleName = "test3",
					LastName = "test3",
					DateOfBirthday = DateTime.Now,
					Phone = "+375444444444"
				};

				var testSeat = new Seat()
				{
					NumberRow = 1,
					NumberColumn = 1,
					Category = null
				};

				// Act
				var data = scheduleService.GetAllAsync().Result.Data[0];
				Schedule schedule = data;
				var scheduleValidation = new ScheduleValidation(schedule);
				var scheduleFuntion = new ScheduleFunction(scheduleValidation, schedule);
				var scheduleFunctionService = new ScheduleFunctionService(scheduleFuntion);


				// Assert
				var assert = scheduleFunctionService.AddTicket(testUser, schedule.Sessions[0], testSeat).Data;
				//Assert.False(assert);
				return !assert;
			}
			catch
			{
				//Assert.False(true);//
				return false;
			}
		}

		//[Fact, TestPriority(2)]
		public async Task<bool> DeleteAllData()
		{
			try
			{
				// Arrange
				var userRepository = new UserRepository(_connectionString);
				var userService = new UserRepositoryService(userRepository);

				var employeeRepository = new EmployeeRepository(_connectionString);
				var employeeService = new EmployeeRepositoryService(employeeRepository);

				var filmRepositpry = new FilmRepository(_connectionString);
				var filmService = new FilmRepositoryService(filmRepositpry);

				var categoryRepositpry = new CategoryRepository(_connectionString);
				var categoryService = new CategoryService(categoryRepositpry);

				var scheduleRepository = new ScheduleRepository(_connectionString);
				var scheduleService = new ScheduleRepositoryService(scheduleRepository);

				// Act

				List<RegisteredUser> collectionRegisteredUsers = userService.GetAllAsync().Result.Data;
				collectionRegisteredUsers.ForEach(async user => await userService.DeleteAsync(user));

				Schedule schedule = scheduleService.GetAllAsync().Result.Data[0];

				schedule.Sessions.ForEach(async session =>
				{
					await filmService.DeleteAsync(session.Film);

					session.Tickets.ForEach(async ticket =>
					{
						await employeeService.DeleteAsync(ticket.Cashier);
						await categoryService.DeleteAsync(ticket.Seat.Category);
					});
				});

				await scheduleRepository.DeleteAsync(schedule);

				//Assert.True(true);
				return true;
			}
			catch (Exception)
			{

				//Assert.True(false);
				return false;
			}
		}

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
	}
}