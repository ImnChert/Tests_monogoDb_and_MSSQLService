using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Business;
using Infrastructure.Data.MongoRepository.Connection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository.Implementations
{
	public class TicketRepository : MainMongoRepository<Ticket>
	{
		private IRepository<Category> _categoryRepository;
		private IRepository<RegisteredUser> _userRepository;
		private IRepository<Session> _sessionRepository;
		private IRepository<Employee> _employeeRepository;

		public TicketRepository(string connectionString)
			: base(connectionString, "ticket")
		{
			_categoryRepository = new CategoryRepository(connectionString);
			_userRepository = new UserRepository(connectionString);
			_sessionRepository = new SessionRepository(connectionString);
			_employeeRepository = new EmployeeRepository(connectionString);
		}

		public override async Task<List<Ticket>> GetAllAsync()
		{
			var filter = new BsonDocument();
			var tickets = new List<Ticket>();

			using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
			{
				while (await cursor.MoveNextAsync())
				{
					IEnumerable<BsonDocument> user = cursor.Current;

					foreach (BsonDocument item in user)
					{
						tickets.Add(InitializationTicket(item));
					}
				}
			}

			return tickets;
		}

		public override async Task<Ticket> GetById(int id)
		{
			var ticket = new Ticket();
			var filter = new BsonDocument("_id", id);

			using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
			{
				if (await cursor.MoveNextAsync())
				{
					if (cursor.Current.Count() == 0)
						return null;

					var elements = cursor.Current.ToList();
					BsonDocument item = elements[0];

					ticket = InitializationTicket(item);
				}
			}

			return ticket;
		}

		public Ticket InitializationTicket(BsonDocument item) => new Ticket()
		{
			Id = item.GetValue("_id").ToInt32(),
			Seat = new Seat()
			{
				NumberRow = item.GetValue("numberRow").ToInt32(),
				NumberColumn = item.GetValue("numberColumn").ToInt32(),
				Category = new Category()
				{
					Name = item.GetValue("categoryName").ToString(),
					Price = _categoryRepository.GetById(item.GetValue("_id").ToInt32()).Result.Price,
				},
			},
			RegisteredUser = _userRepository.GetById(item.GetValue("registeredUser_id").ToInt32()).Result,
			Cashier = _employeeRepository.GetById(item.GetValue("registeredUser_id").ToInt32()).Result,
			//Session = _sessionRepository.GetById(item.GetValue("session_id").ToInt32()).Result

			// TODO: коммент
		};

		public override async Task<bool> InsertAsync(Ticket entity)
		{
			var parser = new MongoParser();
			entity.Id = parser.MaxIndex(_mongoCollection) + 1;

			var seat = new BsonDocument()
			{
				{"numberRow", entity.Seat.NumberRow},
				{"numberColumn", entity.Seat.NumberColumn},
				{"categoryName", entity.Seat.Category.Name},
				{"category_id", entity.Seat.Category.Id}
			};

			var document = new BsonDocument
			{
				{ "_id", entity.Id },
				{"usernameRegisteredUser",entity.RegisteredUser.Username },
				{"registeredUser_id",entity.RegisteredUser.Id },
				//{"nameFilm",entity.Session.Film.Name },
				//{"start", entity.Session.StartTime },
				//{"basePrice", entity.Session.Film.BasePrice },
				//{"session_id", entity.Session.Id },
				{"usernameEmployee",entity.Cashier.Username },
				{"employee_id",entity.Cashier.Id },
				{"seat", seat }
			};
			// TODO: понять что делать
			await _mongoCollection.InsertOneAsync(document);

			return true;
		}

		public override async Task<bool> UpdateAsync(Ticket entity)
		{
			var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

			var update = Builders<BsonDocument>.Update.Set("usernameRegisteredUser", entity.RegisteredUser.Username);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("registeredUser_id", entity.RegisteredUser.Id);
			await _mongoCollection.UpdateOneAsync(filter, update);

			//update = Builders<BsonDocument>.Update.Set("nameFilm", entity.Session.Film.Name);
			//await _mongoCollection.UpdateOneAsync(filter, update);

			//update = Builders<BsonDocument>.Update.Set("start", entity.Session.StartTime);
			//await _mongoCollection.UpdateOneAsync(filter, update);

			//update = Builders<BsonDocument>.Update.Set("basePrice", entity.Session.Film.BasePrice);
			//await _mongoCollection.UpdateOneAsync(filter, update);

			//update = Builders<BsonDocument>.Update.Set("session_id", entity.Session.Id);
			//await _mongoCollection.UpdateOneAsync(filter, update);

			// TODO: коммент

			update = Builders<BsonDocument>.Update.Set("usernameEmployee", entity.Cashier.Username);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("employee_id", entity.Cashier.Id);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("seat", entity.Seat);
			await _mongoCollection.UpdateOneAsync(filter, update);

			return true;
		}
	}
}
