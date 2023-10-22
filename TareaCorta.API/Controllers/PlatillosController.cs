using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TareaCorta4PrograV.DA;

namespace TareaCorta4PrograV.API.Controllers
{
    public class PlatillosController : ApiController
    {
        private LaCriollitaEntities db = new LaCriollitaEntities();

        // GET: api/Platillos
        public IQueryable<Platillos> GetPlatillos()
        {
            return db.Platillos;
        }

        // GET: api/Platillos/5
        [ResponseType(typeof(Platillos))]
        public async Task<IHttpActionResult> GetPlatillos(int id)
        {
            Platillos platillos = await db.Platillos.FindAsync(id);
            if (platillos == null)
            {
                return NotFound();
            }

            return Ok(platillos);
        }

        // GET: api/Platillos/PorNombre/5 
        [HttpGet]
        [Route("api/Platillos/PorNombre/{nombre}")]
        [ResponseType(typeof(Platillos))]
        public async Task<IHttpActionResult> GetPlatillosPorNombre(string nombre)
        {
            Platillos platillos = await db.Platillos.FirstOrDefaultAsync(c => c.Nombre == nombre);
            if (platillos == null)
            {
                return NotFound();
            }

            return Ok(platillos);
        }

        // PUT: api/Platillos/{nombre}
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPlatillos(string nombre, string categoria, string estado, Platillos platillos)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            // Buscar el platillo por su nombre en la base de datos
            Platillos existingPlatillo = await db.Platillos.FirstOrDefaultAsync(p => p.Nombre == nombre);

            if (existingPlatillo == null)
            {
                return Content(HttpStatusCode.NotFound, new { codigo = 404, mensaje = "El platillo no existe." });
            }

            // validar que idestado id categoria y precio que solo se pueda cambiar algunos datos no solo todos 

            if (nombre != null)
            {
                if (nombre.ToLower() != platillos.Nombre.ToLower())
                {
                    existingPlatillo.Nombre = platillos.Nombre;
                }
            }

            if (categoria != null)
            {
                var existingCategoria = await db.Categorias.FirstOrDefaultAsync(c => c.Nombre == categoria);

                if (existingCategoria.idCategoria != platillos.idCategoria)
                {
                    existingPlatillo.idCategoria = existingCategoria.idCategoria;
                }
            }

            if (estado != null)
            {
                var existingEstado = await db.Estados.FirstOrDefaultAsync(e => e.Nombre == estado);

                if (existingEstado.idEstado != platillos.idEstado)
                {
                    existingPlatillo.idEstado = existingEstado.idEstado;
                }
            }

            if (platillos.Costo > 0)
            {
                if (existingPlatillo.Costo != platillos.Costo)
                {
                    existingPlatillo.Costo = platillos.Costo;
                }
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Manejar errores específicos de tu aplicación
                return InternalServerError(ex);
            }

            return StatusCode(HttpStatusCode.NoContent);

        }


        // PUT: api/Platillos/Activar/5
        [ResponseType(typeof(void))]
        [Route("api/Platillos/Activar/{nombre}")]
        public async Task<IHttpActionResult> PutPlatillosActivar(string nombre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Platillos existingPlatillo = await db.Platillos.FirstOrDefaultAsync(p => p.Nombre == nombre);

            //var existingEntity = await db.Platillos.FindAsync(nombre);

            if (existingPlatillo == null)
            {
                return Content(HttpStatusCode.NotFound, new { codigo = 404, mensaje = "El platillo no existe." });
            }

            existingPlatillo.idEstado = 1;

            db.Entry(existingPlatillo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exd)
            {


                if (!PlatillosExists(existingPlatillo.idPlatillo))
                {
                    return Content(HttpStatusCode.NotFound, new { codigo = 404, mensaje = "El platillo no existe." });
                }
                else
                {
                    Debug.WriteLine(exd);
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Platillos/Inactivar/5
        [ResponseType(typeof(void))]
        [Route("api/Platillos/Inactivar/{nombre}")]
        public async Task<IHttpActionResult> PutPlatillosInactivar(string nombre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Platillos existingPlatillo = await db.Platillos.FirstOrDefaultAsync(p => p.Nombre == nombre);

            //var existingEntity = await db.Platillos.FindAsync(nombre);

            if (existingPlatillo == null)
            {
                return Content(HttpStatusCode.NotFound, new { codigo = 404, mensaje = "El platillo no existe." });
            }


            existingPlatillo.idEstado = 2;

            db.Entry(existingPlatillo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exd)
            {


                if (!PlatillosExists(existingPlatillo.idPlatillo))
                {
                    return Content(HttpStatusCode.NotFound, new { codigo = 404, mensaje = "El platillo no existe." });
                }
                else
                {
                    Debug.WriteLine(exd);
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/Platillos
        [ResponseType(typeof(Platillos))]
        public async Task<IHttpActionResult> PostPlatillos(Platillos platillos, string nombreCategoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            platillos.idEstado = 1;



            // Buscar la categoría por su nombre
            var PlatilloExistente = await db.Platillos.FirstOrDefaultAsync(p => p.Nombre == platillos.Nombre);

            if (PlatilloExistente != null)
            {
                return Content(HttpStatusCode.Conflict, new { codigo = 409, mensaje = "El platillo ya existe" });
            }

            var categoria = await db.Categorias.FirstOrDefaultAsync(c => c.Nombre == nombreCategoria);

            if (categoria == null)
            {
                return Content(HttpStatusCode.NotFound, new { codigo = 404, mensaje = "La categoría proporcionada no existe." });
            }

            // Asignar el ID de la categoría al platillo
            platillos.idCategoria = categoria.idCategoria;

            db.Platillos.Add(platillos);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = platillos.idPlatillo }, platillos);
        }

        // DELETE: api/Platillos/5
        [ResponseType(typeof(Platillos))]
        public async Task<IHttpActionResult> DeletePlatillos(string nombre)
        {
            Platillos platillo = await db.Platillos.FirstOrDefaultAsync(p => p.Nombre == nombre);


            if (platillo == null)
            {
                return Content(HttpStatusCode.NotFound, new { codigo = 404, mensaje = "Le Platillo no existe." });
            }

            db.Platillos.Remove(platillo);
            await db.SaveChangesAsync();

            return Ok(platillo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlatillosExists(int id)
        {
            return db.Platillos.Count(e => e.idPlatillo == id) > 0;
            // return db.Platillos.Count(e => e.Nombre == id) > 0;
        }
    }
}