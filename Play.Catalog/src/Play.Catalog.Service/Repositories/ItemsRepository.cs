using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories;

public class ItemsRepository : IItemsRepository
{
	// Group of objects in a DB - like a table in SQL.
	private const string itemCollectionName = "items";
	private readonly IMongoCollection<Item> itemDbCollection;

	// Filter for querying items in MongoDb
	private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

	public ItemsRepository(IMongoDatabase db)
	{
		itemDbCollection = db.GetCollection<Item>(itemCollectionName);
	}

	// Good API pattern is to return IReadOnlyCollection as we wouldn't expect the caller to edit this.
	public async Task<IReadOnlyCollection<Item>> GetAllAsync()
	{
		return await itemDbCollection.Find(filterBuilder.Empty).ToListAsync();
	}

	public async Task<Item> GetAsync(Guid id)
	{
		// Creates an equality filter to be used in the collection Find method.
		FilterDefinition<Item>? itemByIdFilter = filterBuilder.Eq(item => item.Id, id);
		return await itemDbCollection.Find(itemByIdFilter).FirstOrDefaultAsync();
	}

	public async Task CreateAsync(Item item)
	{
		if (item is null)
		{
			throw new ArgumentNullException(nameof(item));
		}

		await itemDbCollection.InsertOneAsync(item);
	}

	public async Task UpdateAsync(Item item)
	{
		if (item is null)
		{
			throw new ArgumentNullException(nameof(item));
		}

		FilterDefinition<Item>? itemByIdFilter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
		await itemDbCollection.ReplaceOneAsync(itemByIdFilter, item);
	}

	public async Task RemoveAsync(Guid id)
	{
		FilterDefinition<Item>? itemByIdFilter = filterBuilder.Eq(existingItem => existingItem.Id, id);
		await itemDbCollection.DeleteOneAsync(itemByIdFilter);
	}
}