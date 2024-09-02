using CocktailApp.Interfaces;
using CocktailApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CocktailApp.Services;

public class CocktailService : ICocktailService
{
    private readonly HttpClient _httpClient;
    private readonly ITranslationService _translationService;

    public CocktailService(IHttpClientFactory httpClientFactory, ITranslationService translationService)
    {
        _httpClient = httpClientFactory.CreateClient("cocktailClient");
        _translationService = translationService;
    }

    public async Task<Cocktail?> GetRandomCocktail()
    {
        try
        {
            var response = await _httpClient.GetAsync("random.php");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var cocktailResponse = JsonConvert.DeserializeObject<CocktailResponse>(content);

            if (cocktailResponse?.Drinks == null || !cocktailResponse.Drinks.Any())
                return null;

            var cocktail = cocktailResponse.Drinks[0];
            var jsonObject = JObject.Parse(content);
            var drinkObject = jsonObject["drinks"]?[0];

            var translationTasks = new List<Task>
        {
            Task.Run(async () => cocktail.Instructions = await _translationService.TranslateToSith(cocktail.Instructions ?? "")),
            Task.Run(async () => cocktail.Ingredients = await ExtractAndTranslateIngredients(drinkObject))
        };

            await Task.WhenAll(translationTasks);

            return cocktail;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"HTTP request error: {e.Message}");
            return null;
        }
        catch (JsonException e)
        {
            Console.WriteLine($"JSON parsing error: {e.Message}");
            return null;
        }
    }

    private async Task<List<Ingredient>> ExtractAndTranslateIngredients(JToken? drinkObject)
    {
        var ingredients = new List<Ingredient>();
        var measures = new List<string>();
        if (drinkObject == null) return ingredients;

        var ingredientNames = new List<string>();

        for (int i = 1; i <= 15; i++)
        {
            var ingredientName = drinkObject[$"strIngredient{i}"]?.ToString();
            if (string.IsNullOrEmpty(ingredientName))
                break;

            ingredientNames.Add(ingredientName);
            measures.Add(drinkObject[$"strMeasure{i}"]?.ToString() ?? "");
        }
        var translatedNames = await _translationService.TranslateToSith(string.Join(", ", ingredientNames));

        var translatedNamesList = translatedNames.Split(", ");
        for (int i = 0; i < translatedNamesList.Length; i++)
        {
            ingredients.Add(new Ingredient
            {
                Name = translatedNamesList[i],
                Measure = measures[i],
                Image = $"https://www.thecocktaildb.com/images/ingredients/{Uri.EscapeDataString(ingredientNames[i])}.png"
            });
        }

        return ingredients;
    }
}