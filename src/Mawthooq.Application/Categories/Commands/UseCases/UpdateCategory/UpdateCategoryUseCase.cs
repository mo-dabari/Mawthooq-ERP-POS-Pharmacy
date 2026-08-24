using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Domain.Categories;

namespace Mawthooq.Application.Categories.Commands.UseCases.UpdateCategory
{
    public class UpdateCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        public UpdateCategoryUseCase(ICategoryRepository categoryRepository)
        {
            ArgumentNullException.ThrowIfNull(categoryRepository, nameof(categoryRepository));

            _categoryRepository = categoryRepository;
        }

        public async Task<UpdateCategoryResponse> ExecuteAsync(UpdateCategoryRequest request, CancellationToken cancellationToken = default)
        {

            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var (category, _) = await _categoryRepository.GetByIdAsync(request.CategoryId.ToDomain(), cancellationToken);

            if (category is null)
                throw new InvalidOperationException($"Category with ID {request.CategoryId} not found.");

            category.Rename(request.Name);
            category.ChangeDescription(request.Description);

            byte[] newRowVersion = await _categoryRepository.UpdateAsync(category, request.rowVersion, cancellationToken);

            return new UpdateCategoryResponse(
                category.CategoryId.ToDto(),
                category.Name,
                category.Description,
                newRowVersion);
        }
    }
}
