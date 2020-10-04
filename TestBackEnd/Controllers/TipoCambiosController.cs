using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace TestBackEnd.Controllers
{
	public class TipoCambiosController : ApiController
	{
		private Model db = new Model();

		//Lista TipoCambio
		public List<Models.TipoCambio> GetTipoCambios()
		{
			var listaCambios = new List<Models.TipoCambio>();

			//Buscamos los clientes 
			var cambio = db.TipoCambio.ToList();
			foreach (var item in cambio)
			{
				//Obtenemos la lista 
				listaCambios.Add(new Models.TipoCambio
				{
					IdTipoCambio = item.IdTipoCambio,
					Monto = item.Monto,
					Fecha = item.Fecha??DateTime.Now,
					FechaCreacion = item.FechaCreacion??DateTime.Now,
					FechaModificacion = item.FechaModificacion??DateTime.Now
				});
			}

			return listaCambios;
		}

		//Lista TipoCambio por mes
		[Route("api/TipoCambios/{mes:int}")]
		public List<Models.TipoCambio> GetTipoCambiosMes(int mes)
		{
			var listaCambios = new List<Models.TipoCambio>();

			//Buscamos los clientes 
			var cambio = db.TipoCambio.ToList();
			foreach (var item in cambio)
			{
				//Obtenemos la lista 
				listaCambios.Add(new Models.TipoCambio
				{
					IdTipoCambio = item.IdTipoCambio,
					Monto = item.Monto,
					Fecha = item.Fecha ?? DateTime.Now,
					FechaCreacion = item.FechaCreacion ?? DateTime.Now,
					FechaModificacion = item.FechaModificacion ?? DateTime.Now
				});
			}
			listaCambios = listaCambios.Where(x => x.Fecha.Month == mes).ToList();

			return listaCambios;
		}

		//Obtener TipoCambio del dia
		[Route("api/TipoCambios/{fecha}")]
		[ResponseType(typeof(Models.TipoCambio))]
		public IHttpActionResult GetTipoCambio(DateTime fecha)
		{
			try
			{
				//Buscamos el tipo de cambio
				var cambio = db.TipoCambio.FirstOrDefault(x => x.Fecha == fecha);

				//Validamos que exista
				if (cambio == null)
				{
					return Error(HttpStatusCode.NotFound, "Tipo de cambio no encontrado");
				}

				var cambioRetorno = new Models.TipoCambio
				{
					IdTipoCambio = cambio.IdTipoCambio,
					Monto = cambio.Monto,
					Fecha = cambio.Fecha ?? DateTime.Now,
					FechaCreacion = cambio.FechaCreacion ?? DateTime.Now,
					FechaModificacion = cambio.FechaModificacion ?? DateTime.Now
				};

				return Ok(cambio);
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				if (ex.InnerException != null)
					message = message + " " + ex.InnerException.Message;
				return Error(HttpStatusCode.InternalServerError, message);
			}
			
		}

		//Crear TipoCambio
		[ResponseType(typeof(Models.TipoCambio))]
		public IHttpActionResult PostTipoCambio(Models.TipoCambio tipoCambio)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				if (tipoCambio.Monto == null)
					return Error(HttpStatusCode.BadRequest, "El monto es requerido");
				if (tipoCambio.Fecha == null)
					return Error(HttpStatusCode.BadRequest, "La fecha es requerida");

				var existe = db.TipoCambio.FirstOrDefault(x => x.Fecha == tipoCambio.Fecha);
				if (existe != null)
					return Error(HttpStatusCode.Ambiguous, string.Format("Ya existe un tipo de cambio para la fecha: {0}", tipoCambio.Fecha));

				db.TipoCambio.Add(new Data.Model.Class.TipoCambio
				{
					Monto = tipoCambio.Monto ?? 0,
					Fecha = Convert.ToDateTime(tipoCambio.Fecha.ToString("MM/dd/yyyy 00:00:00")),
					FechaCreacion = DateTime.Now,
				});
				db.SaveChanges();

				return StatusCode(HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				if (ex.InnerException != null)
					message = message + " " + ex.InnerException.Message;
				return Error(HttpStatusCode.InternalServerError, message);
			}
			
		}

		//Actualizar TipoCambio
		[Route("api/TipoCambios/{fecha}")]
		[ResponseType(typeof(void))]
		public IHttpActionResult PutTipoCambio(DateTime fecha, Models.TipoCambio tipoCambio)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				var tipoCambioExist = db.TipoCambio.FirstOrDefault(x => x.Fecha == fecha);
				if (tipoCambioExist != null)
				{
					if(tipoCambio.Monto!=null)
						tipoCambioExist.Monto = tipoCambio.Monto??0;

					tipoCambioExist.FechaModificacion = DateTime.Now;
					db.SaveChanges();
				}
				else
				{
					return Error(HttpStatusCode.NotFound, "Tipo de cambio no encontrado");
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

		//Eliminacion Tipocambio
		[Route("api/TipoCambios/{fecha}")]
		[ResponseType(typeof(Models.TipoCambio))]
		public IHttpActionResult DeleteTipoCambio(DateTime fecha)
		{
			try
			{
				var tipoCambio = db.TipoCambio.FirstOrDefault(x => x.Fecha == fecha.Date);
				if (tipoCambio == null)
				{
					return Error(HttpStatusCode.NotFound, "Tipo de cambio no encontrado");
				}

				db.TipoCambio.Remove(tipoCambio);
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

		[Route("api/TipoCambios/{code:int}/{Message}")]
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
