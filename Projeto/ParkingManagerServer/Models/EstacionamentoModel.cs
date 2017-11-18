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
        private PosicaoGeografica _SWBoundImagem = null;
        private PosicaoGeografica _NEBoundImagem = null;
        public PosicaoGeografica SWBoundImagem
        {
            get
            {
                if (_SWBoundImagem == null)
                {
                    return new PosicaoGeografica( Localizacao.Latitude, Localizacao.Longitude, Localizacao.Altitude);
                }
                else
                    return _SWBoundImagem;
            }
            set
            {
                _SWBoundImagem = value;
            }
        }
        public PosicaoGeografica NEBoundImagem
        {
            get
            {
                if (_NEBoundImagem == null)
                {
                    return new PosicaoGeografica(Localizacao.Latitude, Localizacao.Longitude, Localizacao.Altitude);
                }
                else
                    return _NEBoundImagem;
            }
            set
            {
                _NEBoundImagem = value;
            }
        }
        public virtual ICollection<PontoModel> Pontos { get; set; }
        public virtual UsuarioModel Responsavel { get; set; }
        public PosicaoGeografica Localizacao { get; set; }
    }
}