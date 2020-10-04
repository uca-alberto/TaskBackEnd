using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestBackEnd.Models
{
	public class Filtros
	{
		public int? IdCliente { get; set; }
		public string NombreCliente { get; set; }
		public int? Anio { get; set; }
		public int? Mes { get; set; }
	}
}