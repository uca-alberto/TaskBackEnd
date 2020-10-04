using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.Class
{
	[Table("TIPOCAMBIO")]
	public class TipoCambio
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int IdTipoCambio { get; set; }

		public DateTime? Fecha { get; set; }

		public double Monto { get; set; }

		public DateTime? FechaCreacion { get; set; }

		public DateTime? FechaModificacion { get; set; }
	}
}
