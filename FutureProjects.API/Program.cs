
using FutureProjects.Application;
using FutureProjects.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace FutureProjects.API
{
    public class Program
    {


        public static void Main(string[] args)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.AllowAnyHeader()
                                            .WithOrigins("https://localhost:4200",
                                            "http://localhost:4200")
                                            .WithMethods("POST", "DELETE");
                                  });
            });


            // Add services to the container.
            builder.Logging.ClearProviders();

            //using var log = new LoggerConfiguration()
            //    .WriteTo.Console()
            //    .WriteTo.File("log.txt")
            //    .CreateLogger();

            //log.Information("Hello, Serilog!");

            var paths = builder.Configuration["Serilog:LogPath"];

            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);

                configuration.MinimumLevel.Information()
                    .Enrich.WithProperty("ApplicationContext", "Ocelot.APIGateway")
                    .Enrich.FromLogContext()
                    .WriteTo.File(builder.Configuration["Serilog:LogPath"])
                    .WriteTo.Console()
                    .ReadFrom.Configuration(context.Configuration);
            });

            builder.Services.AddControllers();

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Future Projects", Version = "v1.0.0", Description = "Future Projects Auth API" });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };
                c.AddSecurityRequirement(securityRequirement);
            });


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                           .AddJwtBearer(
                               options =>
                               {
                                   options.TokenValidationParameters = GetTokenValidationParameters(builder.Configuration);

                                   options.Events = new JwtBearerEvents
                                   {
                                       OnAuthenticationFailed = (context) =>
                                       {
                                           if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                           {
                                               context.Response.Headers.Add("IsTokenExpired", "true");
                                           }
                                           return Task.CompletedTask;
                                       }
                                   };
                               });





            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();


            }
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }


        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:ValidIssuer"],
                ValidAudience = configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                ClockSkew = TimeSpan.Zero,
            };
        }
    }
}
