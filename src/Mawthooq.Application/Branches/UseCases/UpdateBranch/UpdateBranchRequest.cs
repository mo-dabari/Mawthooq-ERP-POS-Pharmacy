using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Branches.UseCases.UpdateBranch
{
    public record class UpdateBranchRequest(int BranchId, string Name, AddressDto Location, bool IsActive)
    {

    }
}
