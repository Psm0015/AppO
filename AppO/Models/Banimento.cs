namespace AppO.Models;

public class Banimento
{
    public int Id { get; set; }
    public string UsuarioId { get; set; }
    public virtual appUser Usuario { get; set; }
    public string AdminId { get; set; }
    public virtual appUser Admin { get; set; }
    public string Motivo { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool IsPermanente { get; set; }
    public DateTime? DataDesbanimento { get; set; }
    public string? AdminDesbanimentoId { get; set; }
    public virtual appUser? AdminDesbanimento { get; set; }
    public string? DataDesbanimentoDesbanimento { get; set; }
}
