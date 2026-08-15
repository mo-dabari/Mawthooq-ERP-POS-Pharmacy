using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Users;

namespace Mawthooq.Application.Users.UseCases.UpdateUser
{
    public class UpdateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UpdateUserResponse Execute(UpdateUserRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            User? user = _userRepository.GetById(request.UserId);

            if (user is null)
                throw new KeyNotFoundException($"User with ID {request.UserId} was not found.");
            user.ChangeRole(request.Role);

            _userRepository.Update(user, request.rowVersion);
            return new UpdateUserResponse(user.UserId, request.PhoneNumber, user.Role);
        }
    }
}
