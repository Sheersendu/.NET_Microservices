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
	5, // no of retries
	retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) // Exponential backoff
					+ TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)) // Random jitter
))
.AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
	3, // no of failing requests before breaking the circuit
	TimeSpan.FromSeconds(15) // time we are going to keep our circuit open
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
