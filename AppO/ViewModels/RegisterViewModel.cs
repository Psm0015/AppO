using System.ComponentModel.DataAnnotations;

namespace AppO.ViewModels;

public class RegisterViewModel
{
    [Required (ErrorMessage ="Informe um nome de Usuário")]
    [Display(Name ="Nome de Usuário")]
    [StringLength (30, MinimumLength =4, ErrorMessage ="O Nome de Usuário não cumpre os requisitos")]
    public string Username { get; set; }
    public string Password { get; set; }
    public string CPassword { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Biography { get; set; }

}
