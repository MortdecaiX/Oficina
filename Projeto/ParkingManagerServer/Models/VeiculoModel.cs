using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ParkingManagerServer.Models
{
    public class VeiculoModel
    {
        public VeiculoModel() { }
        public VeiculoModel(ICollection<UsuarioModel> usuarios, string placa, string marca, string modelo)
        {
            Usuarios = usuarios;
            Placa = placa;
            Marca = marca;
            Modelo = modelo;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<UsuarioModel> Usuarios { get; set; }

       
        public List<long> getIdsUsuarios()
        {
            
            {
                List<long> usuariosIds = new List<long>();
                if(Usuarios !=null)
                    foreach (var usuario in Usuarios)
                    {
                        usuariosIds.Add(usuario.Id);
                    }
                return usuariosIds;
            }
        }

        public string Placa { get; set; }
        public string Marca { get; set; }

        public string Modelo { get; set; }
}
}