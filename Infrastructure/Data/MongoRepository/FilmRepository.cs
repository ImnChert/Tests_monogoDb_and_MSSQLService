using Infrastructure.Data.MongoRepository.Connection;

namespace Infrastructure.Data.MongoRepository
{
	internal class FilmRepository : MainMongoRepository
	{
		public FilmRepository(string connectionString) 
			: base(connectionString, "films")
		{
		}
	}
}
