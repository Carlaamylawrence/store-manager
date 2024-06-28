using Serilog;
using StoreManager.ApplicationService;
using StoreManager.Infrastructure;
using System.Data;
using System.Data.SqlClient;

namespace StoreManagerApp
{
  public class Program
  {
    public static void Main(string[] args)
    {
      Log.Logger = new LoggerConfiguration()
          .WriteTo.Console()
          .CreateLogger();

      Log.Information("Starting up");

      try
      {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((ctx, lc) => lc
            .WriteTo.Console()
            .ReadFrom.Configuration(ctx.Configuration));

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddTransient<IDbConnection, SqlConnection>(_ => new SqlConnection(connectionString));
        builder.Services.AddTransient<ProductsApplicationService>();
        builder.Services.AddTransient<IProductsRepository, ProductsRepository>();
        builder.Services.AddTransient<BranchesApplicationService>();
        builder.Services.AddTransient<IBranchesRepository, BranchesRepository>();
        builder.Services.AddTransient<BranchProductApplicationService>();
        builder.Services.AddTransient<IBranchProductRepository, BranchProductRepository>();
        builder.Services.AddCors(options =>
        {
          options.AddPolicy("CorsPolicy",
              builder => builder.WithOrigins("*")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .WithExposedHeaders("Content-Disposition"));
        });

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
        app.UseCors("CorsPolicy");
     

        app.Run();
      }
      catch (Exception ex)
      {
        Log.Fatal(ex, "Application terminated unexpectedly");
      }
      finally
      {
        Log.CloseAndFlush();
      }
    }
  }
}
