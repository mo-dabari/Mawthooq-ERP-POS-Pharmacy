using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Domain.Users.Enums;

namespace Mawthooq.Application.Users.UseCases.UpdateUser
{
    public record class UpdateUserResponse (int UserId, PhoneDto PhoneNumber, Role Role)
    {

    }
}
