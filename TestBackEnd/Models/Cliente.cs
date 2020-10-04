using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestBackEnd.Models
{
	public class Cliente
	{
		public int IdCliente { get; set; }

		public string Nombre { get; set; }

		public string EstadoCivil { get; set; }

		public string Direccion { get; set; }

		public int? Edad { get; set; }

		public string Correo { get; set; }

		public DateTime FechaCreacion { get; set; }

		public DateTime FechaModificacion { get; set; }

	}
}