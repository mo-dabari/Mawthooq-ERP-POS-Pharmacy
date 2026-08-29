using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.SharedKernel.Abstractions;

namespace Mawthooq.Domain.Categories
{
    public interface ICategoryRepository : IBaseRepository<Category, CategoryId>
    {
        Task<bool> ExistsAndActiveAsync(CategoryId categoryId, CancellationToken cancellationToken = default);
        Task<Category?> GetByNameAsync(string categoryName, CancellationToken cancellationToken = default);
    }
}
