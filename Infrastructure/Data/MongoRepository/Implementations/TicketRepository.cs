using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Interfaces;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Business;
using Infrastructure.Data.MongoRepository.Connection;
using Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using SharpCompress.Common;
using System.Globalization;

namespace Infrastructure.Data.MongoRepository.Implementations
{
	public class TicketRepository : IGetAllById<Ticket>
	{
		private readonly IMongoCollection<BsonDocument> _mongoCollection;
		private IRepository<Category> _categoryRepository;
		private IRepository<RegisteredUser> _userRepository;
		private IRepository<Employee> _employeeRepository;

		public TicketRepository(string connectionString, IMongoCollection<BsonDocument> mongoCollection)
		{
			_mongoCollection = mongoCollection;
			_categoryRepository = new CategoryRepository(connectionString);
			_userRepository = new UserRepository(connectionString);
			_employeeRepository = new EmployeeRepository(connectionString);
		}

		//private IRepository<Category> _categoryRepository;
		//private IRepository<RegisteredUser> _userRepository;
		//private IRepository<Employee> _employeeRepository;

		//public TicketRepository(string connectionString)
		//	: base(connectionString, "ticket")
		//{
		//	_categoryRepository = new CategoryRepository(connectionString);
		//	_userRepository = new UserRepository(connectionString);
		//	_employeeRepository = new EmployeeRepository(connectionString);
		//}

		//public override async Task<List<Ticket>> GetAllAsync()
		//{
		//	var filter = new BsonDocument();
		//	var tickets = new List<Ticket>();

		//	using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
		//	{
		//		while (await cursor.MoveNextAsync())
		//		{
		//			IEnumerable<BsonDocument> user = cursor.Current;

		//			foreach (BsonDocument item in user)
		//			{
		//				tickets.Add(InitializationTicket(item));
		//			}
		//		}
		//	}

		//	return tickets;
		//}

		//public override async Task<Ticket> GetById(int id)
		//{
		//	var ticket = new Ticket();
		//	var filter = new BsonDocument("_id", id);

		//	using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
		//	{
		//		if (await cursor.MoveNextAsync())
		//		{
		//			if (cursor.Current.Count() == 0)
		//				return null;

		//			var elements = cursor.Current.ToList();
		//			BsonDocument item = elements[0];

		//			ticket = InitializationTicket(item);
		//		}
		//	}

		//	return ticket;
		//}

		//public Ticket InitializationTicket(BsonDocument item) => new Ticket()
		//{
		//	Id = item.GetValue("_id").ToInt32(),
		//	Seat = new Seat()
		//	{
		//		NumberRow = item.GetValue("numberRow").ToInt32(),
		//		NumberColumn = item.GetValue("numberColumn").ToInt32(),
		//		Category = new Category()
		//		{
		//			Name = item.GetValue("categoryName").ToString(),
		//			Price = _categoryRepository.GetById(item.GetValue("_id").ToInt32()).Result.Price,
		//		},
		//	},
		//	RegisteredUser = _userRepository.GetById(item.GetValue("registeredUser_id").ToInt32()).Result,
		//	Cashier = _employeeRepository.GetById(item.GetValue("registeredUser_id").ToInt32()).Result

		//	// TODO: коммент
		//};

		//public override async Task<bool> InsertAsync(Ticket entity)
		//{
		//	var parser = new MongoParser();
		//	entity.Id = parser.MaxIndex(_mongoCollection) + 1;

		//	var seat = new BsonDocument()
		//	{
		//		{"numberRow", entity.Seat.NumberRow},
		//		{"numberColumn", entity.Seat.NumberColumn},
		//		{"categoryName", entity.Seat.Category.Name},
		//		{"category_id", entity.Seat.Category.Id}
		//	};

		//	var document = new BsonDocument
		//	{
		//		{ "_id", entity.Id },
		//		{"usernameRegisteredUser",entity.RegisteredUser.Username },
		//		{"registeredUser_id",entity.RegisteredUser.Id },
		//		{"usernameEmployee",entity.Cashier.Username },
		//		{"employee_id",entity.Cashier.Id },
		//		{"seat", seat }
		//	};

