using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Core.Models.Roles.Staff.Positions;
using ApplicationCore.Domain.Interfaces.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Business
{
	public class MongoParser
	{
		private IRepository<RegisteredUser> _registeredUserRepository;
		private IRepository<Film> _filmRepository;

		public MongoParser()
		{
		}

		public MongoParser(IRepository<RegisteredUser> registeredUserRepository)
		{
			_registeredUserRepository = registeredUserRepository;
		}

		public MongoParser(IRepository<Film> filmRepository)
		{
			_filmRepository = filmRepository;
		}

		public MongoParser(IRepository<RegisteredUser> registeredUserRepository, IRepository<Film> filmRepository) : this(registeredUserRepository)
		{
			_filmRepository = filmRepository;
		}

		public int MaxIndex(IMongoCollection<BsonDocument> mongoCollection)
		{
			int maxValue = 0;
			var data = mongoCollection.Find(new BsonDocument()).Sort("{_id:-1}").Limit(1).ToList();

			if (data.Count > 0)
				maxValue = data[0].GetValue("_id").ToInt32();

			return maxValue;
		}

		// TODO: сделать парсеры
		public List<Position> ParsePositions(BsonValue value)
		=> value.AsBsonArray
			.Select(p => GetType(p[1].AsInt32, p[0].AsString))
			.ToList();

		public Position GetType(int id, string name)
		{
			if (name == "Admin")
				return new Admin() { Id = id, Name = name };

			return null;
		}

		public List<Person> ParsePersons(BsonValue value)
			=> value.AsBsonArray
			.Select(p => new Person()
			{
				Id = p[4].AsInt32,
				FirstName = p[0].AsString,
				LastName = p[1].AsString,
				MiddleName = p[2].AsString,
				Post = p[3].AsString
			})
			.ToList();

		public List<Review> ParseReviews(BsonValue value)
		=> value.AsBsonArray
			.Select(p => new Review()
			{
				Id = 0,
				RegisteredUser = _registeredUserRepository.GetById(p[2].AsInt32).Result,
				Description = p[3].AsString
			})
			.ToList();

		public List<Score> ParseScores(BsonValue value)
		=> value.AsBsonArray
			.Select(p => new Score()
			{
				Id = 0,
				RegisteredUser = _registeredUserRepository.GetById(p[2].AsInt32).Result,
				Raiting = p[3].AsInt32
			})
			.ToList();

		public List<Session> ParseSessions(BsonValue value)
		=> value.AsBsonArray
			.Select(p => new Session()
			{
				Id = p[0].AsInt32,
				Film = _filmRepository.GetById(p[4].AsInt32).Result,
				StartTime = p[5].AsDateTime
			})
			.ToList();
	}
}