using Microsoft.AspNetCore.Mvc;
using APIEstados.Data;
using APIEstados.Models;

namespace APIEstados.Controllers;

[ApiController]
[Route("[controller]")]
public class EstadosController : ControllerBase
{
    private readonly ILogger<EstadosController> _logger;
    private readonly EstadosRepository _repository;

    public EstadosController(ILogger<EstadosController> logger,
        EstadosRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<Estado>> GetAll()
    {
        var dados = await _repository.Get();
        _logger.LogInformation(
            $"{nameof(GetAll)}: {dados.Count()} registro(s) encontrado(s)");
        return dados;
    }

    [HttpGet("{codEstado}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<ActionResult<Estado>> Get(string codEstado)
    {
        var dados = await _repository.Get(codEstado);
        if (dados.Count() == 1)
        {
            _logger.LogInformation(
                $"{nameof(Get)}: encontrados dados para o Estado com o codigo '{codEstado}'.");
            return dados.First();
        }
        else
        {
            _logger.LogError(
                $"{nameof(Get)}: Mao foi encontrado um Estado com o codigo '{codEstado}'!");
            return NotFound();
        }

    }
}