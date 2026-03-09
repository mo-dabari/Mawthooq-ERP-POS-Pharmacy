using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Branches.UseCases.GetBranchByID
{
    public record class GetBranchByIDResponse(int BranchId, string Name, AddressDto Location, bool IsActive)
    {

    }
}
