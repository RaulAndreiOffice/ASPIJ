using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using APSPA_BakendAndFrontend.Server.Configuration;
using APSPA_BakendAndFrontend.Server.Data;
using APSPA_BakendAndFrontend.Server.DTOs.Auth;
using APSPA_BakendAndFrontend.Server.model;
using APSPA_BakendAndFrontend.Server.Repositories.Interfaces;
using APSPA_BakendAndFrontend.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace APSPA_BakendAndFrontend.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwt;

        public AuthService(IUserRepository users, AppDbContext context, IOptions<JwtSettings> jwt)
        {
            _users = users;
            _context = context;
            _jwt = jwt.Value;
        }

        public async Task<LoginResponseDto> RegisterAsync(RegisterDto dto)
        {
            if (await _users.GetByEmailAsync(dto.Email) != null)
                throw new ArgumentException("Email already registered.");

            var user = new User
            {
                FirstName    = dto.FirstName,
                LastName     = dto.LastName,
                Email        = dto.Email.ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Age          = dto.Age,
                Gender       = dto.Gender,
                WeightKg     = dto.WeightKg,
                HeightCm     = dto.HeightCm,
                Role         = "User",
                CreatedAt    = DateTime.UtcNow
            };

            await _users.CreateAsync(user);
            return await GenerateTokensAsync(user);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _users.GetByEmailAsync(dto.Email)
                ?? throw new ArgumentException("Invalid email or password.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new ArgumentException("Invalid email or password.");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("Account is disabled.");

            return await GenerateTokensAsync(user);
        }

        public async Task<LoginResponseDto> RefreshAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token == refreshToken)
                ?? throw new ArgumentException("Invalid refresh token.");

            if (token.IsRevoked || token.ExpiresAt < DateTime.UtcNow)
                throw new ArgumentException("Refresh token expired or revoked.");

            // Revoke old token
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return await GenerateTokensAsync(token.User);
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found.");

            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
                throw new ArgumentException("Current password is incorrect.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _users.UpdateAsync(user);
        }

        // ------------------------------------------------------------------

        private async Task<LoginResponseDto> GenerateTokensAsync(User user)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpiryMinutes);
            var accessToken = CreateJwt(user, expiresAt);
            var refreshToken = await CreateRefreshTokenAsync(user.Id);

            return new LoginResponseDto
            {
                AccessToken  = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt    = expiresAt,
                Email        = user.Email,
                Role         = user.Role
            };
        }

        private string CreateJwt(User user, DateTime expiresAt)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer:   _jwt.Issuer,
                audience: _jwt.Audience,
                claims:   claims,
                expires:  expiresAt,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> CreateRefreshTokenAsync(int userId)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            _context.RefreshTokens.Add(new RefreshToken
            {
                UserId    = userId,
                Token     = token,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpiryDays),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            });

            await _context.SaveChangesAsync();
            return token;
        }
    }
}
