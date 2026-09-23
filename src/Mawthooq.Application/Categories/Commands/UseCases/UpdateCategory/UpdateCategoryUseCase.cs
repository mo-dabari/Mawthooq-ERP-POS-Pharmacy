using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Categories.Exceptions;
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

            var (category, _) = await _categoryRepository.GetByIdAsync(request.CategoryIdDto.ToDomain(), cancellationToken);

            if (category is null)
                throw new CategoryNotFoundException(request.CategoryIdDto.Value);

            category.Rename(request.Name);
            category.ChangeDescription(request.Description);

            byte[] newRowVersion = await _categoryRepository.UpdateAsync(category, request.RowVersion, cancellationToken);

            return new UpdateCategoryResponse(
                category.CategoryId.ToDto(),
                category.Name,
                category.Description,
                newRowVersion);
        }
    }
}
