# Fantasy Cocktail API

A .NET Web API that fetches random cocktail recipes, translates instructions and ingredient's name to Sith language, and adds ingredient images.

## Features:
- Fetches random cocktails from TheCocktailDB API
- Translates to Sith using Fun Translations API
- Generates random words on translation failure
- Retrieves ingredient images
- Returns detailed JSON response

## Tech:
- ASP.NET Core Web API
- C# 8.0
- HttpClient, Newtonsoft.Json
- Dependency Injection
- Unit testing (NUnit, Moq)

Endpoint: GET /api/cocktail/random-cocktail?language=sith