using ServiceContracts.DTOs;
namespace ServiceContracts;

/// <summary>
/// represents business logic for Person entity in the system
/// </summary>
public interface IPersonService
{
    /// <summary>
    /// Adds a new person in a list of persons in system
    /// </summary>
    /// <param name="personAddRequest"></param>
    /// <returns>PersonResponse</returns>
    PersonResponse AddPerson(PersonAddRequest? personAddRequest);

    /// <summary>
    /// Return all persons in the list of persons in system
    /// </summary>
    /// <returns>List of PersonResponse</returns>
    List<PersonResponse> GetAllPersons();
}
