namespace AppO.Repositories.Interfaces;

public interface IUserRepository
{
    Task<bool> IsFollowingAsync(string followerId, string followingId);
    Task AddFollow(string followerId, string followingId);
    Task RemoveFollow(string followerId, string followingId);
}