		//	await _mongoCollection.InsertOneAsync(document);

		//	return true;
		//}

		//public override async Task<bool> UpdateAsync(Ticket entity)
		//{
		//	var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

		//	var update = Builders<BsonDocument>.Update.Set("usernameRegisteredUser", entity.RegisteredUser.Username);
		//	await _mongoCollection.UpdateOneAsync(filter, update);

		//	update = Builders<BsonDocument>.Update.Set("registeredUser_id", entity.RegisteredUser.Id);
		//	await _mongoCollection.UpdateOneAsync(filter, update);

		//	update = Builders<BsonDocument>.Update.Set("usernameEmployee", entity.Cashier.Username);
		//	await _mongoCollection.UpdateOneAsync(filter, update);

		//	update = Builders<BsonDocument>.Update.Set("employee_id", entity.Cashier.Id);
		//	await _mongoCollection.UpdateOneAsync(filter, update);

		//	update = Builders<BsonDocument>.Update.Set("seat", entity.Seat);
		//	await _mongoCollection.UpdateOneAsync(filter, update);

		//	return true;
		//}

		//public async Task<List<Ticket>> GetAllByIdOneToMany(int id)
		//{
		//	var tickets = new List<Ticket>();
		//	var filter = new BsonDocument("session_id", id);

		//	using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
		//	{
		//		while (await cursor.MoveNextAsync())
		//		{
		//			IEnumerable<BsonDocument> user = cursor.Current;

		//			foreach (BsonDocument item in user)
		//			{
		//				tickets.Add(InitializationTicket(item));
		//			}
		//		}
		//	}

		//	return tickets;
		//}
		public async Task<List<Ticket>> GetAllByIdOneToMany(int id)
		{
			var pipeline = new BsonDocument
			{
				{"$unwind", "$sessions.tickets"}
			};

			var pipeline2 = new BsonDocument
			{
				{"$match", new BsonDocument{
					{"_id", id }
				}}
			};

			var seat = new BsonDocument()
			{
				{"numberRow", "$sessions.tickets.seat.numberRow"},
				{"numberColumn", "$sessions.tickets.seat.numberColumn"},
				{"categoryName", "$sessions.tickets.seat.categoryName"},
				{"category_id", "$sessions.tickets.seat.category_id"}
			};

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
						//{"seat", "$sessions.tickets.seat" },
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
				if (item.GetValue("employee_id").ToInt32() == 0)
				{
					tickets.Add(new Ticket()
					{
						Id = item.GetValue("_id").ToInt32(),
						Seat = new Seat()
						{
							NumberRow = item.GetValue("numberRow").ToInt32(),
							NumberColumn = item.GetValue("numberColumn").ToInt32(),
							Category = new Category()
							{
								Name = item.GetValue("categoryName").ToString(),
								Price = _categoryRepository.GetById(item.GetValue("category_id").ToInt32()).Result.Price,
							},
						},
						RegisteredUser = _userRepository.GetById(item.GetValue("registeredUser_id").ToInt32()).Result,

					});
				}
				else
				{
					tickets.Add(new Ticket()
					{
						Id = item.GetValue("_id").ToInt32(),
						Seat = new Seat()
						{
							NumberRow = item.GetValue("numberRow").ToInt32(),
							NumberColumn = item.GetValue("numberColumn").ToInt32(),
							Category = new Category()
							{
								Name = item.GetValue("categoryName").ToString(),
								Price = _categoryRepository.GetById(item.GetValue("category_id").ToInt32()).Result.Price,
							},
						},
						RegisteredUser = _userRepository.GetById(item.GetValue("registeredUser_id").ToInt32()).Result,
						Cashier = _employeeRepository.GetById(item.GetValue("employee_id").ToInt32()).Result
					});
				}
			}

			return tickets;
		}
	}
}
