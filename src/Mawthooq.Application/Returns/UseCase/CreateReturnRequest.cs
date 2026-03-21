using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Returns.DTOs;
using Mawthooq.Domain.Returns.Enums;

namespace Mawthooq.Application.Returns.UseCase
{
    public record class CreateReturnRequest (int SaleId, int BranchId, List<ReturnedProductDto> ReturnedProducts)
    {

    }
}
