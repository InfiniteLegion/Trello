using Microsoft.EntityFrameworkCore;
using Trello;
using Trello.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: "_myAllowSpecificOrigins",
					  builder =>
					  {
						  builder.WithOrigins("http://127.0.0.1:5500");
					  });
});

string connectionTest = "Host=localhost;Port=5432;Database=chelo_db;Username=postgres;Password=root";
builder.Services.AddDbContext<CheloDbContext>(o => o.UseNpgsql(connectionTest));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("_myAllowSpecificOrigins");

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
