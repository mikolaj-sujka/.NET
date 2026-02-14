using System.Text.Json.Serialization;
using Dometrain.EFCore.API.Data;
using Dometrain.EfCore.API.Repositories;
using Dometrain.EFCore.API.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IGenreRepository, GenreRepository>();
builder.Services.AddTransient<IBatchGenreService, BatchGenreService>();
builder.Services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Configure Serilog
var serilog = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Configure it for Microsoft.Extensions.Logging
 builder.Services.AddSerilog(serilog);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the DbContext
// Note: We are using Scoped for the DbContext, which is the recommended lifetime for EF Core contexts in web applications.
// The Singleton lifetime is not suitable for DbContext as it can lead to issues with concurrency and memory leaks.
// Using Scoped Lifetime means that a new instance of MoviesContext will be created for each HTTP request, which is ideal for handling database operations in a web application.
builder.Services.AddDbContext<MoviesContext>(optionsBuilder =>
    {
        var connectionString = builder.Configuration.GetConnectionString("MoviesContext");
        optionsBuilder
            .UseSqlServer(connectionString, sqlBuilder => sqlBuilder.MaxBatchSize(50)) // Configure SQL Server provider with a maximum batch size of 50 for efficient bulk operations
            //.UseLazyLoadingProxies() // Enable lazy loading of related entities, allowing navigation properties to be loaded on demand when accessed. Be cautious with this in high-traffic applications, as it can lead to N+1 query issues if not used carefully.
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging() // Only enable in development or when necessary, as it can log sensitive information - avoid in production environments.
            .LogTo(
                Console.WriteLine,
                [DbLoggerCategory.Database.Command.Name],
                LogLevel.Information);
    },
    ServiceLifetime.Scoped,
    ServiceLifetime.Singleton);

var app = builder.Build();

// Check if the DB was migrated
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MoviesContext>();
    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
    if (pendingMigrations.Any())
        throw new Exception("Database is not fully migrated for MoviesContext.");
}

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