using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BenchmarkingDapperEFCoreCRMSqlServer.EFCore;

public class Empresa
{
    [Key]
    public int IdEmpresa { get; set; }
    public string? CNPJ { get; set; }
    public string? Nome { get; set; }
    public string? Cidade { get; set; }
    public List<Contato>? Contatos { get; set; }
}

public class Contato
{
    [Key]
    public int IdContato { get; set; }
    public string? Nome { get; set; }
    public string? Telefone { get; set; }

    [ForeignKey("IdEmpresa")]
    [JsonIgnore]
    public Empresa? EmpresaRepresentada { get; set; }
}