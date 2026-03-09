using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories;

namespace Mawthooq.Application.Categories.UseCases.UpdateCategory
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

            Category? category = _categoryRepository.GetById(request.CategoryId);

            if (category is null)
                throw new InvalidOperationException($"Category with ID {request.CategoryId} not found.");

            category.ChangeName(request.Name);
            category.ChangeDescription(request.Description);

            if (request.IsActive)
                category.Activate();
            else
                category.Deactivate();

            _categoryRepository.Update(category);

            return new UpdateCategoryResponse(category.CategoryId, category.Name, category.Description, category.IsActive);
        }
    }
}
