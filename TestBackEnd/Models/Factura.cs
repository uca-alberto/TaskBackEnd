using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestBackEnd.Models
{
	public class Factura
	{
		public int IdFactura { get; set; }

		public int IdCliente { get; set; }

		public string NombreCliente { get; set; }

		public string Direcion { get; set; }

		public DateTime FechaCreacion { get; set; }

		public double IvaCordoba { get; set; }

		public double IvaDolar { get; set; }

		public double TotalCordoba { get; set; }

		public double TotalDolar { get; set; }

		public List<FacturaLinea> FacturaLinea { get; set; }

	}
}
