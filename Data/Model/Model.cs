namespace Data.Model
{
	using Data.Model.Class;
	using System;
	using System.Data.Entity;
	using System.Linq;

	public class Model : DbContext
	{
		// El contexto se ha configurado para usar una cadena de conexión 'Model' del archivo 
		// de configuración de la aplicación (App.config o Web.config). De forma predeterminada, 
		// esta cadena de conexión tiene como destino la base de datos 'Data.Model.Model' de la instancia LocalDb. 
		// 
		// Si desea tener como destino una base de datos y/o un proveedor de base de datos diferente, 
		// modifique la cadena de conexión 'Model'  en el archivo de configuración de la aplicación.
		public Model()
			: base("name=Model")
		{
		}

		// Agregue un DbSet para cada tipo de entidad que desee incluir en el modelo. Para obtener más información 
		// sobre cómo configurar y usar un modelo Code First, vea http://go.microsoft.com/fwlink/?LinkId=390109.

		public virtual DbSet<Factura> Factura { get; set; }
		public virtual DbSet<FacturaLinea> FacturaLinea { get; set; }
		public virtual DbSet<Producto> Producto { get; set; }
		public virtual DbSet<Cliente> Cliente { get; set; }
		public virtual DbSet<TipoCambio> TipoCambio { get; set; }
	}
}