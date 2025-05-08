using Lezione10.Dev.Data;
using Lezione10.Dev.DTO;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//string? connStr = builder.Configuration.GetConnectionString("Default");


string? connStr = builder.Configuration.GetConnectionString("Azure");
builder.Services.AddSqlServer<SchoolDbContext>(connStr);



builder.Services.AddSingleton<Mapper>();
builder.Services.AddCors(options => options.AddDefaultPolicy(config =>
{
    config.WithOrigins("http://127.0.0.1:5500", "sites.azure.com").AllowAnyHeader().AllowAnyMethod();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using(var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
    ctx.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors(); //OBBLIGATORIAMENTE QUI!!!!!!

app.UseAuthorization();

app.MapControllers();

app.Run();
