using Entities;
using ServiceContracts;
using ServiceContracts.DTOs;
namespace Services;

public class CountriesService : ICountryService
{
    private readonly List<Country> _countries;
    public CountriesService()
    {
        _countries = new List<Country>();
    }
    public Task<CountryResponse> AddCountry(CountryAddRequest? request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "CountryAddRequest cannot be null");
        }

        if (string.IsNullOrEmpty(request.CountryName))
        {
            throw new ArgumentException("Country name cannot be null or empty", nameof(request.CountryName));
        }

        if(_countries.Any(c => c.CountryName != null && c.CountryName.Equals(request.CountryName, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException($"Country with name {request.CountryName} already exists.", nameof(request.CountryName));
        }


        Country country = request.ToCountry();
        country.CountryId = Guid.NewGuid();
        _countries.Add(country);

        return Task.FromResult(country.ToCountryResponse());
    }

    public List<CountryResponse> GetAllCountries()
    {
        return _countries.Select(country => country.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryByCountryId(Guid? countryId)
    {
        if (countryId == null || countryId == Guid.Empty)
        {
            return null;
        }
        Country? countryMatchingCountryId = _countries.FirstOrDefault(c => c.CountryId == countryId);
        return countryMatchingCountryId?.ToCountryResponse();
    }
}
