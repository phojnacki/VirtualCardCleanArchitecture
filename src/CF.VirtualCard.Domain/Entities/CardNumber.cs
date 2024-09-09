using CF.VirtualCard.Domain.Ddd;
using CF.VirtualCard.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace CF.VirtualCard.Domain.Entities
{
	public class CardNumber : ValueObject
	{
		public string Value { get; private set; }

		public CardNumber(string value)
		{
			Value = RemoveNonDigits(value);

			if (Value.Length != 16) {
				throw new CardNumberInvalidException("Card number is not a valid 16 digits with optional spaces or dashes.");
			}
		}

		private static string RemoveNonDigits(string cardNumberString) => Regex.Replace(cardNumberString, @"[^\d]", "");

		public override string ToString() { 
			return Value;
		}

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value;
		}
	}
}