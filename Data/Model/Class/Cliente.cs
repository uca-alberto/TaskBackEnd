using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.Class
{
	[Table("CLIENTE")]
	public class Cliente
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int IdCliente { get; set; }

		public string Nombre { get; set; }

		public string EstadoCivil { get; set; }

		public string Direccion { get; set; }

		public int Edad { get; set; }

		public string Correo { get; set; }

		public DateTime? FechaCreacion { get; set; }

		public DateTime? FechaModificacion { get; set; }

		public virtual List<Factura> facturaLinea { get; set; }
	}
}
