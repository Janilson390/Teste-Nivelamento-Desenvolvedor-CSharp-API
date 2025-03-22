namespace Questao5.DTOs;

public class MovimentacaoRequest
{
    public string RequestId { get; set; }
    public int ContaId { get; set; }
    public decimal Valor { get; set; }
    public string Tipo { get; set; } // "C" para crédito, "D" para débito
}
