using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.Description;

namespace TestBackEnd.Controllers
{
    public class FacturasController : ApiController
    {
		private Model db = new Model();
		//ListarFacturas
		[Route("api/Facturas")]
		public List<Models.Factura> GetFacturas()
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
						TotalDolar = itemlinea.TotalDolar
					});
				}
				fac.FacturaLinea = linea;
				listaFacturas.Add(fac);
			}
			return listaFacturas;
		}

		//Obtener Factura por un Id
		[Route("api/Facturas/{id:int}")]
		[ResponseType(typeof(Models.Factura))]
		public IHttpActionResult GetFacturaId(int id)
		{
			try
			{
				//Asignamos la factura
				var fac = db.Factura.FirstOrDefault(x => x.IdFactura == id);

				//Validamos que exista
				if (fac == null)
				{
					return Error(HttpStatusCode.NotFound, "Factura no encontrada");
				}

				var facturaRetorno = new Models.Factura
				{
					IdFactura = fac.IdFactura,
					IdCliente = fac.IdCliente,
					NombreCliente = fac.Cliente.Nombre,
					Direcion = fac.Direcion,
					TotalCordoba = fac.TotalCordoba,
					TotalDolar = fac.TotalDolar,
					IvaCordoba = fac.IvaCordoba,
					IvaDolar = fac.IvaDolar,
				};
				var lista = new List<Models.FacturaLinea>();
				foreach (var item in fac.FacturaLinea)
				{
					lista.Add(new Models.FacturaLinea
					{
						IdFactura = item.IdFactura,
						IdFacturaLinea = item.IdLinea,
						IdArticulo = item.IdArticulo,
						DescripcionArticulo = item.Producto.Descripcion,
						Cantidad = item.Cantidad,
						PrecioCordoba = item.PrecioCordoba,
						PrecioDolar = item.PrecioDolar,
						TotalCordoba = item.TotalCordoba,
						TotalDolar = item.TotalDolar
					});
				}
				facturaRetorno.FacturaLinea = lista;

				return Ok(facturaRetorno);
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				if (ex.InnerException != null)
					message = message + " " + ex.InnerException.InnerException.Message;

				return Error(HttpStatusCode.InternalServerError, message);
			}
			
		}

		//Crear Factura
		[Route("api/Facturas/{fecha}")]
		[ResponseType(typeof(Models.Factura))]
		public IHttpActionResult PostFactura(DateTime fecha,Models.Factura factura)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				int codigo = 0;
				//Buscamos el ultimo Id de la factuta
				var ultimotId = db.Factura.ToList().OrderByDescending(x=>x.IdFactura).FirstOrDefault();
				if (ultimotId != null)
				{
					codigo = ultimotId.IdFactura;
				}
				//Buscamos el tipo de cambion
				var cambio = db.TipoCambio.FirstOrDefault(x => x.Fecha == fecha);
				if (cambio == null)
					return Error(HttpStatusCode.NotFound, string.Format("No existe tipo de cambio para la fecha: {0}", fecha));

				if (factura.Direcion == null)
					return Error(HttpStatusCode.BadRequest, string.Format("La direccion es requerida", fecha));

				var linea = new List<Data.Model.Class.FacturaLinea>();
				foreach (var item in factura.FacturaLinea)
				{
					var Pro = db.Producto.FirstOrDefault(x=>x.IdProducto == item.IdArticulo);
					if (Pro == null)
						return Error(HttpStatusCode.NotFound, string.Format("El producto con el codigo:{0} no existe", item.IdArticulo));
					else
						if (item.Cantidad > Pro.Stock)
						return Error(HttpStatusCode.BadRequest, string.Format("La cantidad del articulo:{0} supera a la existencia en inventario",item.IdArticulo));

					linea.Add(new Data.Model.Class.FacturaLinea
					{
						IdFactura = (codigo + 1),
						IdArticulo = item.IdArticulo,
						Cantidad = item.Cantidad,
						PrecioCordoba = Pro.PrecioCordoba,
						PrecioDolar = Pro.PrecioDolar,
						TotalCordoba = item.Cantidad * Pro.PrecioCordoba,
						TotalDolar = (item.Cantidad * Pro.PrecioCordoba) / cambio.Monto
					});
					//actualizamos stock
					Pro.Stock = Pro.Stock - item.Cantidad;
				}
				db.Factura.Add(new Data.Model.Class.Factura
				{
					IdFactura = (codigo + 1),
					IdCliente = factura.IdCliente,
					Direcion = factura.Direcion,
					TotalCordoba = linea.Sum(x => x.TotalCordoba),
					TotalDolar = linea.Sum(x => x.TotalDolar),
					IvaCordoba = linea.Sum(x => x.TotalCordoba) * 0.15,
					IvaDolar = linea.Sum(x => x.TotalDolar) * 0.15,
					FacturaLinea = linea,
					FechaCreacion = DateTime.Now
				});
				db.SaveChanges();

				return Ok();
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				if (ex.InnerException !=null)
					message = message + " " + ex.InnerException.InnerException.Message;

				return Error(HttpStatusCode.InternalServerError, message);
			}
			
		}

		//Eliminacion de factura
		[Route("api/Facturas/{id:int}")]
		[ResponseType(typeof(Models.Factura))]
		public IHttpActionResult DeleteFactura(int id)
		{
			try
			{
				var factura = db.Factura.FirstOrDefault(x => x.IdFactura == id);
				if (factura == null)
				{
					return Error(HttpStatusCode.NotFound, "Factura no encontrada");
				}

				db.FacturaLinea.RemoveRange(factura.FacturaLinea);
				db.Factura.Remove(factura);
				db.SaveChanges();
				return StatusCode(HttpStatusCode.NoContent);
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				if (ex.InnerException != null)
					message = message + " " + ex.InnerException.Message;
				return Error(HttpStatusCode.InternalServerError, message);
			}
			
		}

		[Route("api/Facturas/{code:int}/{Message}")]
		public IHttpActionResult Error(HttpStatusCode code, string Message)
		{
			return Content(code, new
			{
				Code = code,
				Message = Message
			});
		}
	}
}
