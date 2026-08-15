using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Medicines.Commands.DTOs;
using Mawthooq.Domain.Medicines.Enums;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Application.Medicines.Commands.CreateMedicine
{
    public sealed record class CreateMedicineResponse(

        CreateProductResponseDto ProductResponse,

        string MedicineName,
        DosageFormType DosageForm,
        ReferenceQuantityDto ReferenceQuantity,
        int TherapeuticClassId,
        int ManufacturerId,
        IReadOnlyList<DrugCompositionDto> DrugCompositions
        );

    public record CreateProductResponseDto(
        int ProductId,
        string Name,
        string SKU,
        int CategoryId,
        UnitOfMeasure BaseUnit,
        bool RequiresExpiryTracking,
        bool CanBeReturnedIfOpened,
        int NearExpiryThresholdInDays,
        string? ImageUrl,
        BarcodeDto? Barcode,
        IReadOnlyList<CreateProductUnitDetailsDto> UnitDetails
    );

    public record CreateProductUnitDetailsDto(
        UnitOfMeasure Unit,
        int ConversionRate,
        UnitRank Rank,
        BarcodeDto? UnitBarcode
    );
}
