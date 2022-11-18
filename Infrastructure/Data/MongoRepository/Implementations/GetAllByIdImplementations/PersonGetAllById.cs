using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository.Implementations.GetAllByIdImplementations
{
	internal class PersonGetAllById : IGetAllById<Person>
	{
		private readonly IMongoCollection<BsonDocument> _mongoCollection;

		public PersonGetAllById(IMongoCollection<BsonDocument> mongoCollection)
		{
			_mongoCollection = mongoCollection;
		}

		public async Task<List<Person>> GetAllByIdOneToMany(int id)
		{
			var pipeline = new BsonDocument
			{
				{"$unwind", "$staff"}
			};

			var pipeline2 = new BsonDocument
			{
				{"$match", new BsonDocument{
					{"_id", id }
				}}
			};

			var pipeline3 = new BsonDocument
			{
				{
					"$project", new BsonDocument
					{
						{"staff_id", "$staff.staff_id"},
						{"firstName", "$staff.firstName"},
						{"lastName", "$staff.lastName"},
						{"middleName", "$staff.middleName"},
						{"post", "$staff.post"},
					}
				}
			};

			var pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
			List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

			var people = new List<Person>();

			foreach (BsonDocument item in results)
			{
				people.Add(new Person()
				{
					Id = item.GetValue("staff_id").ToInt32(),
					FirstName = item.GetValue("firstName").ToString(),
					LastName = item.GetValue("lastName").ToString(),
					MiddleName = item.GetValue("middleName").ToString(),
					Post = item.GetValue("post").ToString()
				});
				;
			}

			return people;
		}
	}
}
