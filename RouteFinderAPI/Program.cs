using RouteFinderAPI.Providers.Services;
using RouteFinderAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Регистрация кэширования
builder.Services.AddMemoryCache(); 

// Настройка провайдеров
builder.Services.ConfigureProviderServices();

// Регистрация агрегированного сервиса с провайдерами
builder.Services.AddScoped<RouteFinderAPI.Services.ISearchService, SearchService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
