using System.Net;
using Microsoft.AspNetCore.Mvc;
using APIRegioes.Data;

namespace APIRegioes.Controllers;

[ApiController]
[Route("[controller]")]
public class RegioesDapperV2Controller : ControllerBase
{
    private readonly ILogger<RegioesDapperV2Controller> _logger;
    private readonly RegioesRepository _repository;

    public RegioesDapperV2Controller(ILogger<RegioesDapperV2Controller> logger,
        RegioesRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public IEnumerable<Regiao> GetRegioes()
    {
        var dados = _repository.GetV2();

        _logger.LogInformation(
            $"{nameof(GetRegioes)}: {dados.Count()} registro(s) encontrado(s)");

        return dados;
    }

    [HttpGet("{codRegiao}/Estados")]
    [ProducesResponseType(typeof(Regiao), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public ActionResult<Regiao> GetEstadosPorRegiao(string codRegiao)
    {
        var dados = _repository.GetV2(codRegiao)
            .SingleOrDefault();
        _logger.LogInformation(
            $"{nameof(GetEstadosPorRegiao)}: {dados?.Estados?.Count() ?? 0} registro(s) encontrado(s)");

        if (dados is null)
            return NotFound();
        
        return dados;
    }
}