using System.ComponentModel.DataAnnotations;

namespace CF.VirtualCard.Application.Dtos;

public record VirtualCardRequestDto
{
    [Required(ErrorMessage = "The Email field is required.")]
    [EmailAddress(ErrorMessage = "The Email field is not a valid email address.")]
    [MaxLength(100, ErrorMessage = "The Email field must not exceed 100 characters.")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The First Name field is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "The First Name field must be between 2 and 100 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "The Surname field is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "The Surname field must be between 2 and 100 characters.")]
    [Display(Name = "Surname")]
    public string Surname { get; set; }
}