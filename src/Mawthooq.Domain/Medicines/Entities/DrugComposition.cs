using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.ActiveIngredients.ValueObjects;
using Mawthooq.Domain.Medicines.ValueObjects;

namespace Mawthooq.Domain.Medicines.Entities
{
    public record class MedicineIngredient
    {
        public int ActiveIngredientId { get; private set; }
        public IngredientAmount IngredientAmount { get; private set; }

        public MedicineIngredient(int activeIngredientId, IngredientAmount ingredientAmount)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(activeIngredientId, nameof(activeIngredientId));
            ArgumentNullException.ThrowIfNull(ingredientAmount, nameof(ingredientAmount));

            ActiveIngredientId = activeIngredientId;
            IngredientAmount = ingredientAmount;
        }

        // internal void ChangeMedicine(int medicineId)
        // {
        //     ArgumentNullException.ThrowIfNull(medicineId, nameof(medicineId));
        //     MedicineId = medicineId;
        // }

        // internal void ChangeActiveIngredient(int activeIngredientId)
        // {
        //     ArgumentOutOfRangeException.ThrowIfNegativeOrZero(activeIngredientId, nameof(activeIngredientId));
        //     ActiveIngredientId = activeIngredientId;
        // }

        // internal void ChangeIngredientAmount(IngredientAmount ingredientAmount)
        // {
        //     ArgumentNullException.ThrowIfNull(ingredientAmount, nameof(ingredientAmount));
        //     IngredientAmount = ingredientAmount;
        // }

        // public override bool Equals(object? obj)
        // {
        //     if (obj is not DrugComposition other)
        //         return false;

        //     if (ReferenceEquals(this, other))
        //         return true;

        //     return MedicineId.Equals(other.MedicineId) && ActiveIngredientId.Equals(other.ActiveIngredientId);
        // }

        // public override int GetHashCode()
        // {
        //     return HashCode.Combine(MedicineId, ActiveIngredientId);
        // }
    }
}
