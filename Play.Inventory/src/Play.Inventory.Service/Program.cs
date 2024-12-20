using Play.Common.MongoDB;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;
using Polly;
using Polly.Timeout;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddMongo()
				.AddMongoRepository<InventoryItem>("inventoryitems");
Random jitterer = new();
builder.Services.AddHttpClient<CatalogClient>(client =>
{
	client.BaseAddress = new Uri("http://localhost:5103");
})
.AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(
	5,
	retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) // Exponential backoff
					+ TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)) // Random jitter
))
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(1)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
