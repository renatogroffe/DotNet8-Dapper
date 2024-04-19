using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using Slapper;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace APIRegioes.Data;

public class RegioesRepository
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public RegioesRepository(IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    private IDbConnection GetConnection()
    {
        var connection = new SqlConnection(
            _configuration.GetConnectionString("BaseDadosGeograficos"));
        
        if (_environment.IsDevelopment())
            return new ProfiledDbConnection(connection, MiniProfiler.Current);

        return connection;
    }

    public IEnumerable<Regiao> Get(string? codRegiao = null)
    {
        using var conexao = GetConnection();

        bool queryWithParameter = !String.IsNullOrWhiteSpace(codRegiao);
        var sqlCmd =
            "SELECT R.IdRegiao, " +
                    "R.CodRegiao, " +
                    "R.NomeRegiao, " +
                    "E.SiglaEstado AS Estados_SiglaEstado, " +
                    "E.NomeEstado AS Estados_NomeEstado, " +
                    "E.NomeCapital AS Estados_NomeCapital " +
            "FROM dbo.Regioes R " +
            "INNER JOIN dbo.Estados E " +
                "ON E.IdRegiao = R.IdRegiao " +
            (queryWithParameter ? $"WHERE (R.CodRegiao = @CodigoRegiao) " : String.Empty) +
            "ORDER BY R.NomeRegiao, E.NomeEstado";

        object? paramQuery = null;
        if (queryWithParameter)
            paramQuery = new { CodigoRegiao = codRegiao };
        var dados = conexao.Query<dynamic>(sqlCmd, paramQuery);

        AutoMapper.Configuration.AddIdentifier(
            typeof(Regiao), "IdRegiao");
        AutoMapper.Configuration.AddIdentifier(
            typeof(Estado), "SiglaEstado");

        return (AutoMapper.MapDynamic<Regiao>(dados)
            as IEnumerable<Regiao>).ToArray();
    }

    public IEnumerable<Regiao> GetV2(string? codRegiao = null)
    {
        using var conexao = GetConnection();

        bool queryWithParameter = !String.IsNullOrWhiteSpace(codRegiao);
        var sqlCmd =
            "SELECT * " +
            "FROM dbo.Regioes " +
            (queryWithParameter ? $"WHERE (CodRegiao = @CodigoRegiao) " : String.Empty) +
            "ORDER BY NomeRegiao";

        object? paramQuery = null;
        if (queryWithParameter)
            paramQuery = new { CodigoRegiao = codRegiao };
        var dados = conexao.Query<Regiao>(sqlCmd, paramQuery);

        foreach (var regiao in dados)
            regiao.Estados = conexao.Query<Estado>(
                "SELECT * FROM dbo.Estados " +
                "WHERE IdRegiao = @IdRegiao",
                new { IdRegiao = regiao.IdRegiao }).ToList();

        return dados;
    }
}