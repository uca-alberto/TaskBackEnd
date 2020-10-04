using Data.Model;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;

namespace TestBackEnd.Controllers
{
	public class ClientesController : ApiController
	{
		private Model db = new Model();

		//Lista de clientes 
		[ResponseType(typeof(List<Models.Cliente>))]
		public List<Models.Cliente> GetClientes()
		{
			var listaClientes = new List<Models.Cliente>();

			//Buscamos los clientes 
			var clientes = db.Cliente.ToList();

			foreach (var item in clientes)
			{
				//Obtenemos la lista de clientes
				listaClientes.Add(new Models.Cliente
				{
					IdCliente = item.IdCliente,
					Nombre = item.Nombre,
					Direccion = item.Direccion,
					Correo = item.Correo,
					Edad = item.Edad,
					EstadoCivil = item.EstadoCivil,
					FechaCreacion = item.FechaCreacion??DateTime.Now,
					FechaModificacion = item.FechaModificacion??DateTime.Now
				});
			}
			return listaClientes;
		}

		//Obtener cliente por un Id
		[Route("api/Clientes/{id:int}")]
		[ResponseType(typeof(Models.Cliente))]
		public IHttpActionResult GetClienteId(int id)
		{
			try
			{
				//Asignamos el cliente
				var cli = db.Cliente.Find(id);

				//Validamos que exista
				if (cli == null)
				{
					return Error(HttpStatusCode.NotFound, "El cliente no existe");
				}

				var clienteRetorno = new Models.Cliente
				{
					IdCliente = cli.IdCliente,
					Nombre = cli.Nombre,
					Direccion = cli.Direccion,
					Correo = cli.Correo,
					Edad = cli.Edad,
					EstadoCivil = cli.EstadoCivil,
					FechaCreacion = cli.FechaCreacion ?? DateTime.Now,
					FechaModificacion = cli.FechaModificacion ?? DateTime.Now
				};

				return Ok(clienteRetorno);
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				if (ex.InnerException != null)
					message = message + " " + ex.InnerException.Message;
				return Error(HttpStatusCode.InternalServerError, message);
			}
			
		}

		//Obtener cliente por un Nombre
		[Route("api/Clientes/{value}")]
		[ResponseType(typeof(Models.Cliente))]
		public IHttpActionResult GetClienteNombre(string value)
		{
			try
			{
				//Asignamos el cliente
				var query = from e in db.Cliente
							where e.Nombre.Contains(value)
							select e;
				//Validamos que exista
				if (query == null)
				{
					return Error(HttpStatusCode.NotFound, "El cliente no existe");
				}

				var clienteRetorno = new Models.Cliente
				{
					IdCliente = query.FirstOrDefault().IdCliente,
					Nombre = query.FirstOrDefault().Nombre,
					Direccion = query.FirstOrDefault().Direccion,
					Correo = query.FirstOrDefault().Correo,
					Edad = query.FirstOrDefault().Edad,
					EstadoCivil = query.FirstOrDefault().EstadoCivil,
					FechaCreacion = query.FirstOrDefault().FechaCreacion ?? DateTime.Now,
					FechaModificacion = query.FirstOrDefault().FechaModificacion ?? DateTime.Now

				};

				return Ok(clienteRetorno);
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				if (ex.InnerException != null)
					message = message + " " + ex.InnerException.Message;
				return Error(HttpStatusCode.InternalServerError, message);
			}
			
		}

		//Crear Cliente
		[ResponseType(typeof(Models.Cliente))]
		public IHttpActionResult PostCliente(Models.Cliente cliente)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);

				if (cliente.Nombre == null)
					return Error(HttpStatusCode.BadRequest, "El nombre es requerido");

				else if (cliente.Correo == null)
					return Error(HttpStatusCode.BadRequest, "El correo es requerido");

				else if (cliente.Direccion == null)
					return Error(HttpStatusCode.BadRequest, "La direccion es requerida");

				db.Cliente.Add(new Data.Model.Class.Cliente
				{
					Nombre = cliente.Nombre,
					Direccion = cliente.Direccion,
					EstadoCivil = cliente.EstadoCivil,
					Correo = cliente.Correo,
					Edad = cliente.Edad??0,
					FechaCreacion = DateTime.Now
				});
				db.SaveChanges();

				return CreatedAtRoute("DefaultApi", new { id = cliente.IdCliente }, cliente);
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				if (ex.InnerException != null)
					message = message + " " + ex.InnerException.Message;
				return Error(HttpStatusCode.InternalServerError, message);
			}
			
		}

		//Actualizar Cliente
		[Route("api/Clientes/{id:int}")]
		[ResponseType(typeof(void))]
		public IHttpActionResult PutCliente(int id, Models.Cliente cliente)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);

				var clienteExiste = db.Cliente.FirstOrDefault(x => x.IdCliente == id);
				if (clienteExiste != null)
				{
					if(cliente.Nombre!=null)
						clienteExiste.Nombre = cliente.Nombre;

					if (cliente.Direccion != null)
						clienteExiste.Direccion = cliente.Direccion;

					if (cliente.EstadoCivil != null)
						clienteExiste.EstadoCivil = cliente.EstadoCivil;

					if (cliente.Correo != null)
						clienteExiste.Correo = cliente.Correo;

					if (cliente.Edad != null)
						clienteExiste.Edad = cliente.Edad??0;

					clienteExiste.FechaModificacion = DateTime.Now;
					db.SaveChanges();
				}
				else
					return Error(HttpStatusCode.NotFound, "El cliente no existe");

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

		//Eliminacion de Cliente
		[Route("api/Clientes/{id:int}")]
		[ResponseType(typeof(Models.Cliente))]
		public IHttpActionResult DeleteCliente(int id)
		{
			try
			{
				var cliente = db.Cliente.Find(id);
				if (cliente == null)
				{
					return Error(HttpStatusCode.NotFound, "El cliente no existe");
				}

				db.Cliente.Remove(cliente);
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

		[Route("api/Clientes/{code:int}/{Message}")]
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
