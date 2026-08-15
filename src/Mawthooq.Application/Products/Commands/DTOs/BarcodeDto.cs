using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Application.Products.Commands.DTOs
{
    public sealed record BarcodeDto(string Value, BarcodeType Type);
}
