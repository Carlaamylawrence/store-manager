using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;

namespace StoreManger
{
  public class Startup
  {

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddCors(options =>
      {
        options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
              builder.WithOrigins("*") 
                     .AllowAnyHeader()
                     .AllowAnyMethod();
            });
      });
   
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // Production error handling
        app.UseExceptionHandler("/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseCors("*");
      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

    }
  }
}
