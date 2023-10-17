namespace Play.Common.Repositories;

public interface IRepository<T> where T : IIdentifiable
{
	Task CreateAsync(T entity);
	Task<IReadOnlyCollection<T>> GetAllAsync();
	Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
	Task<T> GetAsync(Guid id);
	Task UpdateAsync(T entity);
	Task RemoveAsync(Guid id);
}