using AppO.Models;

namespace AppO.ViewModels;

public class UserViewModel
{
    public ProfileViewModel? User { get; set; }
    public bool Following { get; set; }
}
