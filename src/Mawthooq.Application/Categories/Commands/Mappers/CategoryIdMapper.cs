using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;
using Mawthooq.Domain.Categories.ValueObjects;

namespace Mawthooq.Application.Categories.Commands.Mappers
{
    public static class CategoryIdMapper
    {
        public static CategoryId ToDomain(this CategoryIdDto categoryIdDto)
        {
            ArgumentNullException.ThrowIfNull(categoryIdDto, nameof(categoryIdDto));

            return CategoryId.From(categoryIdDto.Value);
        }

        public static CategoryIdDto ToDto(this CategoryId categoryId)
        {
            return new CategoryIdDto(categoryId.Value);
        }
    }
}
