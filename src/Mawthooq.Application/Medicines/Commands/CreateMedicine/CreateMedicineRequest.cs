using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;
using Mawthooq.Application.Medicines.Commands.DTOs;
using Mawthooq.Domain.Medicines.Enums;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Application.Medicines.Commands.CreateMedicine
{
    public sealed record class CreateMedicineRequest(

        CreateProductRequest ProductRequest,

        CreateProductPackagingRequest PackagingRequest,

        string MedicineName,
        DosageFormType DosageForm,
        ReferenceQuantityDto ReferenceQuantity,
        int TherapeuticClassId,
        int ManufacturerId,
        IReadOnlyList<DrugCompositionDto> DrugCompositions
    );

    public sealed record CreateProductRequest(
        string Name,
        string SKU,
        CategoryIdDto CategoryId,
        UnitOfMeasure BaseUnit,
        bool RequiresExpiryTracking,
        bool CanBeReturnedIfOpened,
        int NearExpiryThresholdInDays,
        string? ImageUrl,
        BarcodeDto? Barcode
    );

    public sealed record CreateProductPackagingRequest(
        PackagingTemplate Template,
        int? TotalUnitsPerBox,
        UnitOfMeasure? DirectUnit,
        int? StripsPerBox,
        int? UnitsPerStrip,
        int? InnerBoxesPerOuterBox,
        int? PiecesPerInnerBox,
        UnitOfMeasure? SmallUnit
    );



}
