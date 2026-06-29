using System.Text;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SaveTheDate.Api.Filters;
using SaveTheDate.Application;
using SaveTheDate.Application.Common.Mappings;
using SaveTheDate.Infrastructure;
using SaveTheDate.Infrastructure.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "SaveTheDate API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT token: Bearer {token}"
    });
    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        }
    });
});

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()!;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await SeedAdminUser(app);

app.Run();

static async Task SeedAdminUser(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var userRepo = scope.ServiceProvider.GetRequiredService<SaveTheDate.Domain.Abstractions.IUserRepository>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<SaveTheDate.Domain.Abstractions.IPasswordHasher>();
    var unitOfWork = scope.ServiceProvider.GetRequiredService<SaveTheDate.Domain.Abstractions.IUnitOfWork>();

    var config = app.Configuration.GetSection("Seed:Admin");
    if (!config.Exists()) return;

    var email = config["Email"];
    var password = config["Password"];
    var role = config["Role"] ?? "Admin";

    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return;

    var existing = await userRepo.GetByEmailAsync(email);
    if (existing is not null) return;

    var user = new SaveTheDate.Domain.Users.User(email, passwordHasher.Hash(password), role);
    await userRepo.AddAsync(user);
    await unitOfWork.SaveChangesAsync();

    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Seeded admin user: {Email} / {Password}", email, password);
}
