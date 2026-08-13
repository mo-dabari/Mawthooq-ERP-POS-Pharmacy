using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Users;

namespace Mawthooq.Application.Users.UseCases.GetUserByID
{
    public class GetUserByIdUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public GetUserByIdResponse Execute(int userId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));
            
            User? user = _userRepository.GetById(userId);

            if (user is null)
                throw new KeyNotFoundException($"User with ID {userId} was not found.");

            return new GetUserByIdResponse(user.UserId, user.NationalId.ToDto(), user.FullName, user.Phone.ToDto(), user.DateOfBirth, user.Age, user.Role);
        }
    }
}
