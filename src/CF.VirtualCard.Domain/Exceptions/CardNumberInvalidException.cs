using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.VirtualCard.Domain.Exceptions
{
	public class CardNumberInvalidException(string message) : ValidationException(message)
	{
	}
}
