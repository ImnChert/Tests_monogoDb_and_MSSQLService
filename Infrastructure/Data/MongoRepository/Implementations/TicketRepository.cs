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
    internal class TicketRepository : MainMongoRepository<Ticket>
    {
		private IRepository<Category> _categoryRepository;
		private IRepository<RegisteredUser> _userRepository;
		private IRepository<Session> _sessionRepository;
		private IRepository<Employee> _employeeRepository;

		public TicketRepository(string connectionString)
            : base(connectionString, "films")
        {
			_categoryRepository = new CategoryRepository(connectionString);
			_userRepository = new UserRepository(connectionString);
			_scheduleRepository = new ScheduleRepository(connectionString);	
			_employeeRepository = new EmployeeRepository(connectionString);
		}

        public override Task<List<Ticket>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public override Task<Ticket> GetById(int id)
        {
            throw new NotImplementedException();
        }

		public Ticket InitializationFilm(BsonDocument item, MongoParser parse) => new Ticket()
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
			Session = new Session()
			{

			}
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
				{"nameFilm",entity.Session.Film.Name },
				{"start", entity.Session.StartTime },
				{"basePrice", entity.Session.Film.BasePrice },
				{"session_id", entity.Session.Id },
				{"usernameEmployee",entity.Cashier.Username },
				{"employee_id",entity.Cashier.Id },
				{"seat", seat }
			};

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

			update = Builders<BsonDocument>.Update.Set("nameFilm", entity.Session.Film.Name);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("start", entity.Session.StartTime);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("basePrice", entity.Session.Film.BasePrice);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("session_id", entity.Session.Id);
			await _mongoCollection.UpdateOneAsync(filter, update);

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
