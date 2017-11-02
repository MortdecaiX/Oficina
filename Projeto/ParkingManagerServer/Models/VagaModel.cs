
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingManagerServer.Models
{
    public class VagaModel
    {
        public VagaModel()
        {

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
    }
}