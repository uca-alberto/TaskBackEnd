namespace Data.Migrations
{
	using Data.Model.Class;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.Model.Model>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Data.Model.Model context)
        {
			if (!context.Cliente.Any())
			 {
				 var cliente = new Cliente
				 {
					 Nombre = "Alberto de Jesus",
					 Edad = 20,
					 EstadoCivil = "Soltero",
					 Correo = "Alberto@gmail.com",
					 Direccion = "Managua",
					 FechaCreacion = Convert.ToDateTime(DateTime.Now)
				 };
				 context.Cliente.Add(cliente);
				 context.SaveChanges();
			 }
			 if (!context.TipoCambio.Any())
			 {
				 context.TipoCambio.Add(new TipoCambio
				 {
					 Monto = 31.20,
					 Fecha = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy 00:00:00")),
					 FechaCreacion = DateTime.Now
				 });
				 context.SaveChanges();
			 }
			 if (!context.Producto.Any())
			 {
				 context.Producto.Add(new Producto
				 {
					 Descripcion = "Ron plata de 500ml",
					 Stock = 10,
					 PrecioCordoba = 31.20,
					 PrecioDolar = 1,
					 TipoCambio = 31.20,
					 FechaCreacion = DateTime.Now
				 });
				 context.SaveChanges();
			 }

			 if (!context.Factura.Any())
			 {
				 var factura = new Factura
				 {
					 IdFactura = 1,
					 IdCliente = 1,
					 Direcion = "Managua",
					 FechaCreacion = DateTime.Now,
					 TotalCordoba = 31.20,
					 TotalDolar = 1,
					 IvaCordoba = 35.88,
					 IvaDolar = 1.15,
					 FacturaLinea = new List<FacturaLinea>
					 {
						 new FacturaLinea{IdFactura = 1,IdArticulo = 1, Cantidad = 5, PrecioCordoba = 31.20,PrecioDolar = 1, TotalCordoba = 156, TotalDolar = 5}
					 }
				 };
				 context.Factura.Add(factura);
				 context.SaveChanges();
			 }
		}
	}
}
