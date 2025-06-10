using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.Middleware;
using TaskTracker.Services;
using TaskTracker.Common;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));


builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=tasktracker.db"));

builder.Services.AddScoped<TaskService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Task Tracker API",
        Version = "v1",
        Description = "API per la gestione di task"
    });
});

// Recupera la chiave
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

if(jwtSettings != null)
{ 
    var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey); 
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<LoggingRequestMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
