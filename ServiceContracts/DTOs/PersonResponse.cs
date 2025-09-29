using Entities;
namespace ServiceContracts.DTOs;

/// <summary>
/// Represents DTO class that is used as return 
/// type of most methods in PersonsService
/// </summary>
public class PersonResponse
{
    public Guid PersonId { get; set; }
    public string? PersonName { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public Guid? CountryId { get; set; }
    public string? CountryName { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }
    public double? Age { get; set; }

    /// <summary>
    /// Override Equals method to compare two PersonResponse objects based on PersonId
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>true if the objects are equal; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != typeof(PersonResponse))
        {
            return false;
        }

        PersonResponse other = (PersonResponse)obj;
        return PersonId == other.PersonId &&
                PersonName == other.PersonName &&
                Email == other.Email &&
                DateOfBirth == other.DateOfBirth &&
                Gender == other.Gender &&
                CountryId == other.CountryId &&
                Address == other.Address &&
                ReceiveNewsLetters == other.ReceiveNewsLetters;
    }

    override public int GetHashCode()
    {
        return PersonId.GetHashCode();
    }

}

public static class PersonResponseExtensions
{
    /// <summary>
    /// Converts Person entity to PersonResponse DTO
    /// </summary>
    /// <param name="person">Person entity</param>
    /// <returns>PersonResponse DTO</returns>
    public static PersonResponse ToPersonResponse(this Person person)
    {
        if (person == null)
        {
            throw new ArgumentNullException(nameof(person), "person cannot be null");
        }

        return new PersonResponse()
        {
            PersonId = person.PersonId,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            Gender = person.Gender,
            CountryId = person.CountryId,
            Address = person.Address,
            ReceiveNewsLetters = person.ReceiveNewsLetters,
            Age = (person.DateOfBirth == DateTime.MinValue || person.DateOfBirth == null)
            ? null
            : Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25, 1),
        };
    }
}
