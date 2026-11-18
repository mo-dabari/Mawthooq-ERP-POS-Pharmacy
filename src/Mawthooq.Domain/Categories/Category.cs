using System;
using System.Collections.Generic;
using System.Linq;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Categories
{
    public class Category
    {
        public const int MaxNameLength = 100;
        public const int MaxDescriptionLength = 500;
        public CategoryId CategoryId { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Status Status { get; private set; }
        private Category(CategoryId categoryId, string name, string description)
        {
            ValidateArguments(categoryId, name, description);

            name = name.Trim();
            LengthGuard.MaxLength(name, MaxNameLength, nameof(name));

            description = description.Trim();
            LengthGuard.MaxLength(description, MaxDescriptionLength, nameof(description));

            CategoryId = categoryId;
            Name = name;
            Description = description;
            Status = Status.Active;
        }

        public static Category Create(CategoryId categoryId, string name, string description)
        {
            return new Category(categoryId, name, description);
        }

        public void Rename(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            name = name.Trim();

            LengthGuard.MaxLength(name, MaxNameLength, nameof(name));

            if (string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
                return;

            Name = name;
        }

        public void ChangeDescription(string description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(description, nameof(description));

            description = description.Trim();

            LengthGuard.MaxLength(description, MaxDescriptionLength, nameof(description));

            if (Description == description)
                return;

            Description = description;
        }

        public void Activate()
        {
            if (Status == Status.Active)
                return;

            Status = Status.Active;
        }

        public void Deactivate()
        {
            if (Status == Status.Inactive)
                return;

            Status = Status.Inactive;
        }

        public void ChangeStatus(bool isActive)
        {
            if (isActive)
                Activate();
            else
                Deactivate();
        }
        private void ValidateArguments(CategoryId categoryId, string name, string description)
        {
            ArgumentNullException.ThrowIfNull(categoryId, nameof(categoryId));

            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            ArgumentException.ThrowIfNullOrWhiteSpace(description, nameof(description));
        }
    }
}
