using ApplicationCore.Domain.Core.Models;

namespace ApplicationCore.Domain.Interfaces
{
	public interface IManyToManyRepository<T> where T : EntityBase
	{
		public Task<List<T>> GetManyToManyAsync(int id);
		public Task SetManyToMany(int id, List<T> values);
	}
}
