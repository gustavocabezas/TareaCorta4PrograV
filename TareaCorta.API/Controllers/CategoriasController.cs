using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
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
            var lst = from d in db.Categorias
                      orderby d.Nombre
                      select d;

            return lst;
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

        // GET: api/Categorias/PorNombre/5 
        [HttpGet]
        [Route("api/Categorias/PorNombre/{nombre}")]
        [ResponseType(typeof(Categorias))]
        public async Task<IHttpActionResult> GetCategoriasPorNombre(string nombre)
        {
            Categorias categorias = await db.Categorias.FirstOrDefaultAsync(c => c.Nombre == nombre);
            if (categorias == null)
            {
                return NotFound();
            }

            return Ok(categorias);
        }

        // PUT: api/Categorias/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategorias(string nombre, Categorias categorias)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategoria = await db.Categorias.FirstOrDefaultAsync(c => c.Nombre == nombre);

            if (existingCategoria == null)
            {
                return Content(HttpStatusCode.NotFound, new { codigo = 404, mensaje = "El platillo no existe." });
            }


            if (existingCategoria.Nombre.ToLower() != categorias.Nombre.ToLower())
            {
                existingCategoria.Nombre = categorias.Nombre;
            }


            db.Entry(existingCategoria).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriasExists(categorias.idCategoria))
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

            Categorias existingCategoria = await db.Categorias.FirstOrDefaultAsync(p => p.Nombre == categorias.Nombre);

            if (existingCategoria != null)
            {
                return Content(HttpStatusCode.Conflict, new { codigo = 409, mensaje = "La categoria ya existe." });
            }

            // validar que idestado id categoria y precio que solo se pueda cambiar algunos datos no solo todos 

            db.Categorias.Add(categorias);

            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = categorias.idCategoria }, categorias);
        }

        // DELETE: api/Categorias/5
        [ResponseType(typeof(Categorias))]
        public async Task<IHttpActionResult> DeleteCategorias(string nombre)
        {
            Categorias categorias = await db.Categorias.FirstOrDefaultAsync(p => p.Nombre == nombre);

            if (categorias == null)
            {
                return Content(HttpStatusCode.NotFound, new { codigo = 404, mensaje = "Le Platillo no existe." });
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