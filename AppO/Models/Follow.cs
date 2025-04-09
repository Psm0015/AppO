using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppO.Models;

[Table("Follows")]
public class Follow
{
    public int Id { get; set; }
    public string FollowerId { get; set; }
    public virtual appUser Follower { get; set; }
    public string FollowingId { get; set; }
    public virtual appUser Following { get; set; }
    public DateTime FollowedAt { get; set; }

}
