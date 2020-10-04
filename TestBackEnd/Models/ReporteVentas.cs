using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestBackEnd.Models
{
	public class ReporteVentas
	{
		public int IdCliente { get; set; }
		public string NombreCliente { get; set; }
		public int Anio { get; set; }
		public int Mes { get; set; }
		public double? TotalCordoba { get; set; }
		public double? TotalDolar { get; set; }
		public List<Factura> Facturas { get; set; }
	}
}