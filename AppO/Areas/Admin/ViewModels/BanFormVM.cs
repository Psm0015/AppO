using System.ComponentModel.DataAnnotations;

namespace AppO.Areas.Admin.ViewModels;

public class BanFormVM
{
    [Required(ErrorMessage ="Por Favor Informe o Motivo da Banimento")]
    [Display(Name ="Motivo do Banimento")]
    [StringLength(100, MinimumLength =3, ErrorMessage = "O {0} precisa ter no Mínimo {1} e no Maximo {2} caracteres")]
    public string Motivo { get; set; }
    public string BannedUserId { get; set; }
    [Display(Name ="Permanente?")]
    public bool IsPermanente { get; set; }
    [Display(Name = "Fim do Banimento")]
    public DateTime? DataFim { get; set; }
}
