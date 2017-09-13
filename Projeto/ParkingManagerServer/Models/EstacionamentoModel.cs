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

        public string Nome { get; set; }
        public string ImagemBase64 { get; set; }
        public long ImagemAltura { get; set; }
        public long ImagemLargura { get; set; }
        public float ImagemRotacao { get; set; }
        public PosicaoGeografica LocalizacaoImagem { get; set; }
        public virtual ICollection<PontoModel> Pontos { get; set; }
        public virtual UsuarioModel Responsavel { get; set; }
        public PosicaoGeografica Localizacao { get; set; }
        


    }
}