using System.Reflection;
using FacilityApi.Data;
using Microsoft.AspNetCore.Builder;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("facilities", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Facilities in SF API",
        Version = "V3",
        Description = "Facilities in SF Web API using Asp.net Core"
    });
    options.EnableAnnotations();
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
// Add services to the container.
builder.Services.Configure<FacilityDatabaseSettings>(
    builder.Configuration.GetSection("FacilityDatabase"));

builder.Services.AddSingleton<FacilityService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/facility/swagger.jso", "Facilities in SF API"));
}

app.UseCors(config =>
{
    config
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
    });
app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();

