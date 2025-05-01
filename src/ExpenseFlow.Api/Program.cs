using ExpenseFlow.Infrastructure.DbContext;
using ExpenseFlow.Application.Mapping;       // your AutoMapper Profile sits here
using ExpenseFlow.Application.Validation;    // your ExpenseValidator
using ExpenseFlow.Application.Cqrs.Commands; // your CreateExpenseCommand
using FluentValidation;                      // from FluentValidation
using FluentValidation.AspNetCore;           // for AddFluentValidationAutoValidation
using AutoMapper;                            // from AutoMapper.Extensions.Microsoft.DependencyInjection
using MediatR;                               
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//FluentValidation
builder.Services
    .AddFluentValidationAutoValidation()      // replaces the old AddFluentValidation(...)
    .AddFluentValidationClientsideAdapters();
builder.Services
    .AddValidatorsFromAssemblyContaining<ExpenseValidator>();


// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MapperProfile>();
});


// 4) EF Core + Npgsql
builder.Services.AddDbContext<ExpenseFlowDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 5) MediatR
// — Install MediatR.Extensions.Microsoft.DependencyInjection
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateExpenseCommand>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
