using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBackEnd.Models
{
	public class FacturaLinea
	{
		public int IdFacturaLinea { get; set; }

		public int IdFactura { get; set; }

		public int IdArticulo { get; set; }

		public string DescripcionArticulo { get; set; }

		public double Cantidad { get; set; }

		public double PrecioCordoba { get; set; }

		public double PrecioDolar { get; set; }

		public double TotalCordoba { get; set; }

		public double TotalDolar { get; set; }
	}
}
