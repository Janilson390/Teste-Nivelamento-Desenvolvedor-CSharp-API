using Castle.Core;
using IdempotentAPI.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Questao5.DTOs;
using Microsoft.Data.Sqlite;
using Dapper;
using Questao5.Domain.Entities;
using Newtonsoft.Json;
using IdempotentAPI.Helpers;
using System.Data;

namespace Questao5.Controllers;

[ApiController]
[Route("api/[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
[Idempotent(Enabled = true)]
public class MovimentacaoController : Controller
{
    private readonly IDbConnection _db;

    public MovimentacaoController()
    {
        _db = new SqliteConnection("Data Source=database.db");
    }

    [HttpPost]
    public IActionResult Post([FromBody] MovimentacaoRequest request)
    {
        // Verifica idempotência
        var idempotencia = _db.QueryFirstOrDefault<Idempotencia>("SELECT * FROM idempotencia WHERE chave_idempotencia = @Chave", new { Chave = request.IdRequisicao });
        if (idempotencia != null)
            return Ok();

        // Validações de negócio
        var conta = _db.QueryFirstOrDefault<ContaCorrente>("SELECT * FROM contacorrente WHERE idcontacorrente = @Id", new { Id = request.IdContaCorrente });
        if (conta == null)
            return BadRequest(new { erro = "Conta inválida", tipo = "INVALID_ACCOUNT" });
        if (conta.Ativo == 0)
            return BadRequest(new { erro = "Conta inativa", tipo = "INACTIVE_ACCOUNT" });
        if (request.Valor <= 0)
            return BadRequest(new { erro = "Valor inválido", tipo = "INVALID_VALUE" });
        if (request.Tipo != 'C' && request.Tipo != 'D')
            return BadRequest(new { erro = "Tipo inválido", tipo = "INVALID_TYPE" });

        // Inserção do movimento                   
        var idMovimento = Guid.NewGuid().ToString();
        _db.Execute("INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@Id, @Conta, @Data, @Tipo, @Valor)",
            new { Id = idMovimento, Conta = request.IdContacorrente, Data = DateTime.UtcNow.ToString("dd/MM/yyyy"), Tipo = request.Tipo, Valor = request.Valor });

        //var resultado = new JsonSerializer.Serialize(new { idmovimento = idMovimento });
        _db.ExecuteScalar("INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) VALUES (@Chave, @Requisicao, @Resultado)",
            new { Chave = request.Id, Requisicao = request.IdContacorrente, Resultado = request.Tipo });

        return Ok(new { idmovimento = idMovimento });
    }
}
