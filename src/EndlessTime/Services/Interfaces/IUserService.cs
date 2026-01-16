using Common.Dtos;
using Common.Dtos.Commands;
using Common.Dtos.Responses;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto user, CancellationToken cancellationToken = default);

        Task<AuthResponseDto> AuthenticateUserAsync(UserLoginDto userLoginDetails, CancellationToken cancellationToken = default);
    }
}
