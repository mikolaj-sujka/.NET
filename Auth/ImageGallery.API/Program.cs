using ImageGallery.API.Authorization;
using ImageGallery.API.DbContexts;
using ImageGallery.API.Services;
using ImageGallery.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddDbContext<GalleryContext>(options =>
{
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:ImageGalleryDBConnectionString"]);
});

// register the repository
builder.Services.AddScoped<IGalleryRepository, GalleryRepository>();

// register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// register the authorization handler
builder.Services.AddScoped<IAuthorizationHandler, MustOwnImageHandler>();

builder.Services.AddScoped<IAuthorizationRequirementData, MustOwnImageAttribute>();

// register AutoMapper-related services
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5001/";
        options.Audience = "imagegalleryapi";
        options.TokenValidationParameters = new()
        {
            ValidTypes = ["at+jwt"],
            RoleClaimType = "role",
            NameClaimType = "given_name"
        };
    });

builder.Services.AddAuthorization(authOptions =>
{
    authOptions.AddPolicy("UserCanAddImage",
        AuthorizationPolicies.CanAddImage());

    authOptions.AddPolicy("ClienApplicationCanWrite", policyBuilder =>
    {
        policyBuilder.RequireClaim("scope", "imagegalleryapi.write");
    }); 

    authOptions.AddPolicy("MustOwnImage", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.AddRequirements(new MustOwnImageRequirement());
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
