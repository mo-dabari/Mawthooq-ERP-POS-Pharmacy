using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Suppliers.Enums;

namespace Mawthooq.Application.Suppliers.UseCases.UpdateSupplierStatus
{
    public record class UpdateSupplierStatusResponse(int SupplierId, SupplierStatus Status)
    {

    }
}
