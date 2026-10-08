namespace Project.Models;

public class RecuperacaoSenhaModel{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Codigo { get; set; }
    public DateTime DataExpiracao { get; set; }
}
