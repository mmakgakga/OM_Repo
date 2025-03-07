using Newtonsoft.Json;
using static CountryAPI.Entities.Entities;

namespace CountryAPI.Reporsitories
{
  public class Repositories
  {
    public interface ICountryRepository
    {
      Task<IEnumerable<Country>?> GetAllCountriesAsync();
      Task<Country?> GetCountryByNameAsync(string name);
    }

    public class CountryService : ICountryRepository
    {
      private readonly HttpClient _httpClient;

      public CountryService(HttpClient httpClient)
      {
        _httpClient = httpClient;
      }

      public async Task<IEnumerable<Country>?> GetAllCountriesAsync()
      {
        var response = await _httpClient.GetStringAsync("https://restcountries.com/v3.1/all");
        var countries = JsonConvert.DeserializeObject<IEnumerable<Country>>(response);
        return countries == null ? null : countries;
      }

      public async Task<Country?> GetCountryByNameAsync(string name)
      {
        var response = await _httpClient.GetStringAsync($"https://restcountries.com/v3.1/name/{name}");
        var countryList = JsonConvert.DeserializeObject<IEnumerable<Country>>(response);
        return countryList?.FirstOrDefault();
      }
    }
  }
}