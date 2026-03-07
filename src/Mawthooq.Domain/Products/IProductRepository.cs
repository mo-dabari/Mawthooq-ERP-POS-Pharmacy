using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Common.Interfaces;

namespace Mawthooq.Domain.Products
{
    public interface IProductRepository : IBaseRepository<Product, int>
    {

    }
}
