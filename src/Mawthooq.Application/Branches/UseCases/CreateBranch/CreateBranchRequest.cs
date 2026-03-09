using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Branches.UseCases.CreateBranch
{
    public record class CreateBranchRequest (string Name, AddressDto Location)
    {

    }
}
