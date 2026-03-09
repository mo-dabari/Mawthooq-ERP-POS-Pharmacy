using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Users;

namespace Mawthooq.Application.Users.UseCases.CreateUser
{
    public class CreateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        public CreateUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public CreateUserResponse Execute(CreateUserRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            User user = User.Create(request.NationalId.ToDomain(), request.FirstName, request.MiddleName, request.LastName, request.PhoneNumber.ToDomain(), request.DateOfBirth, request.Role);
            User savedUser = _userRepository.Add(user);
            return new CreateUserResponse(savedUser.UserId, savedUser.NationalId.ToDto(), savedUser.FullName, savedUser.Phone.ToDto(), savedUser.DateOfBirth, savedUser.Age, savedUser.Role);
        }
    }
}
