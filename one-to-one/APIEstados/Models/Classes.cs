namespace APIEstados.Models
{
    public class Estado
    {
        public string? SiglaEstado { get; set; }
        public string? NomeEstado { get; set; }
        public string? NomeCapital { get; set; }
        public Regiao? DadosRegiao { get; set; }
    }

    public class Regiao
    {
        public int? IdRegiao { get; set; }
        public string? NomeRegiao { get; set; }
    }
}