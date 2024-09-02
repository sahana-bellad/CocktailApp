using CocktailApp.Models;

namespace CocktailApp.Interfaces;

public interface ICocktailService
{
    Task<Cocktail?> GetRandomCocktail();
}