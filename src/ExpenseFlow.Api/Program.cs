using ExpenseFlow.Infrastructure.DbContext;
using ExpenseFlow.Infrastructure.Identity;
using ExpenseFlow.Application.Mapping;
using ExpenseFlow.Application.Validation;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Infrastructure.Identity;
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

var builder = WebApplication.CreateBuilder(args);

// 1) Controllers + FluentValidation + AutoMapper
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ExpenseValidator>();
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

// 2) EF Core + Identity
builder.Services.AddDbContext<ExpenseFlowDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(opts =>
    {
        opts.User.RequireUniqueEmail = true;
        // diğer Identity seçenekleri…
    })
    .AddEntityFrameworkStores<ExpenseFlowDbContext>()
    .AddDefaultTokenProviders();

// 3) JWT Authentication
var jwtSection = builder.Configuration.GetSection("JwtSettings");

var keyBytes   = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken            = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey         = new SymmetricSecurityKey(keyBytes),

        ValidateIssuer           = true,
        ValidIssuer              = jwtSection["Issuer"],

        ValidateAudience         = true,
        ValidAudience            = jwtSection["Audience"],

        ValidateLifetime         = true,
        ClockSkew                = TimeSpan.Zero
    };
});

// 4) Authorization & Swagger
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In          = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name        = "Authorization",
        Type        = SecuritySchemeType.ApiKey
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

// 5) MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateExpenseCommand>());

// 6) Kendi servislerimiz
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

var app = builder.Build();

// Swagger/UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Pipeline
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    await IdentityDataSeeder.SeedAsync(scope.ServiceProvider);
}

app.MapControllers();
app.Run();
