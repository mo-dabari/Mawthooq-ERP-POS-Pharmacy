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
            _categoryRepository = categoryRepository;
        }

        public UpdateCategoryResponse Execute(UpdateCategoryRequest request)
        {

            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Category? category = _categoryRepository.GetById(request.CategoryId.ToDomain());

            if (category is null)
                throw new InvalidOperationException($"Category with ID {request.CategoryId} not found.");

            category.Rename(request.Name);
            category.ChangeDescription(request.Description);

            _categoryRepository.Update(category, request.rowVersion);

            return new UpdateCategoryResponse(category.CategoryId.ToDto(), category.Name.ToString(), category.Description);
        }
    }
}
