using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.ActiveIngredients.ValueObjects;

namespace Mawthooq.Domain.ActiveIngredients
{
    public class ActiveIngredient
    {
        public int ActiveIngredientId { get; }
        public ActiveIngredientName Name { get; private set; }
        public ActiveIngredientDescription Description { get; private set; }

        public ActiveIngredient(int activeIngredientId, ActiveIngredientName name, ActiveIngredientDescription description)
            : this(name, description)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(activeIngredientId, nameof(activeIngredientId));

            ActiveIngredientId = activeIngredientId;
        }

        private ActiveIngredient(ActiveIngredientName name, ActiveIngredientDescription description)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(description, nameof(description));

            Name = name;
            Description = description;
        }

        public static ActiveIngredient Create(ActiveIngredientName name, ActiveIngredientDescription description)
        {
            return new ActiveIngredient(name, description);
        }

        public void Rename(ActiveIngredientName newName)
        {
            if (Name.Value.Equals(newName.Value, StringComparison.OrdinalIgnoreCase))
                return;

            Name = newName;
        }

        public void ChangeDescription(ActiveIngredientDescription description)
        {
            if (Name.Value.Equals(description.Value, StringComparison.OrdinalIgnoreCase))
                return;

            Description = description;
        }
    }
}
