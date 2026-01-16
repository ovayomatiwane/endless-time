using AutoMapper;
using Common.Dtos;
using Common.Dtos.Commands;
using Common.Dtos.Responses;
using Common.Exceptions;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Security;

namespace Services
{
    public class UserService(
        ApplicationDataContext databaseContext,
        IMapper mapper,
        IPasswordHasher passwordHasher,
        IAuthenticationService authenticationService) : IUserService
    {
        public async Task<AuthResponseDto> AuthenticateUserAsync(UserLoginDto userLoginDetails, CancellationToken cancellationToken = default)
        {
            ValidateUserLoginDto(userLoginDetails);
            var response = await authenticationService.LoginAsync(userLoginDetails, cancellationToken);

            return response;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUser, CancellationToken cancellationToken = default)
        {
            ValidateCreateUserDto(createUser);

            var user = await databaseContext.Users.SingleOrDefaultAsync(x => x.Email == createUser.Email, cancellationToken);

            if (user is not null)
            {
                string message = $"User with email address: {createUser.Email} already exists.";
                throw new Exception(message);
            }

            Guid userId = Guid.NewGuid();

            string passwordHash = passwordHasher.Hash(createUser.Password);
            string lowercaseEmail = createUser.Email.ToLowerInvariant();

            User newUser = new()
            {
                Id = Guid.NewGuid(),
                Name = createUser.Name,
                Surname = createUser.Surname,
                Email = lowercaseEmail,
                PasswordHash = passwordHash
            };

            databaseContext.Users.Add(newUser);

            await databaseContext.SaveChangesAsync(cancellationToken);
            
            return mapper.Map<UserDto>(newUser);
        }

        private void ValidateCreateUserDto(CreateUserDto createUser)
        {
            string message;

            if (createUser is null)
            {
                message = $"Null argument {nameof(createUser)}.";
                throw new ArgumentNullException(nameof(createUser), message);
            }

            if (string.IsNullOrEmpty(createUser.Name))
            {
                message = $"Invalid name provided. Name cannot be null or empty.";
                throw new RequiredNullOrEmptyStringException(message);
            }

            if (string.IsNullOrEmpty(createUser.Surname))
            {
                message = $"Invalid name provided. Surname cannot be null or empty.";
                throw new RequiredNullOrEmptyStringException(message);
            }

            if (string.IsNullOrEmpty(createUser.Email))
            {
                message = $"Invalid email provided. Email cannot be null or empty.";
                throw new RequiredNullOrEmptyStringException(message);
            }

            if (string.IsNullOrEmpty(createUser.Password))
            {
                message = $"Invalid password provided. Password cannot be null or empty.";
                throw new RequiredNullOrEmptyStringException(message);
            }
        }

        private void ValidateUserLoginDto(UserLoginDto userLogin)
        {
            string message;

            if (userLogin is null)
            {
                message = $"Null argument {nameof(userLogin)}.";
                throw new ArgumentNullException(nameof(userLogin), message);
            }

            if (string.IsNullOrEmpty(userLogin.UserName))
            {
                message = $"Invalid UserName provided. UserName cannot be null or empty.";
                throw new RequiredNullOrEmptyStringException(message);
            }

            if (string.IsNullOrEmpty(userLogin.Password))
            {
                message = $"Invalid Password provided. Password cannot be null or empty.";
                throw new RequiredNullOrEmptyStringException(message);
            }
        }
    }
}
