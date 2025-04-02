using Microsoft.AspNetCore.Identity;

namespace AppO.Models;

public class appUser: IdentityUser
{
    public string? Name { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Biography { get; set; }
}
