using LinqToDB.Mapping;

namespace ConsoleAppDadosEmpresas.Data;

[Table("Empresas")]
public class Empresa
{
    [PrimaryKey, Identity]
    public int Id { get; set; }

    [Column]
    public string? Nome { get; set; }

    [Column]
    public string? Contato { get; set; }

    [Column]
    public string? Cidade { get; set; }

    [Column]
    public string? Pais { get; set; }
}