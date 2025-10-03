using ServiceContracts.DTOs;
using ServiceContracts.Enums;
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

    /// <summary>
    /// Returns a person by personId
    /// </summary>
    /// <param name="personId"></param>
    /// <returns>PersonResponse</returns>
    PersonResponse? GetPersonByPersonId(Guid? personId);

    /// <summary>
    /// Returns filtered list of persons by searchBy and searchString
    /// </summary>
    /// <param name="searchBy"></param>
    /// <param name="searchString"></param>
    /// <returns>returns all matching persons based on the given search field and search string</returns>
    List<PersonResponse> GetFilteredPersonResponse(string? searchBy, string? searchString);

    /// <summary>
    /// Returns sorted list of persons by sortBy and sortOrder
    /// </summary>
    /// <param name="allPersons"></param>
    /// <param name="sortBy"></param>
    /// <param name="sortOrder"></param>
    /// <returns>returns sorted list of persons based on the given sort field and sort order</returns>
    List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string? sortBy, SortedOrderOptions? sortOrder);

    /// <summary>
    ///  Update the specified person based on the given personId
    /// </summary>
    /// <param name="personUpdateRequest"></param>
    /// <returns>return updated person as PersonResponse</returns>
    PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);

    /// <summary>
    /// Deletes a person based on the given personId
    /// </summary>
    /// <param name="personId"></param>
    /// <returns>returns true if the person was deleted successfully, otherwise false</returns>
    bool DeletePerson(Guid? personId);
}
