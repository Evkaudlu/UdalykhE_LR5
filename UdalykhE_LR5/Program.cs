using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UdalykhE_LR5.Models;

var builder = WebApplication.CreateBuilder(args);

var databaseSettings = builder.Configuration
    .GetSection("DatabaseSettings")
    .Get<DatabaseSettings>() ?? new DatabaseSettings();

var validationResults = new List<ValidationResult>();

var validationContext = new ValidationContext(databaseSettings);

bool isValid = Validator.TryValidateObject(
    databaseSettings,
    validationContext,
    validationResults,
    true);

if (!isValid)
{
    Console.WriteLine("Database configuration validation failed:");

    foreach (var error in validationResults)
    {
        Console.WriteLine(error.ErrorMessage);
    }
    Console.ReadLine();

    return;
}

builder.Services.AddControllers();

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseInMemoryDatabase(databaseSettings.DatabaseName));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();