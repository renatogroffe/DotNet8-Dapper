using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIRegioes.Data;

public class Regiao
{
    [Key]
    public int IdRegiao { get; set; }
    public string? CodRegiao { get; set; }
    public string? NomeRegiao { get; set; }
    public List<Estado>? Estados { get; set; }
}

public class Estado
{
    [Key]
    public string? SiglaEstado { get; set; }
    public string? NomeEstado { get; set; }
    public string? NomeCapital { get; set; }
        
    [ForeignKey("IdRegiao")]
    [JsonIgnore]
    public Regiao? Regiao { get; set; }
}