using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestBackEnd.Models
{
	public class TipoCambio
	{
		public int IdTipoCambio { get; set; }

		public DateTime Fecha { get; set; }

		public double? Monto { get; set; }

		public DateTime FechaCreacion { get; set; }

		public DateTime FechaModificacion { get; set; }

	}
}