using ExpenseFlow.Infrastructure.DbContext;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ExpenseFlow.Application.Validation;   

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers()
.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ExpenseValidator>());


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ExpenseFlowDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Run();
