using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Returns.DTOs;

namespace Mawthooq.Application.Returns.UseCase
{
    public record class CreateReturnRequest (int SaleId, int BranchId, List<ReturnedProductDto> ReturnedProducts)
    {

    }
}
