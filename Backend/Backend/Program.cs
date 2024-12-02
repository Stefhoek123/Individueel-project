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
        protected Program()
        {
            // Constructor logic
        }
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<BackendDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("BackendDbContext")));

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // Swagger/OpenAPI Configuration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApiDocument(config =>
            {
                config.Title = "Controllers API";
                config.Version = "v1";
                config.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT token"));
                config.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT token", new List<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Description = "Copy 'Bearer {JWT Token}'",
                    In = OpenApiSecurityApiKeyLocation.Header,
                }));
            });

            // CORS Configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000") // Add the frontend URL here
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            builder.Services.AddScoped<IUserContext, UserContext>();
            RegisterRepos(builder);
            RegisterLogics(builder);

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi();
            }


            app.UseCors("AllowSpecificOrigin"); // CORS should be applied before authorization

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

        private static void RegisterRepos(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITextPostRepository, TextPostRepository>();
            builder.Services.AddScoped<IFamilyRepository, FamilyRepository>();
        }

        private static void RegisterLogics(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserContainer, UserContainer>();
            builder.Services.AddScoped<ITextPostContainer, TextPostContainer>();
            builder.Services.AddScoped<IFamilyContainer, FamilyContainer>();
        }
    }
}
