using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Categories.Exceptions;
using Mawthooq.Application.Categories.UseCases.GetCategoryByID;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Domain.Categories;

namespace Mawthooq.Application.Categories.Commands.UseCases.GetCategoryByID
{
    public class GetCategoryByIdUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        public GetCategoryByIdUseCase(ICategoryRepository categoryRepository)
        {
            ArgumentNullException.ThrowIfNull(categoryRepository, nameof(categoryRepository));

            _categoryRepository = categoryRepository;
        }

        public async Task<GetCategoryByIdResponse> ExecuteAsync(CategoryIdDto categoryIdDto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(categoryIdDto, nameof(categoryIdDto));
            var (category, _) = await _categoryRepository.GetByIdAsync(categoryIdDto.ToDomain(), cancellationToken);

            if (category is null)
                throw new CategoryNotFoundException(categoryIdDto.Value);

            return new GetCategoryByIdResponse(
                category.CategoryId.ToDto(),
                category.Name,
                category.Description,
                category.Status);
        }
    }
}
