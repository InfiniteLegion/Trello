using Microsoft.EntityFrameworkCore;
using Trello;
using Trello.Classes.Mapper;
using Trello.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_myAllowSpecificOrigins",
                      builder =>
                      {
                          builder.WithOrigins("http://127.0.0.1:5500")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
});

string connectionTest = "Host=localhost;Port=5432;Database=chelo_db;Username=postgres;Password=root";
builder.Services.AddDbContext<CheloDbContext>(o => o.UseNpgsql(connectionTest));

builder.Services.AddScoped<BoardMapper>();
builder.Services.AddScoped<CardMapper>();
builder.Services.AddScoped<CommentMapper>();
builder.Services.AddScoped<StatusColumnMapper>();
builder.Services.AddScoped<TagMapper>();
builder.Services.AddScoped<TaskMapper>();
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<TeamNotificationMapper>();
builder.Services.AddScoped<FriendMapper>();
builder.Services.AddScoped<ConfigurationMapper>();

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
