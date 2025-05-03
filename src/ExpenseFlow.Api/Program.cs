using ExpenseFlow.Infrastructure.DbContext;
using ExpenseFlow.Application.Mapping;
using ExpenseFlow.Application.Validation;
using ExpenseFlow.Application.Cqrs.Commands;
using FluentValidation.AspNetCore;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddControllers();
    
// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ExpenseValidator>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

// EF Core / PostgreSQL
builder.Services.AddDbContext<ExpenseFlowDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

// MediatR  
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateExpenseCommand>());

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
