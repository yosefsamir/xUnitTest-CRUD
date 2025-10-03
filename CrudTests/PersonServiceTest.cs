using Xunit;
using ServiceContracts;
using ServiceContracts.DTOs;
using ServiceContracts.Enums;

using Services;
using Entities;
using System;
using System.Threading.Tasks;
using Xunit.Abstractions;
namespace CrudTests;

public class PersonServiceTest
{
    private readonly IPersonService _personService;
    private readonly ICountryService _countryService;
    private readonly ITestOutputHelper _output;

    public PersonServiceTest(ITestOutputHelper output)
    {
        _personService = new PersonService();
        _countryService = new CountriesService();
        _output = output;
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

    #region GetPersonByPersonId method tests
    // if we supply personId is null  , it should return null as PersonResponse
    [Fact]
    public void GetPersonByPersonId_NullPersonId()
    {
        // Arrange
        Guid personId = Guid.Empty;

        // Act
        PersonResponse? personResponse = _personService.GetPersonByPersonId(personId);

        // Assert
        Assert.Null(personResponse);
    }

    // if we supply invalid personId  , it should return null as PersonResponse
    [Fact]
    public void GetPersonByPersonId_InvalidPersonId()
    {
        // Arrange
        Guid personId = Guid.NewGuid();

        // Act
        PersonResponse? personResponse = _personService.GetPersonByPersonId(personId);

        // Assert
        Assert.Null(personResponse);
    }

    // if we supply valid personId  , it should return PersonResponse object
    [Fact]
    public async Task GetPersonByPersonId_ValidPersonId()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new CountryAddRequest()
        {
            CountryName = "USA"
        };
        CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);

        PersonAddRequest personAddRequest = new()
        {
            PersonName = "John Doe",
            Email = "JOHN@EXAMPLE.COM",
            DateOfBirth = new DateTime(2003, 11, 18),
            Gender = GenderOptions.Male,
            CountryId = countryResponse.CountryId,
            Address = "123 Main St, Anytown, USA",
            ReceiveNewsLetters = true
        };
        PersonResponse personResponse = _personService.AddPerson(personAddRequest);

        // Act
        PersonResponse? retrievedPerson = _personService.GetPersonByPersonId(personResponse.PersonId);

