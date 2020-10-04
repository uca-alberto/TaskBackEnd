using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.Class
{
	[Table("FACTURALINEA")]
	public class FacturaLinea
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int IdLinea { get; set; }

		[ForeignKey("Factura")]
		public int IdFactura { get; set; }

		[ForeignKey("Producto")]
		public int IdArticulo { get; set; }

		public double Cantidad { get; set; }

		public double PrecioCordoba { get; set; }

		public double PrecioDolar { get; set; }

		public double TotalCordoba { get; set; }

		public double TotalDolar { get; set; }

		public virtual Factura Factura { get; set; }

		public virtual Producto Producto { get; set; }

	}
}
