using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Abstractions;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Domain.Branches
{
    public interface IBranchRepository : IBaseRepository<Branch, int>
    {
        Branch Add(Branch branch, Address address);
    }
}
