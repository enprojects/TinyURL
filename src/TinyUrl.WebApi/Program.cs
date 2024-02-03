
using Munters.TeamViewerApi.Extensions;
using TinyUrl.Backend.Accessors;
using TinyUrl.Backend.Configurations;
using TinyUrl.Backend.Engines;
using TinyUrl.Backend.Infrastructure;
using TinyUrl.Backend.Mangers;
using TinyUrl.WebApi.Extension;

namespace TinyUrl.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.
            var services = builder.Services;
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAutoMapper();

            // adding infrastructure
            services.AddSingleton<ICacheRepos, CacheRepos>();
            services.AddSingleton<IDbContext, MongoDbContext>();

            // adding configuration 
            services.AddConfiguration<CacheConfiguration>(configuration, "CacheConfiguration");
            services.AddConfiguration<DbConfiguration>(configuration, "DbConfiguration");
            services.AddConfiguration<TinyUrlConfiguration>(configuration, "TinyUrlConfiguration");

            //adding accessors
            services.AddSingleton<ITinyUrlAccessor, TinyUrlAccessor>();

            // adding managers  
            services.AddSingleton<ITinyUrlManager, TinyUrlManager>();

            // engines

            services.AddSingleton<ITinyUrlEngine, TinyUrlEngine>();

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
        }
    }
}
