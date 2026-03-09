using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Branches.UseCases.CreateBranch
{
    public record class CreateBranchResponse (int BranchId, string Name, AddressDto Location, bool IsActive)
    {

    }
}
