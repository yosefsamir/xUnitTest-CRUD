using Entities;

namespace ServiceContracts.DTOs;

/// <summary>
/// Response DTO for Country
/// </summary>
public class CountryResponse
{
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj.GetType() != typeof(CountryResponse))
        {
            return false;
        }

        CountryResponse comparingObject = (CountryResponse)obj;
        return this.CountryId == comparingObject.CountryId && this.CountryName == comparingObject.CountryName;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public static class CountryExtensions
{
    public static CountryResponse ToCountryResponse(this Country country)
    {
        return new CountryResponse()
        {
            CountryId = country.CountryId,
            CountryName = country.CountryName
        };
    }
}
