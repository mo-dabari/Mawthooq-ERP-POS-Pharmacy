using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Users;

namespace Mawthooq.Application.Users.UseCases.AddUserPhone
{
    public class AddUserPhoneUseCase
    {
        private readonly IUserRepository _userRepository;
        public AddUserPhoneUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public void Execute(AddUserPhoneRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            User? user = _userRepository.GetById(request.UserId);

            if (user is null)
                throw new KeyNotFoundException($"User with ID {request.UserId} was not found.");

                user.AddPhone(request.Phone.ToDomain());

                _userRepository.Update(user, request.rowVersion);
        }
    }
}
