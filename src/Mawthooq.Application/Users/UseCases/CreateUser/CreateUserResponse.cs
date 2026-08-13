using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Domain.Users.Enums;

namespace Mawthooq.Application.Users.UseCases.CreateUser
{
    public record class CreateUserResponse (int UserId , NationalIdDto NationalId, string FullName, PhoneDto PhoneNumber, DateOnly DateOfBirth, byte Age, Role Role)
    {

    }
}
