using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Domain.Users.Enums;

namespace Mawthooq.Application.Users.UseCases.CreateUser
{
    public record class CreateUserRequest (
        NationalIdDto NationalId,
        string FirstName,
        string MiddleName,
        string LastName,
        PhoneDto PhoneNumber,
        DateOnly DateOfBirth,
        Role Role
    )
    {
    }
}
