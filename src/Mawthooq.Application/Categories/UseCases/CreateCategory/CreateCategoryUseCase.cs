using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories;

namespace Mawthooq.Application.Categories.UseCases.CreateCategory
{
    public class CreateCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CreateCategoryUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public CreateCategoryResponse Execute(CreateCategoryRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            var category = Category.Create(request.Name, request.Description);
            Category? saveCategory = _categoryRepository.Add(category);
            return new CreateCategoryResponse(saveCategory.CategoryId, saveCategory.Name, saveCategory.Description, saveCategory.IsActive);
        }
    }
}
