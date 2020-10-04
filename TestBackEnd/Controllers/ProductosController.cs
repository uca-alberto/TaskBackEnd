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
    public class ProductosController : ApiController
    {
		private Model db = new Model();

		//Lista de Productos 
		public List<Models.Producto> GetProductos()
		{
			var listaProductos = new List<Models.Producto>();

			//Buscamos los Productos 
			var productos = db.Producto.ToList();

			foreach (var item in productos)
			{
				//Obtenemos la lista de productos
				listaProductos.Add(new Models.Producto
				{
					IdProducto = item.IdProducto,
					Descripcion = item.Descripcion,
					Stock = item.Stock,
					PrecioCordoba = item.PrecioCordoba,
					PrecioDolar = item.PrecioDolar,
					FechaCreacion = item.FechaCreacion??DateTime.Now,
					FechaModificacion = item.FechaModificacion??DateTime.Now,
					TipoCambio = item.TipoCambio
				});
			}
			return listaProductos;
		}

		//Obtener producto por un Id
		[Route("api/productos/{id:int}")]
		[ResponseType(typeof(Models.Producto))]
		public IHttpActionResult GetProductoId(int id)
		{
			try
			{
				//Asignamos el producto
				var pro = db.Producto.Find(id);

				//Validamos que exista
				if (pro == null)
				{
					return Error(HttpStatusCode.NotFound, "Producto no encontrado");
				}

				var productoRetorno = new Models.Producto
				{
					IdProducto = pro.IdProducto,
					Descripcion = pro.Descripcion,
					Stock = pro.Stock,
					PrecioCordoba = pro.PrecioCordoba,
					PrecioDolar = pro.PrecioDolar,
					FechaCreacion = pro.FechaCreacion ?? DateTime.Now,
					FechaModificacion = pro.FechaModificacion ?? DateTime.Now,
					TipoCambio = pro.TipoCambio
				};

				return Ok(productoRetorno);
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				if (ex.InnerException != null)
					message = message + " " + ex.InnerException.Message;
				return Error(HttpStatusCode.InternalServerError, message);
			}
			
		}

		//Obtener producto por un Nombre
		[Route("api/productos/{value}")]
		[ResponseType(typeof(Models.Producto))]
		public IHttpActionResult GetProductoNombre(string value)
		{
			try
			{
				//Asignamos el producto
				var query = from e in db.Producto
							where e.Descripcion.Contains(value)
							select e;
				//Validamos que exista
				if (query == null)
				{
					return Error(HttpStatusCode.NotFound, "Producto no encontrado");
				}

				var productoRetorno = new Models.Producto
				{

					IdProducto = query.FirstOrDefault().IdProducto,
					Descripcion = query.FirstOrDefault().Descripcion,
					Stock = query.FirstOrDefault().Stock,
					PrecioCordoba = query.FirstOrDefault().PrecioCordoba,
					PrecioDolar = query.FirstOrDefault().PrecioDolar,
					TipoCambio = query.FirstOrDefault().TipoCambio,
					FechaCreacion = query.FirstOrDefault().FechaCreacion ?? DateTime.Now,
					FechaModificacion = query.FirstOrDefault().FechaModificacion ?? DateTime.Now

				};
				return Ok(productoRetorno);
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				if (ex.InnerException != null)
					message = message + " " + ex.InnerException.Message;
				return Error(HttpStatusCode.InternalServerError, message);
			}
			
		}

		//Crear producto
		[ResponseType(typeof(Models.Producto))]
		public IHttpActionResult PostProducto(Models.Producto producto)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				if (producto.Descripcion == null)
					return Error(HttpStatusCode.BadRequest, "La descripcion es requerida");

				if (producto.Stock == null)
					return Error(HttpStatusCode.BadRequest, "La cantidad es requerida");

				if (producto.PrecioCordoba == null)
					return Error(HttpStatusCode.BadRequest, "El precio cordoba es requerido");

				if (producto.PrecioDolar == null)
					return Error(HttpStatusCode.BadRequest, "El precio dolar es requerido");

				if (producto.TipoCambio == null)
					return Error(HttpStatusCode.BadRequest, "El tipo de cambio es requerido");

				db.Producto.Add(new Data.Model.Class.Producto
				{
					Descripcion = producto.Descripcion,
					Stock = producto.Stock??0,
					PrecioCordoba = producto.PrecioCordoba??0,
					PrecioDolar = producto.PrecioDolar??0,
					TipoCambio = producto.TipoCambio??0,
					FechaCreacion = DateTime.Now,
				});
				db.SaveChanges();

				return CreatedAtRoute("DefaultApi", new { id = producto.IdProducto }, producto);
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				if (ex.InnerException != null)
					message = message + " " + ex.InnerException.Message;
				return Error(HttpStatusCode.InternalServerError, message);
			}
		}

		//Actualizar producto
		[Route("api/productos/{id:int}")]
		[ResponseType(typeof(void))]
		public IHttpActionResult PutProducto(int id, Models.Producto producto)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				var productoExiste = db.Producto.FirstOrDefault(x => x.IdProducto == id);
				if (productoExiste != null)
				{
					if (producto.Descripcion != null)
						productoExiste.Descripcion = producto.Descripcion;

					if (producto.Stock != null)
						productoExiste.Stock = producto.Stock ?? 0;

					if (producto.PrecioCordoba != null)
						productoExiste.PrecioCordoba = producto.PrecioCordoba ?? 0;

					if (producto.PrecioDolar != null)
						productoExiste.PrecioDolar = producto.PrecioDolar ?? 0;

					if (producto.TipoCambio != null)
						productoExiste.TipoCambio = producto.TipoCambio ?? 0;

					productoExiste.FechaModificacion = DateTime.Now;
					db.SaveChanges();
				}
				else
				{
					return Error(HttpStatusCode.NotFound, "Producto no encontrado");
				}

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

		//Eliminacion de producto
		[Route("api/productos/{id:int}")]
		[ResponseType(typeof(Models.Producto))]
		public IHttpActionResult DeleteProducto(int id)
		{
			try
			{
				var producto = db.Producto.Find(id);
				if (producto == null)
				{
					return Error(HttpStatusCode.NotFound, "Producto no encontrado");
				}

				db.Producto.Remove(producto);
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

		[Route("api/Productos/{code:int}/{Message}")]
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
