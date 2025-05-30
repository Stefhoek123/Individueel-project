using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NSwag.Generation.Processors.Security;
using NSwag;
using Repositories;
using Microsoft.AspNetCore.Builder;
using DAL.Containers;
using Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.SignalR;
using Backend.Hubs;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Models;

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

            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbDb = Environment.GetEnvironmentVariable("DB_DB");
            var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");

            var connectionStringTemplate = builder.Configuration.GetConnectionString("BackendDbContext");
            var connectionString = connectionStringTemplate?
                .Replace("{DB_SERVER}", dbServer)
                .Replace("{DB_DB}", dbDb)
                .Replace("{DB_USER}", dbUser)
                .Replace("{DB_PASSWORD}", dbPassword);

            builder.Services.AddDbContext<BackendDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
                    ValidAudience = builder.Configuration["JwtConfig:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });

            builder.Services.AddAuthorization();

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            builder.Services.AddSignalR();

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
                        policy.WithOrigins("http://192.168.61.1:3000", "http://192.168.61.1:3001", "http://localhost:3000", "http://localhost:3001") // Add the frontend URL here
                            .AllowCredentials()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            RegisterRepos(builder);
            RegisterLogics(builder);

            builder.Services.AddDirectoryBrowser();

            WebApplication app = builder.Build();

            // CORS should be applied before authentication and authorization
            app.UseCors("AllowSpecificOrigin");

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
                RequestPath = "/Uploads"
            });

            app.MapPost("broadcast",
                async (Chat message, IHubContext<ChatHub, IChatClient> context) =>
                {
                    await context.Clients.All.ReceiveMessage(message);

                    return Results.NoContent();
                });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi();
            }

            // Session, Authentication, and Authorization order
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<ChatHub>("/chat");

            app.MapControllers();

            app.Run();
        }

        private static void RegisterRepos(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IFamilyRepository, FamilyRepository>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<JwtService>();
        }

        private static void RegisterLogics(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserContainer, UserContainer>();
            builder.Services.AddScoped<IPostContainer, PostContainer>();
            builder.Services.AddScoped<IFamilyContainer, FamilyContainer>();
            builder.Services.AddScoped<IChatContainer, ChatContainer>();
            builder.Services.AddScoped<JwtService>();
        }
    }
}
