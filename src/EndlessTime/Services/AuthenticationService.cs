using Common.Dtos.Commands;
using Common.Dtos.Responses;
using Common.Exceptions;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using Services.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDataContext _context;
        private readonly IConfiguration _configuration;
        private IPasswordHasher _passwordHasher;

        public AuthenticationService(ApplicationDataContext context, IConfiguration configuration, IPasswordHasher passwordHasher)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResponseDto> LoginAsync(UserLoginDto request, CancellationToken cancellationToken = default)
        {
            var lowercaseUsername = request.UserName.ToLowerInvariant();
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == lowercaseUsername, cancellationToken);

            string message; 
            if (user is null)
            {
                message = $"User with email {request.UserName} not found.";
                throw new EntityNotFoundException(message);
            }

            if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                message = $"Incorrect password entered for user with email: {request.UserName}";
                throw new UnauthorizedException(message);
            }
                

            var token = GenerateJwt(user);

            return new AuthResponseDto
            {
                AccessToken = token.Token,
                ExpiresAt = token.ExpiresAt,
            };
        }

        private (string Token, DateTime ExpiresAt) GenerateJwt(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(
                int.Parse(jwtSettings["ExpiryMinutes"]!)
            );

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return (
                new JwtSecurityTokenHandler().WriteToken(token),
                expires
            );
        }
    }
}
