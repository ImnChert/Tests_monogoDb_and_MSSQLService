using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Interfaces;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MongoRepository.Implementations.RepositoryImplementetions;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace Infrastructure.Data.MongoRepository.Implementations.GetAllByIdImplementations
{
	public class TicketGetAllById : IGetAllById<Ticket>
	{
		private readonly IMongoCollection<BsonDocument> _mongoCollection;
		private IRepository<Category> _categoryRepository;
		private IRepository<RegisteredUser> _userRepository;
		private IRepository<Employee> _employeeRepository;

		public TicketGetAllById(string connectionString, IMongoCollection<BsonDocument> mongoCollection)
		{
			_mongoCollection = mongoCollection;
			_categoryRepository = new CategoryRepository(connectionString);
			_userRepository = new UserRepository(connectionString);
			_employeeRepository = new EmployeeRepository(connectionString);
		}

		public async Task<List<Ticket>> GetAllByIdOneToMany(int id)
		{
			var pipeline = new BsonDocument()
			{
				{ "$unwind", "$sessions"}
			};
			//new BsonDocument { { "$unwind", "$sessions.tickets" } }
			var pipeline2 = new BsonDocument
			{
				{"$match", new BsonDocument{
					{"_id", id }, { "sessions._id", 0}
				}}
			};

			// TODO: только сессия с id 0 будет выбираться

			var pipeline3 = new BsonDocument
			{
				{
					"$project", new BsonDocument
					{
						{ "_id", "$sessions.tickets._id" },
						{"usernameRegisteredUser", "$sessions.tickets.usernameRegisteredUser" },
						{"registeredUser_id","$sessions.tickets.registeredUser_id"},
						{"usernameEmployee", "$sessions.tickets.usernameEmployee" },
						{"employee_id","$sessions.tickets.employee_id" },
						{"numberRow", "$sessions.tickets.seat.numberRow"},
						{"numberColumn", "$sessions.tickets.seat.numberColumn"},
						{"categoryName", "$sessions.tickets.seat.categoryName"},
						{"category_id", "$sessions.tickets.seat.category_id"}
					}
				}
			};

			BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
			List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

			List<Ticket> tickets = new();

			foreach (BsonDocument item in results)
			{
				for (int i = 0; i < item.GetValue("employee_id").AsBsonArray.Count; i++)
				{
					if (item.GetValue("employee_id").AsBsonArray[i].ToInt32() == 0)
					{
						tickets.Add(new Ticket()
						{
							Id = item.GetValue("_id")[i].ToInt32(),
							Seat = new Seat()
							{
								NumberRow = item.GetValue("numberRow")[i].ToInt32(),
								NumberColumn = item.GetValue("numberColumn")[i].ToInt32(),
								Category = new Category()
								{
									Name = item.GetValue("categoryName")[i].ToString(),
									Price = _categoryRepository.GetById(item.GetValue("category_id")[i].ToInt32()).Result.Price,
								},
							},
							RegisteredUser = _userRepository.GetById(item.GetValue("registeredUser_id")[i].ToInt32()).Result,

						});
					}
					else
					{
						tickets.Add(new Ticket()
						{
							Id = item.GetValue("_id")[i].ToInt32(),
							Seat = new Seat()
							{
								NumberRow = item.GetValue("numberRow")[i].ToInt32(),
								NumberColumn = item.GetValue("numberColumn")[i].ToInt32(),
								Category = _categoryRepository.GetById(item.GetValue("category_id")[i].ToInt32()).Result,
							},
							RegisteredUser = _userRepository.GetById(item.GetValue("registeredUser_id")[i].ToInt32()).Result,
							Cashier = _employeeRepository.GetById(item.GetValue("employee_id")[i].ToInt32()).Result
						});
					}
				}
			}

			// TODO: хуже этого не придумаешь

			return tickets;
		}
	}
}
