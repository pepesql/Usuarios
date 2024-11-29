
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;
using Usuarios.Extensions;
using Usuarios.Models;
using Usuarios.Repositorios;
using Usuarios.Services;

namespace Usuarios
{
    public class Program
    {
        public IConfiguration Configuration { get; }

        public static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            LogManager.Setup(option => option.LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/Nlog.config")));

            builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
            builder.Services.AddScoped<IPasswordsManagers,PasswordsManager>();
            builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();

            builder.Services.AddScoped<INotificationManager, NotificationManager>();

            builder.Services.AddResponseCaching();
           
           // builder.Services.AddSingleton<IWebSocketConnectionManager, WebSocketConnectionManager>();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string"
        + "'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(connectionString));



            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options => {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = false,
                      ValidateAudience = false,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                  };
              });


            builder.Services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Autorizacion Standar, Usar Bearer. Ejemplo \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });


                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);



            });

           builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WEBAPI", Version = "v1", Description ="Ejemplo de documentacion"  });
            });


            var app = builder.Build();

            var logger = app.Services.GetRequiredService<ILoggerManager>();

            app.ConfigureExceptionHandler(logger);


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseResponseCaching();
            app.MapControllers();


           

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromMinutes(2), // Mantiene viva la conexión
                ReceiveBufferSize = 4 * 1024 // Tamaño del buffer de recepción
            };
            app.UseWebSockets(webSocketOptions);



            //app.UseWebSockets();

            // Middleware personalizado para manejar las conexiones WebSocket
            app.UseMiddleware<WebSocketMiddleware>();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            app.UseWebSockets();



            //esta parte es para generar mas documentacion de swagger

            



           app.Run();
        }
    }
}
