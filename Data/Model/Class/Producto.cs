using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.Class
{
	[Table("PRODUCTO")]
	public class Producto
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int IdProducto { get; set; }

		public string Descripcion { get; set; }

		public double Stock { get; set; }

		public double PrecioCordoba { get; set; }

		public double PrecioDolar { get; set; }

		public double TipoCambio { get; set; }

		public DateTime? FechaCreacion { get; set; }

		public DateTime? FechaModificacion { get; set; }
	}
}
