using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Settings;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Deserialise the RepositorySettings section into a RepositorySettings object
var repositorySettings = builder.Configuration.GetSection(nameof(RepositorySettings)).Get<RepositorySettings>();

//*******  Add services to the container  **********

// Setup the mongo DB for DI.
if (repositorySettings is not null)
{
	builder.Services.AddSingleton(_ =>
	{
		// Gets the Mongo DB settings containing the host and port from appsettings.json.
		var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
		var mongoClient = new MongoClient(mongoDbSettings?.ConnectionString);
		return mongoClient.GetDatabase(repositorySettings.RepositoryName);
	});
}

builder.Services.AddSingleton<IItemsRepository, ItemsRepository>();

// Stores the GUIDs and DateTimeOffsets as a strings so it's easier to read in the DB.
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

// SuppressAsyncSuffixInActionNames prevents the async suffic from CreatedAtAction(nameof(GetByIdAsync), new {id = item.Id}, item);
builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
