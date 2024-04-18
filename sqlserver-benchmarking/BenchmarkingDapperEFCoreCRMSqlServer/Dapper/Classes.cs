using Dapper.Contrib.Extensions;

namespace BenchmarkingDapperEFCoreCRMSqlServer.Dapper;

[Table("dbo.Empresas")]
public class Empresa
{
    [Key]
    public int IdEmpresa { get; set; }
    public string? CNPJ { get; set; }
    public string? Nome { get; set; }
    public string? Cidade { get; set; }

    [Write(false)]
    public List<Contato>? Contatos { get; set; }
}

[Table("dbo.Contatos")]
public class Contato
{
    [Key]
    public int IdContato { get; set; }
    public string? Nome { get; set; }
    public string? Telefone { get; set; }
    public int IdEmpresa { get; set; }
}