using Newtonsoft.Json;
namespace CocktailApp.Models;

public class Cocktail
{
    public string Language { get; set; } = "Sith";

    [JsonProperty("strDrink")]
    public string? Title { get; set; }

    [JsonProperty("StrInstructions")]
    public string? Instructions { get; set; }

    [JsonProperty("StrDrinkThumb")]
    public string? Image { get; set; }

    public List<Ingredient>? Ingredients { get; set; }
}

public class Ingredient
{
    public string? Name { get; set; }
    public string? Measure { get; set; }
    public string? Image { get; set; }
}
public class CocktailResponse
{
    public List<Cocktail> Drinks { get; set; } = new List<Cocktail>();
}