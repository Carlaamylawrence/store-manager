using StoreManager.ApplicationService;
using StoreManager.Infrastructure;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IDbConnection, SqlConnection>(_ => new SqlConnection(connectionString));
builder.Services.AddTransient<ProductsApplicationService>();
builder.Services.AddTransient<IProductsRepository, ProductsRepository>();
builder.Services.AddTransient<BranchesApplicationService>();
builder.Services.AddTransient<IBranchesRepository, BranchesRepository>();
builder.Services.AddTransient<BranchProductApplicationService>();
builder.Services.AddTransient<IBranchProductRepository, BranchProductRepository>();

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
