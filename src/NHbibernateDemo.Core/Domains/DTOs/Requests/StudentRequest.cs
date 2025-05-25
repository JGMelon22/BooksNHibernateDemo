using System.ComponentModel.DataAnnotations;

namespace NHbibernateDemo.Core.Domains.DTOs.Requests;

public record StudentRequest
(
    [Required(AllowEmptyStrings = false, ErrorMessage = "Student Name is a required field!")]
    [MinLength(2, ErrorMessage = "Student Name must contain at least 2 characters!")]
    [MaxLength(100, ErrorMessage = "Student Name can not exceed 100 characters!")]
    string Name,

    [Required(AllowEmptyStrings = false, ErrorMessage = "Student Email is a required field!")]
    [EmailAddress(ErrorMessage = "Not a valid E-mail was passed!")]
    string Email,

    [Required(AllowEmptyStrings = false, ErrorMessage = "Student Course is a required field!")]
    [MinLength(2, ErrorMessage = "Student Course must contain at least 2 characters!")]
    [MaxLength(100, ErrorMessage = "Student Course can not exceed 100 characters!")]
    string Course,

    [AllowedValues("Male", "Female", "Other", ErrorMessage = "Gender must be Male, Female, or Other")]
    string Gender
);
