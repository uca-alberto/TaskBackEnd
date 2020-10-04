using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model.Class
{
	[Table("FACTURA")]
	public class Factura
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int IdFactura { get; set; }

		[ForeignKey("Cliente")]
		public int IdCliente { get; set; }

		public string Direcion { get; set; }

		public double IvaCordoba { get; set; }

		public double IvaDolar { get; set; }

		public double TotalCordoba { get; set; }

		public double TotalDolar { get; set; }

		public DateTime? FechaCreacion { get; set; }

		public virtual Cliente Cliente { get; set; }

		public virtual List<FacturaLinea> FacturaLinea { get; set; }
	}
}
