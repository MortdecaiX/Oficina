
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingManagerServer.Models
{
    public class VagaModel
    {
        public VagaModel()
        {

        }
        public VagaModel(long id, long numero, TipoVaga tipo,   PosicaoGeografica localizacao, int pavimento,  UsuarioModel responsavel)
        {
            Id = id;
            Numero = numero;
            Tipo = tipo;
            Localizacao = localizacao;
            Pavimento = pavimento;
            Responsavel = responsavel;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long Numero { get; set; }
        public TipoVaga Tipo { get; set; }
	    public virtual OcupacaoModel Ocupacao { get; set; }
        public PosicaoGeografica Localizacao { get; set; }
        public int Pavimento { get; set; }
        public virtual ReservaModel Reserva { get; set; }
        public virtual UsuarioModel Responsavel { get; set; }
    }
}