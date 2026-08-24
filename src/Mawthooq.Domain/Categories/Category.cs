using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories.Enums;
using Mawthooq.Domain.Categories.ValueObjects;

namespace Mawthooq.Domain.Categories
{
    public class Category
    {
        public CategoryId CategoryId { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Status { get; private set; }
        private Category(CategoryId categoryId, string name, string description)
        {
            ValidateArguments(categoryId, name, description);

            CategoryId = categoryId;
            Name = name;
            Description = description.Trim();
            Status = true;
        }

        public static Category Create(CategoryId categoryId, string name, string description)
        {
            return new Category(categoryId, name, description);
        }

        public void Rename(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            name = name.Trim();

            if (string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
                return;

            Name = name;
        }

        public void ChangeDescription(string description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(description, nameof(description));
            description = description.Trim();

            if (Description == description)
                return;

            Description = description;
        }

        public void Activate()
        {
            if (Status == true)
                return;

            Status = true;
        }

        public void Deactivate()
        {
            if (Status == false)
                return;

            Status = false;
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
