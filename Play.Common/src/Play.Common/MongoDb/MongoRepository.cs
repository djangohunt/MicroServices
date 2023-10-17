using System.Linq.Expressions;
using MongoDB.Driver;

namespace Play.Common.MongoDb;

public class MongoRepository<T> : IRepository<T> where T : IIdentifiable
{
	// Group of objects in a DB - like a table in SQL.
	private readonly string _collectionName;
	private readonly IMongoCollection<T> _dbCollection;

	// Filter for querying items in MongoDb
	private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

	public MongoRepository(IMongoDatabase db, string collectionName)
	{
		_collectionName = collectionName;
		_dbCollection = db.GetCollection<T>(_collectionName);
	}

	// Good API pattern is to return IReadOnlyCollection as we wouldn't expect the caller to edit this.
	public async Task<IReadOnlyCollection<T>> GetAllAsync()
	{
		return await _dbCollection.Find(filterBuilder.Empty).ToListAsync();
	}

	public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
	{
		return await _dbCollection.Find(filter).ToListAsync();
	}

	public async Task<T> GetAsync(Guid id)
	{
		// Creates an equality filter to be used in the collection Find method.
		FilterDefinition<T>? entityByIdFilter = filterBuilder.Eq(entity => entity.Id, id);
		return await _dbCollection.Find(entityByIdFilter).FirstOrDefaultAsync();
	}

	public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
	{
		return await _dbCollection.Find(filter).FirstOrDefaultAsync();
	}

	public async Task CreateAsync(T entity)
	{
		if (entity is null)
		{
			throw new ArgumentNullException(nameof(entity));
		}

		await _dbCollection.InsertOneAsync(entity);
	}

	public async Task UpdateAsync(T entity)
	{
		if (entity is null)
		{
			throw new ArgumentNullException(nameof(entity));
		}

		FilterDefinition<T>? entityByIdFilter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
		await _dbCollection.ReplaceOneAsync(entityByIdFilter, entity);
	}

	public async Task RemoveAsync(Guid id)
	{
		FilterDefinition<T>? entityByIdFilter = filterBuilder.Eq(existingEntity => existingEntity.Id, id);
		await _dbCollection.DeleteOneAsync(entityByIdFilter);
	}
}