using APSPA_BakendAndFrontend.Server.DTOs.Profile;
using APSPA_BakendAndFrontend.Server.Repositories.Interfaces;
using APSPA_BakendAndFrontend.Server.Services.Interfaces;

namespace APSPA_BakendAndFrontend.Server.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository _users;

        public ProfileService(IUserRepository users)
        {
            _users = users;
        }

        public async Task<ProfileResponseDto> GetAsync(int userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found.");

            return new ProfileResponseDto
            {
                Id        = user.Id,
                FirstName = user.FirstName,
                LastName  = user.LastName,
                Email     = user.Email,
                Age       = user.Age,
                Gender    = user.Gender,
                WeightKg  = user.WeightKg,
                HeightCm  = user.HeightCm,
                Role      = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<ProfileResponseDto> UpdateAsync(int userId, UpdateProfileDto dto)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found.");

            if (dto.FirstName != null) user.FirstName = dto.FirstName;
            if (dto.LastName != null) user.LastName = dto.LastName;
            if (dto.Age.HasValue) user.Age = dto.Age;
            if (dto.Gender != null) user.Gender = dto.Gender;
            if (dto.WeightKg.HasValue) user.WeightKg = dto.WeightKg;
            if (dto.HeightCm.HasValue) user.HeightCm = dto.HeightCm;
            user.UpdatedAt = DateTime.UtcNow;

            await _users.UpdateAsync(user);
            return await GetAsync(userId);
        }
    }
}
