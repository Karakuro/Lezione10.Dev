using Lezione10.Dev.Data;
using Lezione10.Dev.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
string? connStr = builder.Configuration.GetConnectionString("Default");
builder.Services.AddSqlServer<SchoolDbContext>(connStr);
builder.Services.AddSingleton<Mapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
