using Microsoft.EntityFrameworkCore;
using task2.Data;

var builder = WebApplication.CreateBuilder(args);

// Читаємо рядок підключення з appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Реєстрація DbContext
builder.Services.AddDbContext<SplitwiseDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
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