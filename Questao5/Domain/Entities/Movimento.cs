namespace Questao5.Domain.Entities;

public class Movimento
{
    public int Id { get; set; }
    public int IdContaCorrente { get; set; }
    public DateTime Data { get; set; }
    public char Tipo { get; set; }
    public double Valor { get; set; }
}
