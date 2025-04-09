using AppO.Data;
using AppO.Models;
using AppO.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppO.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsFollowingAsync(string followerId, string followingId)
    {
        var follow = await _context.Follows
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

        return follow != null;
    }

    public async Task AddFollow(string followerId, string followingId)
    {
        Follow follow = new Follow
        {
            FollowerId = followerId,
            FollowingId = followingId,
            FollowedAt = DateTime.UtcNow
        };

        _context.Follows.Add(follow);
        await _context.SaveChangesAsync();
    }
    public async Task RemoveFollow(string followerId, string followingId)
    {
        var follow = _context.Follows.FirstOrDefault(f => f.FollowerId == followerId && f.FollowingId == followingId);

        if(follow != null)
        {
            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync();
        }
    }
}
