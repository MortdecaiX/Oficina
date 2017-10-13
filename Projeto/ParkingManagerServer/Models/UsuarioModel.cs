using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ParkingManagerServer.Models
{
    public class UsuarioModel
    {
        public UsuarioModel()
        {

        }
        public UsuarioModel(string nome, string sobrenome, string email, string senha, string cpf, ICollection<VeiculoModel> veiculos)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            Email = email;
            Senha = senha;
            CPF = cpf;
            Veiculos = veiculos;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string CPF { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<VeiculoModel> Veiculos { get; set; }
        
        public List<long> getIdsVeiculos()  {
                List<long> veiculosIds = new List<long>();
                if(Veiculos !=null)
                    foreach(var veiculo in Veiculos)
                    {
                        veiculosIds.Add(veiculo.Id);
                    }
                return veiculosIds;
            } 
    }
}