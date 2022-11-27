using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces;
using Infrastructure.Business;
using Infrastructure.Data.MongoRepository.Connection;
using Infrastructure.Data.MongoRepository.Implementations.GetAllByIdImplementations;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;

namespace Infrastructure.Data.MongoRepository.Implementations.RepositoryImplementetions
{
	public class ScheduleRepository : MainMongoRepository<Schedule>
	{
		private IGetAllById<Session> _sessionGetAllById;

		public ScheduleRepository(string connectionString)
			: base(connectionString, "schedule")
		{
			_sessionGetAllById = new SessionGetAllById(connectionString, _mongoCollection);
		}

		protected override Schedule Initialization(BsonDocument item)
			=> new Schedule()
			{
				Id = item.GetValue("_id").ToInt32(),
				Date = DateTime.Parse(item.GetValue("date").ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal),
				Hall = new Hall()
				{
					Number = item.GetValue("numberHall").ToInt32(),
				},
				Sessions = _sessionGetAllById.GetAllByIdOneToMany(item.GetValue("_id").ToInt32()).Result
			};

		[Obsolete]
		public override async Task<bool> InsertAsync(Schedule entity)
		{
			var parser = new MongoParser();
			entity.Id = parser.MaxIndex(_mongoCollection) + 1;

			var sessions = new BsonArray();

			entity.Sessions.ForEach(async item =>
			{
				var tickets = new BsonArray();

				if (item.Tickets != null)
				{
					item.Tickets.ForEach(ticket =>
					{
						var seat = new BsonDocument
						{
						{"numberRow", ticket.Seat.NumberRow},
						{"numberColumn", ticket.Seat.NumberColumn},
						{"categoryName", ticket.Seat.Category.Name},
						{"category_id", ticket.Seat.Category.Id}
						};

						if (ticket.Cashier != null)
						{
							tickets.Add(new BsonDocument{
								{ "_id", ticket.Id },
								{ "usernameRegisteredUser",ticket.RegisteredUser.Username },
								{ "registeredUser_id",ticket.RegisteredUser.Id },
								{ "usernameEmployee",ticket.Cashier.Username },
								{ "employee_id",ticket.Cashier.Id },
								{ "seat", seat }
							});
						}
						else
						{
							tickets.Add(new BsonDocument{
								{ "_id", ticket.Id },
								{ "usernameRegisteredUser",ticket.RegisteredUser.Username },
								{ "registeredUser_id",ticket.RegisteredUser.Id },
								{ "usernameEmployee", "undefaited" },
								{ "employee_id", 0 },
								{ "seat", seat }
							});
						}
					});
				}
				sessions.Add(new BsonDocument
				{
					{"_id", item.Id},
					{"nameFilm", item.Film.Name},
					{"duration", item.Film.Duration},
					{"basePrice", item.Film.BasePrice},
					{"film_id", item.Film.Id},
					{"start", item.StartTime},
					{"tickets", tickets }
				});
			});

			var document = new BsonDocument
			{
				{ "_id", entity.Id },
				{"numberHall",entity.Hall.Number },
				{"date",entity.Date },
				{"sessions", sessions },

			};

			await _mongoCollection.InsertOneAsync(document);

			return true;
		}

		public override async Task<bool> UpdateAsync(Schedule entity)
		{
			var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

			var update = Builders<BsonDocument>.Update.Set("numberHall", entity.Hall.Number);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("date", entity.Date);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("sessions", entity.Sessions);
			await _mongoCollection.UpdateOneAsync(filter, update);

			return true;
		}
	}
}