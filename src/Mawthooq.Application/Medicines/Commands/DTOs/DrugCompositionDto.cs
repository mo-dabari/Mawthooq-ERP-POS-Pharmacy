using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Medicines.Enums;

namespace Mawthooq.Application.Medicines.Commands.DTOs
{
    public sealed record class DrugCompositionDto(
        int MedicineId,
        int ActiveIngredientId,
        IngredientAmountDto IngredientAmount);

    public sealed record IngredientAmountDto(
        decimal Amount,
        IngredientAmountUnits Unit);

}
