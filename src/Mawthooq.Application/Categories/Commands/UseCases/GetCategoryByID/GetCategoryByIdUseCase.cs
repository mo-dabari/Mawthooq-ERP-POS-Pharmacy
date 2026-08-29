using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Domain.Categories;

namespace Mawthooq.Application.Categories.UseCases.GetCategoryByID
{
    public class GetCategoryByIdUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        public GetCategoryByIdUseCase(ICategoryRepository categoryRepository)
        {
            ArgumentNullException.ThrowIfNull(categoryRepository, nameof(categoryRepository));

            _categoryRepository = categoryRepository;
        }

        public async Task<GetCategoryByIdResponse> ExecuteAsync(CategoryIdDto categoryId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(categoryId, nameof(categoryId));
            var (category, _) = await _categoryRepository.GetByIdAsync(categoryId.ToDomain(), cancellationToken);

            if (category is null)
                throw new InvalidOperationException($"Category with id {categoryId} not found");

            return new GetCategoryByIdResponse(
                category.CategoryId.ToDto(),
                category.Name,
                category.Description,
                category.Status);
        }
    }
}
