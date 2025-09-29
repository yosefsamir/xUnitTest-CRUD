using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTOs;

/// <summary>
/// Data Transfer Object for adding a new person
/// </summary>
public class PersonAddRequest
{
    [Required(ErrorMessage = "Person name is required")]
    public string? PersonName { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Date of birth is required")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Gender is required")]
    public GenderOptions? Gender { get; set; }
    public Guid? CountryId { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }


    /// <summary>
    /// Converts PersonAddRequest to Person entity
    /// </summary>
    /// <returns>Person entity</returns>
    public Person ToPerson()
    {
        return new Person()
        {
            PersonName = this.PersonName,
            Email = this.Email,
            DateOfBirth = this.DateOfBirth,
            Gender = this.Gender.ToString(),
            CountryId = this.CountryId,
            Address = this.Address,
            ReceiveNewsLetters = this.ReceiveNewsLetters
        };
    }
}
