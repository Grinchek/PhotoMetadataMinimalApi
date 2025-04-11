using Microsoft.EntityFrameworkCore;
using PhotoMetadataMinimalApi.Data;
using PhotoMetadataMinimalApi.Models;

namespace PhotoMetadataMinimalApi.Repositories;

public class UserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<bool> ExistsAsync(string nickname) =>
        _db.Users.AnyAsync(u => u.Nickname == nickname);

    public Task<User?> GetByNicknameAsync(string nickname) =>
        _db.Users.FirstOrDefaultAsync(u => u.Nickname == nickname);

    public async Task<User> AddAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }
}
