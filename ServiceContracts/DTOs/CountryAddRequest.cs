using Entities;

namespace ServiceContracts.DTOs;

/// <summary>
/// Request DTO for adding a country
/// </summary>
public class CountryAddRequest
{
    public string? CountryName { get; set; }

    public Country ToCountry()
    {
        return new Country()
        {
            CountryName = this.CountryName
        };
    }
}
// request >> Entity >> response
