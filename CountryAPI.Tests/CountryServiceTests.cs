using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static CountryAPI.Entities.Entities;
using static CountryAPI.Reporsitories.Repositories;

namespace CountryAPI.Tests
{
  [TestClass]
  public class CountryServiceTests
  {
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private HttpClient _httpClient;
    private ICountryRepository _countryService;

    [TestInitialize]
    public void Setup()
    {
      _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
      _httpClient = _httpMessageHandlerMock.CreateClient();
      _countryService = new CountryService(_httpClient);
    }

    [TestMethod]
    public async Task GetAllCountriesAsync_ReturnsAllCountries()
    {
      // Arrange
      var name = new Name() { Common = "South Africa" };
      var flags = new Flags() { Png = "????" };
      var mockResponse = JsonConvert.SerializeObject(new List<Country>
            {
                new Country { Name = name, Flags = flags }
            });

      _httpMessageHandlerMock.SetupRequest(HttpMethod.Get, "https://restcountries.com/v3.1/all")
          .ReturnsResponse(mockResponse, "application/json");

      // Act
      var result = await _countryService.GetAllCountriesAsync();

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(250, result.Count());
      Assert.IsTrue(result.Any(c => c.Name.Common == "South Georgia"));
      Assert.IsTrue(result.Any(c => c.Name.Common == "Anguilla"));
    }

    [TestMethod]
    public async Task GetCountryByNameAsync_ReturnsCountryDetails()
    {
      // Arrange
      var countryName = new Name() { Common = "South Africa" };
      var capital = new List<string> { "Pretoria" };
      var flag = new Flags() { Png = "https://flagcdn.com/w320/za.png" };

      var mockResponse = JsonConvert.SerializeObject(new List<Country>
            {
                new Country { Name = countryName, Population = 59308690, Capital = capital, Flags = flag }
            });

      _httpMessageHandlerMock.SetupRequest(HttpMethod.Get, $"https://restcountries.com/v3.1/name/{countryName}")
          .ReturnsResponse(mockResponse, "application/json");

      // Act
      var result = await _countryService.GetCountryByNameAsync(countryName.Common);

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("South Africa", result.Name.Common);
      Assert.AreEqual(59308690, result.Population);
      Assert.AreEqual("Pretoria", result.Capital[0]);
      Assert.AreEqual("????", result.Flags.Png);
    }
  }
}
