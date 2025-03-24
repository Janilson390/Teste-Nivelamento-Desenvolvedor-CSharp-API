using Questao5.Domain.Entities;

namespace Questao5.DTOs;

public class MovimentacaoRequest
{
    public int Id { get; set; }
    public int IdContacorrente { get; set; }
    public double Valor { get; set; }
    public char Tipo { get; set; }
    public string? Resultado { get; set; }
}
