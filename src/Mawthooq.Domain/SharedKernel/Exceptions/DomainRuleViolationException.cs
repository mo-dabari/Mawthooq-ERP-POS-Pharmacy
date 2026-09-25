using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.SharedKernel.Exceptions
{
    public class DomainRuleViolationException : Exception
    {
        public DomainRuleViolationException(string message) : base(message)
        {
        }
    }
}
