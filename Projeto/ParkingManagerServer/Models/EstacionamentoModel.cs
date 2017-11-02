using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ParkingManagerServer.Models
{
    public class EstacionamentoModel
    {
        [Key]
        public long Id { get; set; }
        public float Zoom { get; set; }
        public string Nome { get; set; }
        public string ImagemBase64 { get; set; }
        public string ImagemURL { get; set; }
        public PosicaoGeografica SWBoundImagem { get; set; }
        public PosicaoGeografica NEBoundImagem { get; set; }
        public virtual ICollection<PontoModel> Pontos { get; set; }
        public virtual UsuarioModel Responsavel { get; set; }
        public PosicaoGeografica Localizacao { get; set; }
    }
}