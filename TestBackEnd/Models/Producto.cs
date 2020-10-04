using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestBackEnd.Models
{
	public class Producto
	{
		public int IdProducto { get; set; }

		public string Descripcion { get; set; }

		public double? Stock { get; set; }

		public double? PrecioCordoba { get; set; }

		public double? PrecioDolar { get; set; }

		public double? TipoCambio { get; set; }

		public DateTime FechaCreacion { get; set; }

		public DateTime FechaModificacion { get; set; }

	}
}