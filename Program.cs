//using JamilNativeAPI.DataContext;
using JamilNativeAPI.Context;
using JamilNativeAPI.Respositories;
using JamilNativeAPI.Respositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITenantManager, TenantRepository>();
builder.Services.AddScoped<IWaterManager, WaterRepository>();

//Load configuration
var configuration = builder.Configuration;

builder.Services.AddControllers();

//configuring CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:7225")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//configuring the entity framework Context type (registering the dependency injection)
var connectionString = builder.Configuration.GetConnectionString("DefaultDb");

builder.Services.AddDbContext<KakaireEstatesContext>(options =>
options.UseMySql(connectionString, 
new MySqlServerVersion(new Version(8, 0, 40))
//Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql")

));


var app = builder.Build();

//Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}


app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
