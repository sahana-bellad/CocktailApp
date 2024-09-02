using CocktailApp.Interfaces;
using CocktailApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var cocktailClientSettings = builder.Configuration.GetSection("CocktailClientSettings");
var funTranslationsSettings = builder.Configuration.GetSection("FunTranslationsSettings");

var cocktailClientBaseAddress = cocktailClientSettings["BaseAddress"] ?? "https://www.thecocktaildb.com/api/json/v1/1/";
var funTranslationsBaseAddress = funTranslationsSettings["BaseAddress"] ?? "https://api.funtranslations.com/";

builder.Services.AddHttpClient("cocktailClient", client =>
{
    client.BaseAddress = new Uri(cocktailClientBaseAddress);
});

builder.Services.AddHttpClient("translateClient", client =>
{
    client.BaseAddress = new Uri(funTranslationsBaseAddress);
});

builder.Services.AddScoped<ICocktailService, CocktailService>();
builder.Services.AddScoped<ITranslationService, TranslationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();