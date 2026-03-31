using mtkpm.Infrastructure;
using mtkpm.Infrastructure.Data.Contexts;
using mtkpm.Infrastructure.Services.SeedData;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using mtkpm.Domain.Entities.Identity_Auth;
using mtkpm.Application;
using mtkpm.Middleware;
using System.Reflection;
using MediatR;

namespace mtkpm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Application Layer (MediatR, AutoMapper, FluentValidation)
            builder.Services.AddApplication();

            // Add Infrastructure Layer (DbContext, Identity, JWT, Repositories, Services)
            builder.Services.AddInfrastructure(builder.Configuration);

            // Add MediatR Pipeline Behaviors for Validation
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            
            // Configure Swagger with JWT Authentication
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MTKPM E-Commerce API",
                    Version = "v1",
                    Description = "E-Commerce API with Clean Architecture + CQRS Pattern",
                    Contact = new OpenApiContact
                    {
                        Name = "MTKPM Team",
                        Email = "support@mtkpm.com"
                    }
                });

                // Add XML comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                // Add JWT Authentication to Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập 'Bearer' [space] và token của bạn\n\nVí dụ: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Use Exception Handling Middleware
            app.UseExceptionHandlingMiddleware();

            // Use Request/Response Logging Middleware
            app.UseMiddleware<mtkpm.Middleware.RequestResponseLoggingMiddleware>();

            // Initialize Database with seed data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dataSeeder = services.GetRequiredService<DataSeeder>();
                    await dataSeeder.SeedAsync();
                    
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogInformation("Database seeded successfully");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MTKPM E-Commerce API v1");
                    c.RoutePrefix = string.Empty;
                    c.DocumentTitle = "MTKPM API Documentation";
                    c.DefaultModelsExpandDepth(-1);
                });
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            // IMPORTANT: Authentication must come before Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
