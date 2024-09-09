using System.ComponentModel.DataAnnotations;

namespace CF.Api.Controllers
{
	public record WithdrawRequestDto
	{
		[Required(ErrorMessage = "The WithdrawalAmount field is required.")]
		[Display(Name = "WithdrawalAmount")]
		public decimal Amount { get; set; }

	}
}