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
		private static string _connectionString = "mongodb://localhost:27017";
		private static Schedule _testSchedule = new Schedule();

		[Fact, TestPriority(0)]
		public async Task InsertAndGet_AddingObjectsViaRepositoriesAndReadingDataUsingARepository_True()
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
				var categoryService = new CategoryRepositoryService(categoryRepositpry);

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
				var categoryAsync = await categoryService.GetAllAsync();
				Category categoryData = categoryAsync.Data[0];

				await employeeService.InsertAsync(employee);
				var employeeAsync = await employeeService.GetAllAsync();
				Employee employeeData = employeeAsync.Data[0];

				collectionUsers.ForEach(async user => await userService.InsertAsync(user));
				Thread.Sleep(100);
				var userAsync = await userService.GetAllAsync();
				List<RegisteredUser> usersData = userAsync.Data;

				FilmPreparation(film, usersData);
				await filmService.InsertAsync(film);
				var filmAsync = await filmService.GetAllAsync();
				Film filmData = filmAsync.Data[0];

				SchedulePreparation(schedule, collectionTickets, filmData, usersData, employeeData, categoryData);
				await scheduleService.InsertAsync(schedule);
				BaseResponse<Schedule> scheduleData = await scheduleService.GetById(schedule.Id);

				_testSchedule = scheduleData.Data;

				// Assert

				Assert.True(true);
			}
			catch (Exception ex)
			{
				Assert.True(false);
			}
		}

		[Fact, TestPriority(1)]
		public void AddTicket_TryToCreateTheWrongTicket_False()
		{
			Thread.Sleep(1000);

			try
			{
				// Arrange

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
				Schedule schedule = _testSchedule;
				var scheduleValidation = new ScheduleValidation(schedule);
				var scheduleFuntion = new ScheduleFunction(scheduleValidation, schedule);
				var scheduleFunctionService = new ScheduleFunctionService(scheduleFuntion);

				// Act
				var data = scheduleFunctionService.AddTicket(testUser, schedule.Sessions[0], testSeat);

				// Assert
				var result = data.Data;
				Assert.False(result);
			}
			catch (Exception ex)
			{
				Assert.False(true);
			}
		}

		[Fact, TestPriority(2)]
		public async Task Delete_DeleteAllDataInSource_True()
		{
			Thread.Sleep(2000);

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
				var categoryService = new CategoryRepositoryService(categoryRepositpry);

				var scheduleRepository = new ScheduleRepository(_connectionString);
				var scheduleService = new ScheduleRepositoryService(scheduleRepository);

				Schedule schedule = _testSchedule;

				// Act

				var collectionRegisteredUsersAsync = await userService.GetAllAsync();
				List<RegisteredUser> collectionRegisteredUsers = collectionRegisteredUsersAsync.Data;
				collectionRegisteredUsers.ForEach(async user => await userService.DeleteAsync(user));

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

				Assert.True(true);
			}
			catch (Exception)
			{
				Assert.True(false);
			}
		}

		private void FilmPreparation(Film film, List<RegisteredUser> usersData)
		{
			film.Reviews.ForEach(review => review.RegisteredUser = usersData[0]);
			film.Scores.ForEach(score => score.RegisteredUser = usersData[0]);
		}

		private void SchedulePreparation(Schedule schedule, List<Ticket> collectionTickets, Film filmData,
										List<RegisteredUser> usersData, Employee employeeData, Category categoryData)
		{
			foreach (Ticket ticket in collectionTickets)
			{
				ticket.RegisteredUser = usersData[0];
				ticket.Seat.Category = categoryData;
				ticket.Cashier = employeeData;
			}

			schedule.Sessions[0].Tickets = collectionTickets;
			schedule.Sessions[0].Film = filmData;
		}

		private List<RegisteredUser> GetTestUsers()
			=> new List<RegisteredUser>()
			{
				new RegisteredUser()
				{
					Username = "test1",
					Password = "test3",
					FirstName = "test3",
					MiddleName = "test3",
					LastName = "test3",
					DateOfBirthday = DateTime.Now,
					Phone = "+375444444441"
				},
				new RegisteredUser()
				{
					Username = "test2",
					Password = "test3",
					FirstName = "test3",
					MiddleName = "test3",
					LastName = "test3",
					DateOfBirthday = DateTime.Now,
					Phone = "+375444444442"
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