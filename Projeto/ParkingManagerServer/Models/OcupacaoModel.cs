using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParkingManagerServer.Models
{
    public class OcupacaoModel
    {
        public OcupacaoModel() { }
        public OcupacaoModel(long id, VeiculoModel veiculo, UsuarioModel usuario, DateTime dataEntrada, DateTime dataSaida)
        {
            Id = id;
            Veiculo = veiculo;
            Usuario = usuario;
            DataEntrada = dataEntrada;
            DataSaida = dataSaida;

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id{get;set;}
        public virtual VeiculoModel Veiculo { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual UsuarioModel Usuario { get; set; }
        public DateTime DataEntrada{get;set;}

        public DateTime DataSaida{get;set;}
    }
}