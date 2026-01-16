using Common.Dtos.Commands;
using Common.Dtos.Responses;

namespace Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthResponseDto> LoginAsync(UserLoginDto request, CancellationToken cancellationToken = default);
    }
}
