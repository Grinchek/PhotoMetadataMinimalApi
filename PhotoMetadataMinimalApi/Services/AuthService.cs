using Microsoft.AspNetCore.Identity;
using PhotoMetadataMinimalApi.Dtos;
using PhotoMetadataMinimalApi.Models;
using PhotoMetadataMinimalApi.Repositories;

namespace PhotoMetadataMinimalApi.Services;

public class AuthService
{
    private readonly UserRepository _userRepo;
    private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public AuthService(UserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<User?> RegisterAsync(RegisterUserDto dto)
    {
        if (await _userRepo.ExistsAsync(dto.Nickname)) return null;

        var user = new User
        {
            Nickname = dto.Nickname,
            Domain = dto.Domain
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
        return await _userRepo.AddAsync(user);
    }

    public async Task<User?> LoginAsync(LoginUserDto dto)
    {
        var user = await _userRepo.GetByNicknameAsync(dto.Nickname);
        if (user == null) return null;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        return result == PasswordVerificationResult.Success ? user : null;
    }
}
