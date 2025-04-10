namespace AppO.ViewModels;

public class ProfileViewModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? UserName { get; set; }
    public string? Biography { get; set; }
    public byte[]? UserImage { get; set; }
    public byte[]? BannerImage { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
}
