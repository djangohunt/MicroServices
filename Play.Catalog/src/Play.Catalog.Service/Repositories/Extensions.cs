using System.Diagnostics;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service.Repositories;

public static class Extensions
{
	public static IServiceCollection AddMongo(this IServiceCollection  services)
	{
		// Stores the GUIDs and DateTimeOffsets as a strings so it's easier to read in the DB.
		BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
		BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

		services.AddSingleton(serviceProvider =>
		{
			var config = serviceProvider.GetService<IConfiguration>();
			Debug.Assert(config != null, nameof(config) + " is null");

			// Deserialise the RepositorySettings section into a RepositorySettings object
			var repositorySettings = config.GetSection(nameof(RepositorySettings)).Get<RepositorySettings>();
			Debug.Assert(repositorySettings != null, nameof(repositorySettings) + " is null");

			// Gets the Mongo DB settings containing the host and port from appsettings.json.
			var mongoDbSettings = config.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
			var mongoClient = new MongoClient(mongoDbSettings?.ConnectionString);
			return mongoClient.GetDatabase(repositorySettings.RepositoryName);
		});

		// We don't NEED to return, but we are so we can use fluent style method chaining.
		return services;
	}

	public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) 
		where T : IIdentifiable
	{
		services.AddSingleton<IRepository<T>>(serviceProvider =>
		{
			var db = serviceProvider.GetService<IMongoDatabase>();
			return new MongoRepository<T>(db ?? throw new InvalidOperationException(), collectionName);
		});

		// We don't NEED to return, but we are so we can use fluent style method chaining.
		return services;
	}
}