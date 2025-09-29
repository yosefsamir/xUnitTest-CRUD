using Xunit;
using ServiceContracts;
using ServiceContracts.DTOs;
using ServiceContracts.Enums;

using Services;
using Entities;
using System;
namespace CrudTests;

public class PersonServiceTest
{
    private readonly IPersonService _personService;

    public PersonServiceTest()
    {
        _personService = new PersonService();
    }

    #region AddPerson method tests
    // when we supply null value as personAddRequest it should throw NullReferenceException
    [Fact]
    public void AddPerson_NullPerson()
    {
        // Arrange
        PersonAddRequest? personAddRequest = null;

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => _personService.AddPerson(personAddRequest));
    }

    // when we supply null value as PersonName in personAddRequest it should throw ArgumentException
    [Fact]
    public void AddPerson_NullPersonName()
    {
        // Arrange
        PersonAddRequest? personAddRequest = new PersonAddRequest()
        {
            PersonName = null
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _personService.AddPerson(personAddRequest));
    }

    // when we supply proper PersonAddRequest it should Add The Person in the list and return PersonResponse object
    [Fact]
    public void AddPerson_ValidPerson()
    {
        // Arrange
        PersonAddRequest? personAddRequest = new PersonAddRequest()
        {
            PersonName = "John Doe",
            Email = "JOE@EXAMPLE.COM",
            DateOfBirth = new DateTime(2003, 11, 18),
            Gender = GenderOptions.Male,
            CountryId = Guid.NewGuid(),
            Address = "123, Main Street, City - 12345",
            ReceiveNewsLetters = true
        };

        // Act
        PersonResponse? personResponse = _personService.AddPerson(personAddRequest);
        List<PersonResponse> allPersons = _personService.GetAllPersons();

        // Assert
        Assert.True(personResponse.PersonId != Guid.Empty);
        Assert.Contains(personResponse, allPersons);
        Assert.NotNull(personResponse);
    }
    #endregion
}

