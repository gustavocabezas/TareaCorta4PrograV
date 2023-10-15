using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TareaCorta4.DA;

namespace TareaCorta.API.Controllers
{
    public class CategoriasController : ApiController
    {
        private LaCriollitaEntities db = new LaCriollitaEntities();

        // GET: api/Categorias
        public IQueryable<Categorias> GetCategorias()
        {
            return db.Categorias;
        }

        // GET: api/Categorias/5
        [ResponseType(typeof(Categorias))]
        public async Task<IHttpActionResult> GetCategorias(int id)
        {
            Categorias categorias = await db.Categorias.FindAsync(id);
            if (categorias == null)
            {
                return NotFound();
            }

            return Ok(categorias);
        }

        // PUT: api/Categorias/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategorias(int id, Categorias categorias)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categorias.idCategoria)
            {
                return BadRequest();
            }

            db.Entry(categorias).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriasExists(id))
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

        // POST: api/Categorias
        [ResponseType(typeof(Categorias))]
        public async Task<IHttpActionResult> PostCategorias(Categorias categorias)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categorias.Add(categorias);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = categorias.idCategoria }, categorias);
        }

        // DELETE: api/Categorias/5
        [ResponseType(typeof(Categorias))]
        public async Task<IHttpActionResult> DeleteCategorias(int id)
        {
            Categorias categorias = await db.Categorias.FindAsync(id);
            if (categorias == null)
            {
                return NotFound();
            }

            db.Categorias.Remove(categorias);
            await db.SaveChangesAsync();

            return Ok(categorias);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoriasExists(int id)
        {
            return db.Categorias.Count(e => e.idCategoria == id) > 0;
        }
    }
}