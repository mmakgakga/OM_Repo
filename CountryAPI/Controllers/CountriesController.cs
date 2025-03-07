using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static CountryAPI.Entities.Entities;
using static CountryAPI.Reporsitories.Repositories;

namespace CountryAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [EnableCors("AllowSpecificOrigin")]
  public class CountriesController : ControllerBase
  {
    private readonly ICountryRepository _countryRepository;

    public CountriesController(ICountryRepository countryRepository)
    {
      _countryRepository = countryRepository;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> Get()
    {
      var countries = await _countryRepository.GetAllCountriesAsync();
      if (countries == null)
      {
        return NotFound(new { message = "Countries not found" });
      }
      return countries.ToList();
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<Country>> Get(string name)
    {
      var country = await _countryRepository.GetCountryByNameAsync(name);
      if (country == null)
      {
        return NotFound(new { message = "Country not found" });
      }
      return country;
    }
  }
}