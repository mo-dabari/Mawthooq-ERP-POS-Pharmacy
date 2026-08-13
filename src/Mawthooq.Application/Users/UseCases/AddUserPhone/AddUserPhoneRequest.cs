using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Users.UseCases.AddUserPhone
{
    public record class AddUserPhoneRequest(int UserId , PhoneDto Phone)
    {

    }
}
