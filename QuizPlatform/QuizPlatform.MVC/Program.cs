using Microsoft.EntityFrameworkCore;
using QuizPlatform.Repository.Database;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(name: "QuizPlatform",
configureClient: options =>
{
    options.BaseAddress = new Uri("https://localhost:7071");
    options.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue(
    mediaType: "application/json", quality: 1.0));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tests}/{action=index}/{id?}")
    .WithStaticAssets();


app.Run();
