using ApplicationCore.Domain.Core.Models.Cinema.Films;
using Infrastructure.Data.MongoRepository.Connection;

namespace Infrastructure.Data.MongoRepository.Implementations
{
    internal class FilmRepository : MainMongoRepository<Film>
    {
        public FilmRepository(string connectionString)
            : base(connectionString, "films")
        {
        }

        public override Task<List<Film>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public override Task<Film> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> InsertAsync(Film entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> UpdateAsync(Film entity)
        {
            throw new NotImplementedException();
        }
    }
}
