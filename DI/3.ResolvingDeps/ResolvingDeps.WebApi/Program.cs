using ResolvingDeps.WebApi.Filters;
using ResolvingDeps.WebApi.HostedServices;
using ResolvingDeps.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(provider =>
{
    var logger = provider.GetRequiredService<ILogger<DurationLoggerFilter>>();
    return new DurationLoggerFilter(logger);
});

builder.Services.AddHostedService<BackgroundTicker>();

var app = builder.Build();

app.UseMiddleware<DurationLoggerMiddleware>();
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
