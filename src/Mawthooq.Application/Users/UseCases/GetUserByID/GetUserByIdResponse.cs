using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Domain.Users.Enums;

namespace Mawthooq.Application.Users.UseCases.GetUserByID
{
    public record class GetUserByIdResponse(int UserId , NationalIdDto NationalId, string FullName, PhoneDto PhoneNumber, DateOnly DateOfBirth, byte Age, Role Role)
    {

    }
}
