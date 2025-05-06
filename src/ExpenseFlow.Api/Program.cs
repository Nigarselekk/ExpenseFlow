using ExpenseFlow.Infrastructure.DbContext;
using ExpenseFlow.Infrastructure.Identity;
using ExpenseFlow.Application.Mapping;
using ExpenseFlow.Application.Validation;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ExpenseFlow.Application.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ExpenseValidator>();
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);


builder.Services.AddDbContext<ExpenseFlowDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(opts =>
    {
        opts.User.RequireUniqueEmail = true;

    })
    .AddEntityFrameworkStores<ExpenseFlowDbContext>()
    .AddDefaultTokenProviders();

//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var jwtSection = builder.Configuration.GetSection("JwtSettings");

var keyBytes = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

builder.Services
  .AddAuthentication(options =>
  {

      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
  })
  .AddJwtBearer(options =>
  {
      options.RequireHttpsMetadata = false;
      options.SaveToken = true;
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
          ValidateIssuer = true,
          ValidIssuer = jwtSection["Issuer"],
          ValidateAudience = true,
          ValidAudience = jwtSection["Audience"],
          ValidateLifetime = true,
          ClockSkew = TimeSpan.Zero,
          NameClaimType = ClaimTypes.NameIdentifier,
          RoleClaimType = ClaimTypes.Role
      };
  });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateExpenseCommand>());


builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ExpenseFlowDbContext>();
    db.Database.Migrate();
    await IdentityDataSeeder.SeedAsync(scope.ServiceProvider);
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    await IdentityDataSeeder.SeedAsync(scope.ServiceProvider);
}

app.MapControllers();
app.Run();
