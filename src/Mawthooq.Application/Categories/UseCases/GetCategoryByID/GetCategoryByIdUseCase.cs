using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories;

namespace Mawthooq.Application.Categories.UseCases.GetCategoryByID
{
    public class GetCategoryByIdUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        public GetCategoryByIdUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public GetCategoryByIdResponse Execute(int categoryId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(categoryId, nameof(categoryId));
            Category? category = _categoryRepository.GetById(categoryId);

            if (category is null)
                throw new InvalidOperationException($"Category with id {categoryId} not found");

            return new GetCategoryByIdResponse(category.CategoryId, category.Name, category.Description, category.IsActive);
        }
    }
}
