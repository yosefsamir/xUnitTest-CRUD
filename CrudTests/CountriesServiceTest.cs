using System.Threading.Tasks;
using ServiceContracts;
using ServiceContracts.DTOs;
using Services;
using Xunit;

namespace CrudTests;

public class CountriesServiceTest
{
    private readonly ICountryService _countryService;

    public CountriesServiceTest()
    {
        _countryService = new CountriesService();
    }

    #region AddCountry
    // when CountryAddRequest is null, should throw ArgumentNullException
    [Fact]
    public async Task AddCountry_NullCountryAddRequest()
    {
        // Arrange
        CountryAddRequest? countryAddRequest = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await _countryService.AddCountry(countryAddRequest!);
        });
    }

    // when the country name is null, should throw ArgumentException
    [Fact]
    public async Task AddCountry_NullCountryName()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new CountryAddRequest()
        {
            CountryName = null
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _countryService.AddCountry(countryAddRequest);
        });
    }

    // when the countery name is duplicate, should throw ArgumentException
    [Fact]
    public async Task AddCountry_DuplicateCountryName()
    {
        // Arrange
        CountryAddRequest countryAddRequest1 = new CountryAddRequest()
        {
            CountryName = "Egypt"
        };

        CountryAddRequest countryAddRequest2 = new CountryAddRequest()
        {
            CountryName = "Egypt"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _countryService.AddCountry(countryAddRequest1);
            await _countryService.AddCountry(countryAddRequest2);
        });
    }

    // when you supply valid country name, should return the added country object
    [Fact]
    public async Task AddCountry_ValidCountry()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new CountryAddRequest()
        {
            CountryName = "Egypt"
        };

        // Act
        CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);
        List<CountryResponse> countries_from_getAllCountries = _countryService.GetAllCountries();

        // Assert
        Assert.NotNull(countryResponse);
        Assert.Equal(countryAddRequest.CountryName, countryResponse.CountryName);
        Assert.True(countryResponse.CountryId != Guid.Empty);
        Assert.Contains(countryResponse, countries_from_getAllCountries);
    }
    #endregion

    #region GetAllCountries
    [Fact]
    // the list of countries should be empty by default (before adding any country)
    public void GetAllCountries_EmptyList()
    {
        // Act
        var actual_country_response_list = _countryService.GetAllCountries();

        //Assert
        Assert.Empty(actual_country_response_list);
    }

    [Fact]
    public async Task GetAllCountries_AddFewCountries()
    {
        // Arange
        List<CountryAddRequest> country_request_list = new
        List<CountryAddRequest>()
        {
            new CountryAddRequest {CountryName = "US"} ,
            new CountryAddRequest {CountryName = "EG"}
        };

        // Act
        var country_list_from_add_country = new List<CountryResponse>();

        foreach (var country_request in country_request_list)
        {
            country_list_from_add_country.Add(await _countryService.AddCountry(country_request));
        }

        //Arrange
        var actualCountryResponseList = _countryService.GetAllCountries();

        foreach (var expected_country in country_list_from_add_country)
        {
            Assert.Contains(expected_country, actualCountryResponseList);
        }
    }

    #endregion

    #region GetCountryById
    // when the countryId is null, should return null
    [Fact]
    public void GetCountryByCountryId_EmptyGuid()
    {
        // Arrange
        Guid? countryId = null;

        // Act
        var country_response = _countryService.GetCountryByCountryId(countryId);

        // Assert
        Assert.Null(country_response);
    }

    // when the countryId is not found, should return null
    [Fact]
    public void GetCountryByCountryId_CountryIdNotFound()
    {
        // Arrange
        Guid countryId = Guid.NewGuid();

        // Act
        var country_response = _countryService.GetCountryByCountryId(countryId);

        // Assert
        Assert.Null(country_response);
    }

    // when the countryId is found, should return the country object as countryResponse object
    [Fact]
    public async Task GetCountryByCountryId_CountryIdFound()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new CountryAddRequest()
        {
            CountryName = "Egypt"
        };

        // Act
        var country_response_Add = await _countryService.AddCountry(countryAddRequest);
        var country_response_get = _countryService.GetCountryByCountryId(country_response_Add.CountryId);

        // Assert
        Assert.NotNull(country_response_get);
        Assert.Equal(country_response_Add, country_response_get);
    }
    
    #endregion
}


