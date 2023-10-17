using Play.Catalog.Service.Entities;
using Play.Common.MongoDb;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//*******  Add services to the container  **********

// Setup the mongo DB for DI
builder.Services
	.AddMongo()
	.AddMongoRepository<Item>("items");

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
