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

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            builder.Services.AddDistributedMemoryCache();
            // builder.Services.AddSession();
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
                        policy.WithOrigins("http://localhost:3000", "http://localhost:3001") // Add the frontend URL here
                            .AllowCredentials()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            // Add authentication with cookies
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/auth/login";
                    options.LogoutPath = "/auth/logout";
                    options.AccessDeniedPath = "/auth/access-denied";
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Force HTTPS in production
                    options.Cookie.SameSite = SameSiteMode.Lax; // Or SameSiteMode.Strict if no cross-origin is needed
                    options.Cookie.IsEssential = true;
                });


            // Add session support
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".AspNetCore.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            builder.Services.AddScoped<SessionVariables>();
            RegisterRepos(builder);
            RegisterLogics(builder);

            builder.Services.AddDirectoryBrowser();

            WebApplication app = builder.Build();
            
            app.UseCors("AllowSpecificOrigin"); // CORS should be applied before authorization

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<ChatHub>("/chatHub");
            //});

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
                RequestPath = "/Uploads"
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi();
            }

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void RegisterRepos(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IFamilyRepository, FamilyRepository>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();
        }

        private static void RegisterLogics(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserContainer, UserContainer>();
            builder.Services.AddScoped<IPostContainer, PostContainer>();
            builder.Services.AddScoped<IFamilyContainer, FamilyContainer>();
            builder.Services.AddScoped<IChatContainer, ChatContainer>();
        }
    }
}
