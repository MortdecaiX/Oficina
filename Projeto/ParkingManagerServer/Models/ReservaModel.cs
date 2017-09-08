using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingManagerServer.Models
{
    public class ReservaModel
    {
        public ReservaModel() { }
        public ReservaModel(VeiculoModel veiculo, UsuarioModel usuario, DateTime data, DateTime dataEntrada, DateTime dataExpiracao, DateTime dataSaida)
        {
            Veiculo = veiculo;
            Data = data;
            DataEntrada = dataEntrada;
            DataExpiracao = dataExpiracao;
            DataSaida = dataSaida;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual VeiculoModel Veiculo { get; set; }

        public virtual UsuarioModel Usuario { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime DataExpiracao { get; set; }
        public DateTime DataSaida { get; set; }
    }
}