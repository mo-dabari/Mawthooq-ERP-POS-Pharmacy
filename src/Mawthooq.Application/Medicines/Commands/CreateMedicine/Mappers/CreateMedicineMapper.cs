using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Medicines.Commands.DTOs;
using Mawthooq.Domain.Medicines;
using Mawthooq.Domain.Medicines.Entities;
using Mawthooq.Domain.Medicines.Enums;
using Mawthooq.Domain.Medicines.ValueObjects;
using Mawthooq.Domain.Products;

namespace Mawthooq.Application.Medicines.Commands.CreateMedicine.Mappers
{
    public static class CreateMedicineMapper
    {
        public static Medicine ToDomain(this CreateMedicineRequest request, int ProductId)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            MedicineName medicineName = new(request.MedicineName);

            DosageForm dosageForm = new
            (
                ConvertFromStringToEnum<DosageFormType>(request.DosageForm)
            );

            ReferenceQuantity referenceQuantity = new
            (
                request.ReferenceQuantity.Quantity,
                ConvertFromStringToEnum<ReferenceQuantityUnits>
                (
                    request.ReferenceQuantity.Unit
                )
            );

            Medicine medicine = Medicine.Create
            (
                ProductId,
                medicineName, dosageForm,
                referenceQuantity,
                request.TherapeuticClassId,
                request.ManufacturerId
            );

            if (!request.DrugCompositions.Any())
                throw new InvalidOperationException("Cannot Add Medicine without Add Drug Compositions or Active Ingredients");

            foreach (var drugComposition in request.DrugCompositions)
            {
                IngredientAmount ingredientAmount = new IngredientAmount
                (
                    drugComposition.IngredientAmount.Amount,
                    ConvertFromStringToEnum<IngredientAmountUnits>
                    (
                        drugComposition.IngredientAmount.Unit
                    )
                );

                DrugComposition drug = new
                (
                    drugComposition.MedicineId,
                    drugComposition.ActiveIngredientId,
                    ingredientAmount
                );

                medicine.AddDrugComposition(drug);
            }
            return medicine;
        }


        public static CreateMedicineResponse ToDto(this Medicine medicine, Product product)
        {
            CreateProductResponseDto responseProductDto = new
            (
                product.ProductId,
                product.Name,
                product.SKU.Value,
                product.CategoryId,
                product.BaseUnit.ToString(),
                product.RequiresExpiryTracking,
                product.CanBeReturnedIfOpened,
                product.NearExpiryThresholdInDays,
                product.ImageUrl,
                new BarcodeDto
                (
                    product.Barcode?.Value ?? string.Empty,
                    product.Barcode?.Type.ToString() ?? string.Empty
                ),
                product.UnitDetails.Select
                (
                    ud => new CreateProductUnitDetailsDto
                    (
                        ud.Unit.ToString(),
                        ud.ConversionRate,
                        ud.Rank.ToString(),
                        new BarcodeDto
                        (
                            ud.UnitBarcode?.Value ?? string.Empty,
                            ud.UnitBarcode?.Type.ToString() ?? string.Empty
                        )
                    )
                ).ToImmutableList()
            );

            CreateMedicineResponse responseMedicine = new
            (
               responseProductDto,
                medicine.MedicineName.Value,
                medicine.DosageForm.Value.ToString(),
                new ReferenceQuantityDto
                (
                    medicine.ReferenceQuantity.Value,
                    medicine.ReferenceQuantity.Unit.ToString()
                ),
                medicine.TherapeuticClassId,
                medicine.ManufacturerId,
                medicine.DrugCompositionsValues.Select
                (
                    dc => new DrugCompositionDto
                    (
                        dc.MedicineId,
                        dc.ActiveIngredientId,
                        new IngredientAmountDto
                        (
                            dc.IngredientAmount.Value,
                            dc.IngredientAmount.Unit.ToString()
                        )
                    )
                ).ToImmutableList()
            );

            return responseMedicine;
        }

        private static T ConvertFromStringToEnum<T>(string value) where T : struct, Enum
        {
            if (!Enum.TryParse(value, ignoreCase: true, out T unit))
                throw new InvalidOperationException($"'{value}' is not a valid {typeof(T).Name}");

            return unit;
        }
    }
}
