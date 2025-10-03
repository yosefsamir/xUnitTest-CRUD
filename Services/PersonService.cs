using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTOs;
using ServiceContracts.Enums;
using Services.Helpers;
namespace Services;

public class PersonService : IPersonService
{

    private readonly List<Person> _persons = new List<Person>();
    private readonly ICountryService _countryService;

    public PersonService()
    {
        _countryService = new CountriesService();
    }

    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        PersonResponse personResponse = person.ToPersonResponse();
        personResponse.CountryName = _countryService.GetCountryByCountryId(personResponse.CountryId)?.CountryName;
        return personResponse;
    }
    public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
    {
        // validate
        if (personAddRequest == null)
        {
            throw new NullReferenceException(nameof(personAddRequest));
        }
        // model validation
        ValidationHelper.ModelValidation(personAddRequest);

        // convert PersonAddRequest to Person entity
        Person person = personAddRequest.ToPerson();
        person.PersonId = Guid.NewGuid();
        _persons.Add(person);

        // convert Person entity to PersonResponse
        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetAllPersons()
    {
        return _persons.Select(ConvertPersonToPersonResponse).ToList();
    }

    public PersonResponse? GetPersonByPersonId(Guid? personId)
    {
        if (personId == null)
            return null;
        Person? person = _persons.FirstOrDefault(p => p.PersonId == personId);

        if (person == null)
            return null;
        return person.ToPersonResponse();
    }

    public List<PersonResponse> GetFilteredPersonResponse(string? searchBy, string? searchString)
    {
        List<PersonResponse> allPersons = GetAllPersons();
        List<PersonResponse> filteredPersons = allPersons;
        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return filteredPersons;

        switch (searchBy)
        {
            case nameof(Person.PersonName):
                filteredPersons = allPersons
                    .Where(p => p.PersonName != null && p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;
            case nameof(Person.Email):
                filteredPersons = allPersons
                    .Where(p => p.Email != null && p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;
            case nameof(Person.Gender):
                filteredPersons = allPersons
                    .Where(p => p.Gender != null && p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;
            case nameof(Person.DateOfBirth):
                filteredPersons = allPersons
                    .Where(p => p.DateOfBirth != null && p.DateOfBirth.Value.ToString("dd/MM/yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;
            case nameof(Person.CountryId):
                filteredPersons = allPersons
                    .Where(p => p.CountryName != null && p.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;
            case nameof(Person.Address):
                filteredPersons = allPersons
                    .Where(p => p.Address != null && p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;
            default:
                filteredPersons = allPersons;
                break;
        }
        return filteredPersons;
    }

    public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string? sortBy, SortedOrderOptions? sortOrder)
    {
        if (string.IsNullOrEmpty(sortBy) || sortOrder == null)
            return allPersons;

        List<PersonResponse> sortedPersons = allPersons;

        switch (sortBy)
        {
            case nameof(Person.PersonName):
                sortedPersons = sortOrder == SortedOrderOptions.ASC
                    ? allPersons.OrderBy(p => p.PersonName).ToList()
                    : allPersons.OrderByDescending(p => p.PersonName).ToList();
                break;
            case nameof(Person.Email):
                sortedPersons = sortOrder == SortedOrderOptions.ASC
                    ? allPersons.OrderBy(p => p.Email).ToList()
                    : allPersons.OrderByDescending(p => p.Email).ToList();
                break;
            case nameof(Person.Gender):
                sortedPersons = sortOrder == SortedOrderOptions.ASC
                    ? allPersons.OrderBy(p => p.Gender).ToList()
                    : allPersons.OrderByDescending(p => p.Gender).ToList();
                break;
            case nameof(Person.DateOfBirth):
                sortedPersons = sortOrder == SortedOrderOptions.ASC
                    ? allPersons.OrderBy(p => p.DateOfBirth).ToList()
                    : allPersons.OrderByDescending(p => p.DateOfBirth).ToList();
                break;
            case nameof(Person.CountryId):
                sortedPersons = sortOrder == SortedOrderOptions.ASC
                    ? allPersons.OrderBy(p => p.CountryName).ToList()
                    : allPersons.OrderByDescending(p => p.CountryName).ToList();
                break;
            case nameof(Person.Address):
                sortedPersons = sortOrder == SortedOrderOptions.ASC
                    ? allPersons.OrderBy(p => p.Address).ToList()
                    : allPersons.OrderByDescending(p => p.Address).ToList();
                break;
        }
        return sortedPersons;
    }

    public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        // validate
        if (personUpdateRequest == null)
        {
            throw new ArgumentNullException(nameof(personUpdateRequest));
        }
        // model validation
        ValidationHelper.ModelValidation(personUpdateRequest);

        // get the person by personId from the list of persons
        Person? person = _persons.FirstOrDefault(p => p.PersonId == personUpdateRequest.PersonId);
        if (person == null)
        {
            throw new ArgumentNullException($"Given personId {personUpdateRequest.PersonId} does not exist");
        }

        // update the person details
        person.PersonName = personUpdateRequest.PersonName;
        person.Email = personUpdateRequest.Email;
        person.DateOfBirth = personUpdateRequest.DateOfBirth;
        person.Address = personUpdateRequest.Address;
        person.CountryId = personUpdateRequest.CountryId;
        person.Gender = personUpdateRequest.Gender.ToString();
        person.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

        return person.ToPersonResponse();
    }

    public bool DeletePerson(Guid? personId)
    {
        if (personId == null)
            return false;

        Person? person = _persons.FirstOrDefault(p => p.PersonId == personId);
        if (person == null)
            return false;

        return _persons.Remove(person);
    }

}
