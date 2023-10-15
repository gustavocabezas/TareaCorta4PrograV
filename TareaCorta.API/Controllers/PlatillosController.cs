using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TareaCorta4.DA;

namespace TareaCorta.API.Controllers
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

        // PUT: api/Platillos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPlatillos(int id, Platillos platillos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != platillos.idPlatillo)
            {
                return BadRequest();
            }

            db.Entry(platillos).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlatillosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }




         

        // PUT: api/Platillos/Activar/5
        [ResponseType(typeof(void))]
        [Route("api/Platillos/Activar/{id}")] 
        public async Task<IHttpActionResult> PutPlatillosActivar(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } 
             
            var existingEntity = await db.Platillos.FindAsync(id);

            if (existingEntity == null)
                return Content(HttpStatusCode.NotFound, new { mensaje = id });

            existingEntity.idEstado = 1;

            db.Entry(existingEntity).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exd)
            {
                if (!PlatillosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    Debug.WriteLine(exd);
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Platillos/Activar/5
        [ResponseType(typeof(void))]
        [Route("api/Platillos/Inactivar/{id}")]
        public async Task<IHttpActionResult> PutPlatillosInactivar(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } 

            var existingEntity = await db.Platillos.FindAsync(id);

            if (existingEntity == null)
                return Content(HttpStatusCode.NotFound, new { mensaje = id });

            existingEntity.idEstado = 2;

            db.Entry(existingEntity).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exd)
            {
                if (!PlatillosExists(id))
                {
                    return NotFound();
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
        public async Task<IHttpActionResult> PostPlatillos(Platillos platillos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            platillos.idEstado = 1;
            db.Platillos.Add(platillos);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = platillos.idPlatillo }, platillos);
        }

        // DELETE: api/Platillos/5
        [ResponseType(typeof(Platillos))]
        public async Task<IHttpActionResult> DeletePlatillos(int id)
        {
            Platillos platillos = await db.Platillos.FindAsync(id);
            if (platillos == null)
            {
                return NotFound();
            }

            db.Platillos.Remove(platillos);
            await db.SaveChangesAsync();

            return Ok(platillos);
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