using Microsoft.Extensions.Configuration;
/*

    the url of the api is -> https://languageenginequery-g8hwe9a0fta2b0b7.polandcentral-01.azurewebsites.net/api/proximity/query

*/

using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Language Proximity API", Version = "v1" });
});  // For API documentation/testing

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register ExcelService with file path from config
builder.Services.AddSingleton<ExcelService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var filePath = config["ExcelFilePath"];
    return new ExcelService(filePath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Language Proximity API v1"));
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");  // Enable CORS
app.UseAuthorization();
app.MapControllers();

app.Run();