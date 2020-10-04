using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace TestBackEnd.Controllers
{
    public class ReportesController : ApiController
    {
		private Model db = new Model();
		//Reportes de venta
		[HttpGet()]
		[ResponseType(typeof(List<Models.ReporteVentas>))]
		public List<Models.ReporteVentas> GetClientes([FromUri] Models.Filtros filtros)
		{
			var listaReportes = new List<Models.ReporteVentas>();

			//Buscamos los clientes 
			var reporte = db.Cliente.ToList();

			foreach (var item in reporte)
			{
				var factura = new List<Models.Factura>();
				foreach (var itemfac in item.facturaLinea)
				{
					factura = GetFacturas(item.IdCliente);
				}

				listaReportes.Add(new Models.ReporteVentas
				{
					IdCliente = item.IdCliente,
					NombreCliente = item.Nombre,
					Anio = filtros.Anio == null ? DateTime.Now.Year : filtros.Anio ?? 0,
					Mes = filtros.Mes == null ? DateTime.Now.Month : filtros.Mes ?? 0,
					TotalCordoba = factura.Sum(x => x.TotalCordoba),
					TotalDolar = factura.Sum(x => x.TotalDolar),
					Facturas = factura
				});
			}
			//Filtros 
			if (filtros.IdCliente != null)
				listaReportes = listaReportes.Where(x => x.IdCliente == filtros.IdCliente).ToList();
			if (filtros.NombreCliente != null)
			{
				var listafiltrada = from e in listaReportes
									where e.NombreCliente.Contains(filtros.NombreCliente)
									select e;
				listaReportes = listafiltrada.ToList();
			}
			if (filtros.Anio != null)
			{
				foreach (var item in listaReportes)
				{
					item.Anio = filtros.Anio??0;
					item.Facturas = item.Facturas.Where(x=>x.FechaCreacion.Year == filtros.Anio).ToList();
				}
			}
			if (filtros.Mes != null)
			{
				foreach (var item in listaReportes)
				{
					item.Mes = filtros.Mes ?? 0;
					item.Facturas = item.Facturas.Where(x => x.FechaCreacion.Month == filtros.Mes).ToList();
				}
			}
			return listaReportes;
		}

		//Lista de Factura
		[Route("api/Reportes/{IdCliente:int}")]
		public List<Models.Factura> GetFacturas(int IdCliente)
		{
			var listaFacturas = new List<Models.Factura>();

			//Buscamos las Factura 
			var facturas = db.Factura.ToList();

			foreach (var item in facturas)
			{
				//Obtenemos la lista de facturas
				var fac = new Models.Factura();
				var linea = new List<Models.FacturaLinea>();
				fac.IdFactura = item.IdFactura;
				fac.IdCliente = item.IdCliente;
				fac.NombreCliente = item.Cliente.Nombre;
				fac.Direcion = item.Direcion;
				fac.TotalCordoba = item.TotalCordoba;
				fac.TotalDolar = item.TotalDolar;
				fac.IvaCordoba = item.IvaCordoba;
				fac.IvaDolar = item.IvaDolar;
				fac.FechaCreacion = item.FechaCreacion??DateTime.Now;

				var lin = item.FacturaLinea;
				foreach (var itemlinea in lin)
				{
					linea.Add(new Models.FacturaLinea
					{
						IdFactura = itemlinea.IdFactura,
						IdFacturaLinea = itemlinea.IdLinea,
						IdArticulo = itemlinea.IdArticulo,
						DescripcionArticulo = itemlinea.Producto.Descripcion,
						Cantidad = itemlinea.Cantidad,
						PrecioCordoba = itemlinea.PrecioCordoba,
						PrecioDolar = itemlinea.PrecioDolar,
						TotalCordoba = itemlinea.TotalCordoba,
						TotalDolar = itemlinea.TotalDolar,
					});
				}
				fac.FacturaLinea = linea;
				listaFacturas.Add(fac);
			}
			if (IdCliente != null)
			{
				listaFacturas = listaFacturas.Where(x => x.IdCliente == IdCliente).ToList();
			}
			return listaFacturas;
		}
	}
}
