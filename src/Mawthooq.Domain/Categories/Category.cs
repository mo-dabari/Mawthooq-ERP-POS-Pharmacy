using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Categories
{
    public class Category
    {
        public int CategoryId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public Category(int categoryId, string name, string description) : this(name, description)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(categoryId, nameof(categoryId));

            ValidateArguments(name, description);

            CategoryId = categoryId;
        }

        private Category(string name, string description)
        {
            ValidateArguments(name, description);

            Name = name;
            Description = description;
            IsActive = true;
        }

        public static Category Create(string name, string description)
        {
            return new Category(name, description);
        }
        public void SetCategoryId(int categoryID)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(categoryID, nameof(categoryID));
            if (CategoryId != 0)
                throw new InvalidOperationException("Category ID is already set and cannot be changed.");
            CategoryId = categoryID;
        }

        public void ChangeName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            if (Name == name)
                return;

            Name = name;
        }

        public void ChangeDescription(string description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(description, nameof(description));
            if (Description == description)
                return;

            Description = description;
        }

        public void Activate()
        {
            if (IsActive)
                return;

            IsActive = true;
        }

        public void Deactivate()
        {
            if (!IsActive)
                return;

            IsActive = false;
        }

        private void ValidateArguments(string name, string description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            ArgumentException.ThrowIfNullOrWhiteSpace(description, nameof(description));
        }
    }
}