        // Assert
        Assert.NotNull(retrievedPerson);
        Assert.Equal(personResponse.PersonId, retrievedPerson.PersonId);
    }
    #endregion

    #region GetAllPerson
    // when there are no persons in the list, it should return empty list of PersonResponse
    [Fact]
    public void GetAllPersons_EmptyList()
    {
        // Act
        List<PersonResponse> allPersons = _personService.GetAllPersons();

        // Assert
        Assert.Empty(allPersons);
    }

    // first we add few persons using AddPerson method, then we call GetAllPersons method, it should return list of two PersonResponse objects
    [Fact]
    public async Task GetAllPersons_AddFewPersons()
    {
        // Arrange
        CountryAddRequest countryAddRequest1 = new CountryAddRequest()
        {
            CountryName = "USA"
        };
        CountryAddRequest countryAddRequest2 = new CountryAddRequest()
        {
            CountryName = "Canada"
        };
        CountryResponse countryResponse1 = await _countryService.AddCountry(countryAddRequest1);
        CountryResponse countryResponse2 = await _countryService.AddCountry(countryAddRequest2);
        PersonAddRequest personAddRequest1 = new PersonAddRequest()
        {
            PersonName = "John Doe",
            Email = "JOHN@EXAMPLE.COM",
            DateOfBirth = new DateTime(2003, 11, 18),
            Gender = GenderOptions.Male,
            CountryId = countryResponse1.CountryId,
            Address = "123 Main St, Anytown, USA",
            ReceiveNewsLetters = true
        };
        PersonAddRequest personAddRequest2 = new PersonAddRequest()
        {
            PersonName = "Jane Smith",
            Email = "JANE@EXAMPLE.COM",
            DateOfBirth = new DateTime(1995, 5, 20),
            Gender = GenderOptions.Female,
            CountryId = countryResponse2.CountryId,
            Address = "456 Elm St, Othertown, USA",
            ReceiveNewsLetters = false
        };
        List<PersonResponse> addedPersons = new List<PersonResponse>();
        foreach (PersonAddRequest personAddRequest in new PersonAddRequest[] { personAddRequest1, personAddRequest2 })
        {
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);
            addedPersons.Add(personResponse);
        }
        // print personsresponse from addedpersons
        _output.WriteLine("Expected Persons:");
        foreach (PersonResponse person in addedPersons)
        {
            _output.WriteLine(person.ToString());
        }

        // Act
        List<PersonResponse> allPersons = _personService.GetAllPersons();
        _output.WriteLine("Actual Persons:");
        foreach (PersonResponse person in allPersons)
        {
            _output.WriteLine(person.ToString());
        }

        // Assert
        Assert.Equal(addedPersons.Count, allPersons.Count);
    }
    #endregion

    #region GetFilteredPersonResponse method tests
    // if the search text is empty and search by is "PersonName" it should return all persons.
    [Fact]
    public async Task GetFilteredPersons_EmptySearchText()
    {
        // Arrange
        CountryAddRequest countryAddRequest1 = new CountryAddRequest()
        {
            CountryName = "USA"
        };
        CountryAddRequest countryAddRequest2 = new CountryAddRequest()
        {
            CountryName = "Canada"
        };
        CountryResponse countryResponse1 = await _countryService.AddCountry(countryAddRequest1);
        CountryResponse countryResponse2 = await _countryService.AddCountry(countryAddRequest2);
        PersonAddRequest personAddRequest1 = new PersonAddRequest()
        {
            PersonName = "John Doe",
            Email = "JOHN@EXAMPLE.COM",
            DateOfBirth = new DateTime(2003, 11, 18),
            Gender = GenderOptions.Male,
            CountryId = countryResponse1.CountryId,
            Address = "123 Main St, Anytown, USA",
            ReceiveNewsLetters = true
        };
        PersonAddRequest personAddRequest2 = new PersonAddRequest()
        {
            PersonName = "Jane Smith",
            Email = "JANE@EXAMPLE.COM",
            DateOfBirth = new DateTime(1995, 5, 20),
            Gender = GenderOptions.Female,
            CountryId = countryResponse2.CountryId,
            Address = "456 Elm St, Othertown, USA",
            ReceiveNewsLetters = false
        };
        PersonAddRequest personAddRequest3 = new PersonAddRequest()
        {
            PersonName = "Alice Johnson",
            Email = "ALICE@EXAMPLE.COM",
            DateOfBirth = new DateTime(1990, 3, 15),
            Gender = GenderOptions.Female,
            CountryId = countryResponse1.CountryId,
            Address = "789 Oak St, Sometown, USA",
            ReceiveNewsLetters = true
        };

        List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>()
        { personAddRequest1, personAddRequest2, personAddRequest3 };
        List<PersonResponse> addedPersons = new List<PersonResponse>();
        foreach (PersonAddRequest personAddRequest in personAddRequests)
        {
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);
            addedPersons.Add(personResponse);
        }
        // print personsresponse from addedpersons
        _output.WriteLine("Expected Persons:");
        foreach (PersonResponse person in addedPersons)
        {
            _output.WriteLine(person.ToString());
        }

        // Act
        List<PersonResponse> SearchedPersons = _personService.GetFilteredPersonResponse("PersonName", "");

        _output.WriteLine("Actual Persons:");
        foreach (PersonResponse person in SearchedPersons)
        {
            _output.WriteLine(person.ToString());
        }

        // Assert
        foreach (PersonResponse person in addedPersons)
        {
            Assert.Contains(person, SearchedPersons);
        }
    }

    // first we will add few persons using AddPerson method ,then we will call GetFilterdPerson and serche by
    // "PersonName" with some search text , it should return all matching persons
    [Fact]
    public async Task GetFilteredPersons_SearchByPersonName()
    {
        // Arrange
        CountryAddRequest countryAddRequest1 = new CountryAddRequest()
        {
            CountryName = "USA"
        };
        CountryAddRequest countryAddRequest2 = new CountryAddRequest()
        {
            CountryName = "Canada"
        };
        CountryResponse countryResponse1 = await _countryService.AddCountry(countryAddRequest1);
        CountryResponse countryResponse2 = await _countryService.AddCountry(countryAddRequest2);
        PersonAddRequest personAddRequest1 = new PersonAddRequest()
        {
            PersonName = "YoUsef",
            Email = "JOHN@EXAMPLE.COM",
            DateOfBirth = new DateTime(2003, 11, 18),
            Gender = GenderOptions.Male,
            CountryId = countryResponse1.CountryId,
            Address = "123 Main St, Anytown, USA",
            ReceiveNewsLetters = true
        };
        PersonAddRequest personAddRequest2 = new PersonAddRequest()
        {
            PersonName = "mousef",
            Email = "JANE@EXAMPLE.COM",
            DateOfBirth = new DateTime(1995, 5, 20),
            Gender = GenderOptions.Female,
            CountryId = countryResponse2.CountryId,
            Address = "456 Elm St, Othertown, USA",
            ReceiveNewsLetters = false
        };
        PersonAddRequest personAddRequest3 = new PersonAddRequest()
        {
            PersonName = "Alice Johnson",
            Email = "ALICE@EXAMPLE.COM",
            DateOfBirth = new DateTime(1990, 3, 15),
            Gender = GenderOptions.Female,
            CountryId = countryResponse1.CountryId,
            Address = "789 Oak St, Sometown, USA",
            ReceiveNewsLetters = true
        };

        List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>()
        { personAddRequest1, personAddRequest2, personAddRequest3 };
        List<PersonResponse> addedPersons = new List<PersonResponse>();
        foreach (PersonAddRequest personAddRequest in personAddRequests)
        {
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);
            addedPersons.Add(personResponse);
        }
        // print personsresponse from addedpersons
        _output.WriteLine("Expected Persons:");
        foreach (PersonResponse person in addedPersons)
        {
            if (person.PersonName != null && person.PersonName.Contains("ou", StringComparison.OrdinalIgnoreCase))
            {
                _output.WriteLine(person.ToString());
            }
        }

        // Act
        List<PersonResponse> SearchedPersons =
        _personService.GetFilteredPersonResponse(nameof(Person.PersonName), "ou");

        _output.WriteLine("Actual Persons:");
        foreach (PersonResponse person in SearchedPersons)
        {
            _output.WriteLine(person.ToString());
        }

        // Assert
        foreach (PersonResponse person in addedPersons)
        {
            if (person.PersonName != null && person.PersonName.Contains("ou", StringComparison.OrdinalIgnoreCase))
            {
                Assert.Contains(person, SearchedPersons);
            }
        }
    }

    #endregion

    #region GetSortedPersons method tests
    // when we sort persons by "PersonName" in ascending order, 
    // it should return sorted list of persons by "PersonName" in ascending order
    [Fact]
    public async Task GetSortedPersons_SortByPersonName_Ascending()
    {
        // Arrange
        CountryAddRequest countryAddRequest1 = new CountryAddRequest()
        {
            CountryName = "USA"
        };
        CountryAddRequest countryAddRequest2 = new CountryAddRequest()
        {
            CountryName = "Canada"
        };
        CountryResponse countryResponse1 = await _countryService.AddCountry(countryAddRequest1);
        CountryResponse countryResponse2 = await _countryService.AddCountry(countryAddRequest2);
        PersonAddRequest personAddRequest1 = new PersonAddRequest()
        {
            PersonName = "John Doe",
            Email = "JOHN@EXAMPLE.COM",
            DateOfBirth = new DateTime(2003, 11, 18),
            Gender = GenderOptions.Male,
            CountryId = countryResponse1.CountryId,
            Address = "123 Main St, Anytown, USA",
            ReceiveNewsLetters = true
        };
        PersonAddRequest personAddRequest2 = new PersonAddRequest()
        {
            PersonName = "Jane Smith",
            Email = "JANE@EXAMPLE.COM",
            DateOfBirth = new DateTime(1995, 5, 20),
            Gender = GenderOptions.Female,
            CountryId = countryResponse2.CountryId,
            Address = "456 Elm St, Othertown, USA",
            ReceiveNewsLetters = false
        };
        PersonAddRequest personAddRequest3 = new PersonAddRequest()
        {
            PersonName = "Alice Johnson",
            Email = "ALICE@EXAMPLE.COM",
            DateOfBirth = new DateTime(1990, 3, 15),
            Gender = GenderOptions.Female,
            CountryId = countryResponse1.CountryId,
            Address = "789 Oak St, Sometown, USA",
            ReceiveNewsLetters = true
        };

        List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>()
        { personAddRequest1, personAddRequest2, personAddRequest3 };
        List<PersonResponse> addedPersons = new List<PersonResponse>();
        foreach (PersonAddRequest personAddRequest in personAddRequests)
        {
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);
            addedPersons.Add(personResponse);
        }

        addedPersons = addedPersons.OrderBy(p => p.PersonName).ToList();
        // print personsresponse from addedpersons
        _output.WriteLine("Expected Persons:");
        foreach (PersonResponse person in addedPersons)
        {
            _output.WriteLine(person.ToString());
        }

        // Act
        List<PersonResponse> allPersons = _personService.GetAllPersons();
        SortedOrderOptions sortOrder = SortedOrderOptions.ASC;
        string sortBy = nameof(Person.PersonName);
        List<PersonResponse> SortedPersons = _personService.GetSortedPersons(allPersons, sortBy, sortOrder);

        _output.WriteLine("Actual Persons:");
        foreach (PersonResponse person in SortedPersons)
        {
            _output.WriteLine(person.ToString());
        }

        // Assert
        for (int index = 0; index < addedPersons.Count; index++)
        {
            Assert.Equal(addedPersons[index], SortedPersons[index]);
        }
    }
    #endregion

    #region UpdatePerson

    //  when we supply null as PersonUpdateRequest , it should throw argumnet null exception
    [Fact]
    public void UpdataPerson_NullPersonUpdateRequest()
    {
        // Arrange
        PersonUpdateRequest? personUpdateRequest = null;

        // Assert and Act
        Assert.Throws<ArgumentNullException>(() =>
        {
            _personService.UpdatePerson(personUpdateRequest);
        });

    }

    // when we supply invalid person id , it should throw Argument Null Exception
    [Fact]
    public void UpdataPerson_InvalidPersonID()
    {
        // Arrange
        PersonUpdateRequest? personUpdateRequest = new()
        {
            PersonId = new Guid(),
            PersonName = "yousef",
            Email = "yousef@test.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = GenderOptions.Male
        };

        // Assert and Act
        Assert.Throws<ArgumentNullException>(() =>
        {
            _personService.UpdatePerson(personUpdateRequest);
        });

    }

    // when PersonName is null, it shouls throw argument null exception
    [Fact]
    public async Task UpdatePerson_PersonNameIsNull()
    {
        CountryAddRequest countryAddRequest = new()
        {
            CountryName = "EG"
        };
        CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);
        PersonAddRequest personAddRequest = new()
        {
            PersonName = "Yousef",
            Email = "test@example.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse.CountryId,
            Address = "123 Main St, Anytown, USA",
            ReceiveNewsLetters = true
        };

        PersonResponse personResponse_added = _personService.AddPerson(personAddRequest);
        PersonUpdateRequest personUpdateRequest = personResponse_added.ToPersonUpdateRequest();
        personUpdateRequest.PersonName = null;

        // Assert and Act
        Assert.Throws<ArgumentException>(() =>
        {
            _personService.UpdatePerson(personUpdateRequest);
        });

    }

    // first add a new person and try to update the person name and email .
    [Fact]

    public async Task UpdatePerson_UpdateNameAndEmailAsync()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new()
        {
            CountryName = "EG"
        };
        CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);
        PersonAddRequest personAddRequest = new()
        {
            PersonName = "Yousef",
            Email = "test@example.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse.CountryId,
            Address = "123 Main St, Anytown, USA",
            ReceiveNewsLetters = true
        };

        PersonResponse personResponse_added = _personService.AddPerson(personAddRequest);
        PersonUpdateRequest personUpdateRequest = personResponse_added.ToPersonUpdateRequest();

        personUpdateRequest.PersonName = "Updated Name";
        personUpdateRequest.Email = "updated@example.com";

        // Act
        PersonResponse personResponse_updated = _personService.UpdatePerson(personUpdateRequest);
        PersonResponse personResponse_fromGetById = _personService.GetPersonByPersonId(personResponse_added.PersonId)!;

        // Assert
        Assert.NotNull(personResponse_updated);
        Assert.Equal(personResponse_fromGetById.PersonName, personResponse_updated.PersonName);
        Assert.Equal(personResponse_fromGetById.Email, personResponse_updated.Email);
        Assert.Equal("Updated Name", personResponse_updated.PersonName);
        Assert.Equal("updated@example.com", personResponse_updated.Email);
    }


    #endregion

    #region DeletePerson
    // when we supply valid personId , it should return True
    [Fact]
    public async Task DeletePerson_ValidPersonId()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new()
        {
            CountryName = "EG"
        };
        CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);
        PersonAddRequest personAddRequest = new()
        {
            PersonName = "Yousef",
            Email = "test@example.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse.CountryId,
            Address = "123 Main St, Anytown, USA",
            ReceiveNewsLetters = true
        };

        PersonResponse personResponse_added = _personService.AddPerson(personAddRequest);

        // Act
        bool isDeleted = _personService.DeletePerson(personResponse_added.PersonId);

        // Assert
        Assert.True(isDeleted);
    }

    // when we supply invalid personId , it should return false
    [Fact]
    public void DeletePerson_InvalidPersonId()
    {
        // Arrange
        Guid personId = Guid.NewGuid();

        // Act
        bool isDeleted = _personService.DeletePerson(personId);

        // Assert
        Assert.False(isDeleted);
    }
    #endregion
}

