using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using WebAPI_Projeto02.Context;
using WebAPI_Projeto02.Extensions;
using WebAPI_Projeto02.Filters;
using WebAPI_Projeto02.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => {
    options.Filters.Add(typeof(ApiExceptionFilter));
}).AddJsonOptions(options => {
    options.JsonSerializerOptions.
        ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseMySql(mySqlConnection, 
        ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddScoped<ApiLoggingFilter> ();

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfigration
{
    LogLevel = LogLevel.Information
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.SettingsExceptionHandler();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
