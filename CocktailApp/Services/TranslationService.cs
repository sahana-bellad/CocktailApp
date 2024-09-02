using CocktailApp.Interfaces;
using Newtonsoft.Json.Linq;

namespace CocktailApp.Services;

public class TranslationService : ITranslationService
{
    private readonly HttpClient _httpClient;
    private readonly Random _random = new Random();
    private const string _letters = "abcdefghijklmnopqrstuvwxyz";

    public TranslationService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("translateClient");
    }

    public async Task<string> TranslateToSith(string text)
    {
        try
        {
            var requestUri = $"translate/sith?text={Uri.EscapeDataString(text)}";
            var response = await _httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                return GenerateRandomTranslation(text);
            }

            var json = await response.Content.ReadAsStringAsync();
            var jToken = JToken.Parse(json);
            return jToken["contents"]?["translated"]?.Value<string>() ?? GenerateRandomTranslation(text);
        }
        catch
        {
            return GenerateRandomTranslation(text);
        }
    }

    private string GenerateRandomTranslation(string text)
    {
        string[] words = text.Split(' ');
        string[] randomWords = new string[words.Length];

        for (int i = 0; i < words.Length; i++)
        {
            randomWords[i] = GenerateRandomWord();
        }

        return string.Join(" ", randomWords);
    }

    private string GenerateRandomWord()
    {
        int length = _random.Next(3, 16);
        char[] word = new char[length];
        for (int i = 0; i < length; i++)
        {
            word[i] = _letters[_random.Next(_letters.Length)];
        }
        return new string(word);
    }
}