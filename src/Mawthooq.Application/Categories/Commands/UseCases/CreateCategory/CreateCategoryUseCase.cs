using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Categories.ValueObjects;

namespace Mawthooq.Application.Categories.Commands.UseCases.CreateCategory
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
            var category = Category.Create(CategoryId.New(), request.Name, request.Description);
            Category? saveCategory = _categoryRepository.Add(category);

            if (saveCategory is null)
                throw new InvalidOperationException("Failed to create category");

            return new CreateCategoryResponse(
                saveCategory.CategoryId.ToDto(),
                saveCategory.Name,
                saveCategory.Description,
                saveCategory.Status);
        }
    }
}
