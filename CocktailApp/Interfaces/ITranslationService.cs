namespace CocktailApp.Interfaces;

public interface ITranslationService
{
    Task<string> TranslateToSith(string text);
}