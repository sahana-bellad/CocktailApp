using CocktailApp.Interfaces;
using CocktailApp.Models;
using CocktailApp.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;

namespace CocktailApp.Tests;

[TestFixture]
public class CocktailServiceTests
{
    private Mock<IHttpClientFactory> _mockHttpClientFactory;
    private Mock<ITranslationService> _mockTranslationService;
    private CocktailService _service;

    [SetUp]
    public void Setup()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockTranslationService = new Mock<ITranslationService>();
        _service = new CocktailService(_mockHttpClientFactory.Object, _mockTranslationService.Object);
    }

    [Test]
    public async Task GetRandomCocktail_ReturnsCocktail_WhenServiceReturnsOne()
    {
        var expectedCocktail = new Cocktail { Title = "Test Cocktail", Language = "Sith" };
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(new CocktailResponse
            {
                Drinks = new List<Cocktail> { expectedCocktail }
            }))
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        var httpClient = new HttpClient(handlerMock.Object);
        _mockHttpClientFactory.Setup(factory => factory.CreateClient("cocktailClient"))
            .Returns(httpClient);

        _mockTranslationService
            .Setup(service => service.TranslateToSith(It.IsAny<string>()))
            .ReturnsAsync((string s) => s);

        var result = await _service.GetRandomCocktail();

        Assert.IsNotNull(result);
        Assert.That(result.Title, Is.EqualTo("Test Cocktail"));
        Assert.That(result.Language, Is.EqualTo("Sith"));
    }


    [Test]
    public async Task GetRandomCocktail_ReturnsNull_WhenServiceReturnsNull()
    {
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(new CocktailResponse
            {
                Drinks = new List<Cocktail>()
            }))
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        var httpClient = new HttpClient(handlerMock.Object);
        _mockHttpClientFactory.Setup(factory => factory.CreateClient("cocktailClient"))
            .Returns(httpClient);

        var result = await _service.GetRandomCocktail();

        Assert.IsNull(result);
    }
}