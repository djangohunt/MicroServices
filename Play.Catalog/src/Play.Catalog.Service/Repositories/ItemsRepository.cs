using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories;

public class ItemsRepository
{
	// Group of objects in a DB - like a table in SQL.
	private const string itemCollectionName = "items";
	private readonly IMongoCollection<Item> itemDbCollection;

	// Filter for querying items in MongoDb
	private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

	public ItemsRepository()
	{
		var mongoClient = new MongoClient("mongodb://localhost:27017");
		IMongoDatabase? database = mongoClient.GetDatabase("Catalog");
		itemDbCollection = database.GetCollection<Item>(itemCollectionName);
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
}