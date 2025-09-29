using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTOs;
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
}
