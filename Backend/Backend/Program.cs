
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NSwag.Generation.Processors.Security;
using NSwag;
using Repositories;
using Microsoft.AspNetCore.Builder;
using DAL.Containers;
using Interface;

namespace Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<BackendDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("BackendDbContext")));

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddOpenApiDocument(config =>
            {
                config.Title = "Controllers API";
                config.Version = "v1";
                config.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT token"));

                config.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT token", new List<string>(),
                    new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        Description = "Copy 'Bearer {JWT Token}'",
                        In = OpenApiSecurityApiKeyLocation.Header,
                    }));
            });

            builder.Services.AddCors(setup =>
            {
                setup.AddDefaultPolicy(b =>
                {
                    b.SetIsOriginAllowed(origin => origin == builder.Configuration.GetValue<string>("BackendUrl"));
                    b.WithHeaders("Content-Type");
                    // b.WithHeaders("Authorization");
                    b.WithMethods("GET", "POST", "PUT", "DELETE");
                });
            });

            builder.Services.AddScoped<IUserContext, UserContext>();
            RegisterRepos(builder);
            RegisterLogics(builder);

            WebApplication? app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUI();
            }

            //Ensure database is created, not needed for development. 
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<BackendDbContext>();
                context.Database.EnsureCreated();
                // DbInitializer.Initialize(context);
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors();
            app.MapControllers();

            app.Run();

        }

        private static void RegisterRepos(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
        }

        private static void RegisterLogics(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserContainer, UserContainer>();
        }
    }
}
