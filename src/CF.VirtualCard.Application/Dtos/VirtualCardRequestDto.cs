using System.ComponentModel.DataAnnotations;

namespace CF.VirtualCard.Application.Dtos;

public record VirtualCardRequestDto
{
    [Required(ErrorMessage = "The CardNumber field is required.")]
	[StringLength(19, MinimumLength = 16, ErrorMessage = "The CardNumber field must be between 16 and 19 characters.")]
	[Display(Name = "CardNumber")]
    public string CardNumber { get; set; }

    [Required(ErrorMessage = "The First Name field is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "The First Name field must be between 2 and 100 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "The Surname field is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "The Surname field must be between 2 and 100 characters.")]
    [Display(Name = "Surname")]
    public string Surname { get; set; }
}