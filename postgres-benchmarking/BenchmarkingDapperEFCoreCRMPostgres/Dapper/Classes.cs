namespace BenchmarkingDapperEFCoreCRMPostgres.Dapper;

public class Empresa
{
    public int IdEmpresa { get; set; }
    public string? CNPJ { get; set; }
    public string? Nome { get; set; }
    public string? Cidade { get; set; }
    public List<Contato>? Contatos { get; set; }
}

public class Contato
{
    public int IdContato { get; set; }
    public string? Nome { get; set; }
    public string? Telefone { get; set; }
    public int IdEmpresa { get; set; }
}