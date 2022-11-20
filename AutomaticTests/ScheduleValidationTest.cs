using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Core.Models.Roles.Staff.Positions;
using ApplicationCore.Services.Implementations.Validations;

namespace AutomaticTests
{
	public class ScheduleValidationTest
	{
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
								Category =  GetTestCategory()
							},
							RegisteredUser = GetTestUsers()[1],
							Cashier = null
						},
						new Ticket()
						{
							Seat = new Seat()
							{
								NumberRow = 2,
								NumberColumn = 1,
								Category = GetTestCategory()
							},
							RegisteredUser = GetTestUsers()[0],
							Cashier = GetTestEmployee()
						}
					};

		private List<Session> GetTestListSession()
			=> new List<Session>()
			{
				new Session()
				{
					Film = GetTestFilm(),
					StartTime= DateTime.Now,
					Tickets =  GetTestTicket()
				}
			};

		private Schedule GetTestSchedule()
			=> new Schedule
			{
				Hall = new Hall { Id = 1 },
				Date = new DateTime(),
				Sessions = GetTestListSession()
			};

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

		[Fact]
		public void IsValidSession_ScheduleContainsSession_IsTrue()
		{
			//Arange
			Schedule schedule = GetTestSchedule();
			var scheduleValidation = new ScheduleValidation(schedule);
			var testSession = new Session()
			{
				Film = GetTestFilm(),
				StartTime = DateTime.Now,
				Tickets = GetTestTicket()
			};

			// Act
			var result = scheduleValidation.ContainSession(testSession);

			//Assert
			Assert.True(result);
		}

		[Fact]
		public void IsValidSession_ScheduleContainsSession_IsFalse()
		{
			//Arange
			Schedule schedule = GetTestSchedule();
			var scheduleValidation = new ScheduleValidation(schedule);
			var testSession = new Session()
			{
				Film = null,
				StartTime = DateTime.Now,
				Tickets = GetTestTicket()
			};

			// Act
			var result = scheduleValidation.ContainSession(testSession);

			//Assert
			Assert.False(result);
		}

		[Fact]
		public void IsValidSeat_ThereIsTheSeatInAnyTicket_IsTrue()
		{
			//Arange
			Schedule schedule = GetTestSchedule();
			var scheduleValidation = new ScheduleValidation(schedule);
			var testSession = new Seat()
			{
				NumberRow = 1,
				NumberColumn = 1,
				Category = GetTestCategory()
			};

			// Act
			var result = scheduleValidation.ContainSeat(testSession);

			//Assert
			Assert.True(result);
		}

		[Fact]
		public void IsValidSeat_ThereIsTheSeatInAnyTicket_IsFalse()
		{
			//Arange
			Schedule schedule = GetTestSchedule();
			var scheduleValidation = new ScheduleValidation(schedule);
			var testSession = new Seat()
			{
				NumberRow = 2,
				NumberColumn = 2,
				Category = GetTestCategory()
			};

			// Act
			var result = scheduleValidation.ContainSeat(testSession);

			//Assert
			Assert.False(result);
		}
	}
}
