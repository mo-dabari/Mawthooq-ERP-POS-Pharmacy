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
            ArgumentNullException.ThrowIfNull(categoryRepository, nameof(categoryRepository));

            _categoryRepository = categoryRepository;
        }

        public async Task<CreateCategoryResponse> ExecuteAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var category = Category.Create(CategoryId.New(), request.Name, request.Description);
            var (saveCategory, rowVersion) = await _categoryRepository.AddAsync(category, cancellationToken);

            if (saveCategory is null || rowVersion is null)
                throw new InvalidOperationException("Failed to create category");

            return new CreateCategoryResponse(
                saveCategory.CategoryId.ToDto(),
                saveCategory.Name,
                saveCategory.Description,
                saveCategory.Status,
                rowVersion);
        }
    }
}
