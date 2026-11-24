using SQLite;

namespace Agrosens.Models
{
    [Table("sensores")]
    public class Sensor
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, MaxLength(50)]
        public string Tipo { get; set; } = string.Empty; // Umidade, Porteira, Agua, Silagem

        [NotNull, MaxLength(200)]
        public string Nome { get; set; } = string.Empty;

        [NotNull, MaxLength(100)]
        public string Codigo { get; set; } = string.Empty;

        [NotNull, MaxLength(200)]
        public string Localizacao { get; set; } = string.Empty;

        public int? LimiteAlerta { get; set; }

        public string? TipoMedicao { get; set; } // Solo / Ar (Umidade)

        public string? CategoriaPorteira { get; set; } // Pasto de gado / Cercado de porcos / Acesso geral / Outro

        public bool AlertaPorteiraAberta { get; set; }

        public int? MinutosAberta { get; set; }

        public double? CapacidadeLitros { get; set; } // Água do gado (opcional)

        public string? TipoSilagem { get; set; } // Silo / Cocho / Galpão de ração

        public double? ValorAtual { get; set; } // última leitura em percentual (%)

        public bool? PorteiraAberta { get; set; } // último estado da porteira (aberta/fechada)

        public bool Notificacoes { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
