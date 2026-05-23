using Microsoft.EntityFrameworkCore;
using PhotoMarket.BL.Interfaces;
using PhotoMarket.BL.Services;
using PhotoMarket.DAL.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PhotoMarketContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register BL services
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IPhotographerService, PhotographerService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IDownloadService, DownloadService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
