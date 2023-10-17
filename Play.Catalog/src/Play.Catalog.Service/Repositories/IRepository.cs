using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories;

public interface IRepository<T> where T : IIdentifiable
{
	Task CreateAsync(T entity);
	Task<IReadOnlyCollection<T>> GetAllAsync();
	Task<T> GetAsync(Guid id);
	Task UpdateAsync(T entity);
	Task RemoveAsync(Guid id);
}