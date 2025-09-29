using ServiceContracts.DTOs;

namespace ServiceContracts;

/// <summary>
/// Service contract for Country operations
/// </summary>
public interface ICountryService
{
    /// <summary>
    /// Adds a new country
    /// </summary>
    /// <param name="CountryAddRequest">The country add request.</param>
    /// <returns> Returns the added country object after added it.</returns>
    public Task<CountryResponse> AddCountry(CountryAddRequest request);

    /// <summary>
    /// Gets all countries
    /// </summary>
    /// <returns> A list of all countries.</returns>
    List<CountryResponse> GetAllCountries();

    /// <summary>
    /// Gets a country by its unique identifier
    /// </summary>
    /// <param name="countryId">The unique identifier of the country.</param>
    /// <returns> Returns the country object if found, otherwise null.</returns>
    CountryResponse? GetCountryByCountryId(Guid? countryId);
}
