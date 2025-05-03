using ExpenseFlow.Infrastructure.DbContext;
using ExpenseFlow.Application.Mapping;       
using ExpenseFlow.Application.Validation;     
using ExpenseFlow.Application.Cqrs.Commands;  
using FluentValidation;                       
using FluentValidation.AspNetCore;           
using AutoMapper;                            
using MediatR;                               
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//FluentValidation
builder.Services
    .AddFluentValidationAutoValidation()       
    .AddFluentValidationClientsideAdapters();
builder.Services
    .AddValidatorsFromAssemblyContaining<ExpenseValidator>();


// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MapperProfile>();
});


builder.Services.AddDbContext<ExpenseFlowDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

// 5) MediatR
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
