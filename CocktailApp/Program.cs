using CocktailApp.Interfaces;
using CocktailApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("cocktailClient", client =>
{
    client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/v1/1/");
});

builder.Services.AddHttpClient("translateClient", client =>
{
    client.BaseAddress = new Uri("https://api.funtranslations.com/");
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