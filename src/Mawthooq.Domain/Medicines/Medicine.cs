using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Medicines.Entities;
using Mawthooq.Domain.Medicines.Enums;
using Mawthooq.Domain.Medicines.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.TherapeuticClasses.ValueObjects;

namespace Mawthooq.Domain.Medicines
{
    public class Medicine
    {
        private List<MedicineIngredient> _medicineIngredient = new();
        public ProductId ProductId { get; }
        public MedicineId MedicineId { get; }
        public MedicineName MedicineName { get; private set; }
        public DosageForm DosageForm { get; }
        public ReferenceQuantity ReferenceQuantity { get; }
        public int TherapeuticClassId { get; private set; }
        public int ManufacturerId { get; private set; }
        public IReadOnlyList<MedicineIngredient> DrugCompositionsValues => _medicineIngredient.AsReadOnly();

        private Medicine(
            ProductId productId,
            MedicineId medicineId,
            MedicineName medicineName,
            DosageForm dosageForm,
            ReferenceQuantity referenceQuantity,
            int therapeuticClassId,
            int manufacturerId
        )
        {
            ArgumentNullException.ThrowIfNull(productId, nameof(productId));
            ArgumentNullException.ThrowIfNull(medicineId, nameof(medicineId));
            ArgumentNullException.ThrowIfNull(medicineName, nameof(medicineName));
            ArgumentNullException.ThrowIfNull(dosageForm, nameof(dosageForm));
            ArgumentNullException.ThrowIfNull(referenceQuantity, nameof(referenceQuantity));

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(therapeuticClassId, nameof(therapeuticClassId));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(manufacturerId, nameof(manufacturerId));

            ProductId = productId;
            MedicineId = medicineId;
            MedicineName = medicineName;
            DosageForm = dosageForm;
            ReferenceQuantity = referenceQuantity;
            TherapeuticClassId = therapeuticClassId;
            ManufacturerId = manufacturerId;
        }

        public static Medicine Create(
            ProductId productId,
            MedicineId medicineId,
            MedicineName medicineName,
            DosageForm dosageForm,
            ReferenceQuantity referenceQuantity,
            int therapeuticClassId,
            int manufacturerId
        )
        {
            return new Medicine(productId, medicineId, medicineName, dosageForm, referenceQuantity, therapeuticClassId, manufacturerId);
        }

        public void Rename(MedicineName newMedicineName)
        {
            ArgumentNullException.ThrowIfNull(newMedicineName, nameof(newMedicineName));

            if (MedicineName.Value.Equals(newMedicineName.Value, StringComparison.OrdinalIgnoreCase))
                return;

            MedicineName = newMedicineName;
        }

        public void ChangeTherapeuticClass(int newTherapeuticClassId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newTherapeuticClassId, nameof(newTherapeuticClassId));

            if (TherapeuticClassId == newTherapeuticClassId)
                return;

            TherapeuticClassId = newTherapeuticClassId;
        }

        public void ChangeManufacturer(int newManufacturerId)
        {
            ArgumentNullException.ThrowIfNull(newManufacturerId, nameof(newManufacturerId));

            if (ManufacturerId.Equals(newManufacturerId))
                return;

            ManufacturerId = newManufacturerId;
        }

        public void AddIngredient(MedicineIngredient medicineIngredient)
        {
            ArgumentNullException.ThrowIfNull(medicineIngredient, nameof(medicineIngredient));

            ValidateFromActiveIngredientIsExistMedicine(medicineIngredient);

            _medicineIngredient.Add(medicineIngredient);
        }

        public void RemoveIngredient(MedicineIngredient drugComposition)
        {
            ArgumentNullException.ThrowIfNull(drugComposition, nameof(drugComposition));

            MedicineIngredient? existing = _medicineIngredient.FirstOrDefault(d => d.Equals(drugComposition));

            if (existing is null)
                throw new InvalidOperationException("The medicinal component to be modified is not present in the medication");

            _medicineIngredient.Remove(existing);
        }

        //implementation
        // public void ChangeIngredientAmount();

        private void ValidateFromActiveIngredientIsExistMedicine(MedicineIngredient medicineIngredient)
        {
            MedicineIngredient? hasDuplicates = _medicineIngredient.FirstOrDefault(d => d.Equals(medicineIngredient));

            if (hasDuplicates is not null)
                throw new InvalidOperationException("The active ingredient cannot be repeated within the same medication.");
        }
    }
}
