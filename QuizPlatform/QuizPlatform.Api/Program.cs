using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizPlatform.Api.Middlewares;
using QuizPlatform.Identity.Models;
using QuizPlatform.Repository.Database;
using QuizPlatform.Repository.Implementations;
using QuizPlatform.Service.Services.Abstractions;
using QuizPlatform.Services.Abstractions;
using QuizPlatform.Services.Implementations;
using Serilog;
using System.Text.Json.Serialization;
using QuizPlatform.MessageSender.Extensions;
using QuizPlatform.Service.Services.Implementations;
using QuizPlatform.Domain.Abstractions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddMailSender();

Log.Logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();



builder.Host.UseSerilog(); // Use Serilog as the logging provider

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
  .AddEntityFrameworkStores<AppDbContext>();

builder.Host.UseSerilog(); // Use Serilog as the logging provider
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IQuizService, QuizService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseErrorHandlingMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();