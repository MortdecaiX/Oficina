using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using ParkingManagerServer.Models;

namespace ParkingManagerServer.Controllers
{
    public class VeiculoModelsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/VeiculoModels
        public IQueryable<VeiculoModel> GetVeiculoModels()
        {
            return db.VeiculoModels;
        }

        // GET: api/VeiculoModels/{id}/Usuarios
        [Route("api/VeiculoModels/{id}/Usuarios")]
        public IQueryable<UsuarioModel> GetUsuarios(long id)
        {
            return db.VeiculoModels.Find(id).Usuarios.AsQueryable();
        }
        // GET: api/VeiculoModels/{placa}
        [Route("api/VeiculoModels/Placa/{placa}")]
        public VeiculoModel GetVeiculoModel(string placa)
        {
            var result = db.VeiculoModels.Where(v => v.Placa.ToLower().Equals(placa.ToLower()));
            if(result != null)
                return result.First();
            return null;
        }
        // GET: api/VeiculoModels/{placa}
        [Route("api/VeiculoModels/Placa/{placa}/Usuarios")]
        public IQueryable<UsuarioModel> GetVeiculoUsuariosModel(string placa)
        {
            var results = db.VeiculoModels.Where(v => v.Placa.ToLower().Equals(placa.ToLower()));
                if(results!=null)
                    return results.First().Usuarios.AsQueryable();
           
                return new List<UsuarioModel>().AsQueryable();
           
        }
        // GET: api/VeiculoModels/5
        [ResponseType(typeof(VeiculoModel))]
        public IHttpActionResult GetVeiculoModel(long id)
        {
            VeiculoModel veiculoModel = db.VeiculoModels.Find(id);
            if (veiculoModel == null)
            {
                return NotFound();
            }

            return Ok(veiculoModel);
        }

        // PUT: api/VeiculoModels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVeiculoModel(long id, VeiculoModel veiculoModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != veiculoModel.Id)
            {
                return BadRequest();
            }

            db.Entry(veiculoModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeiculoModelExists(id))
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

        // POST: api/VeiculoModels
        [ResponseType(typeof(VeiculoModel))]
        public IHttpActionResult PostVeiculoModel(VeiculoModel veiculoModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VeiculoModels.Add(veiculoModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = veiculoModel.Id }, veiculoModel);
        }
        // GET: api/UsuarioModels/5
        [AcceptVerbs("GET")]
        [Route("api/CreateUsuarioModel")]
        public UsuarioModel CreateUsuarioModel()
        {
            UsuarioModel usuarioModel = new UsuarioModel("teste", "testse", "sdfsdf", "asdfasdf", null);
            List<VeiculoModel> veiculos = new List<VeiculoModel>();
          
            veiculos.Add(new VeiculoModel(new List<UsuarioModel>() { usuarioModel }, "asd-3423", "Fiat", "Uno"));

            usuarioModel.Veiculos = veiculos;


            db.UsuarioModels.Add(usuarioModel);
            db.SaveChanges();

            return db.UsuarioModels.Find(usuarioModel.Id);
            
        }

        // DELETE: api/VeiculoModels/5
        [ResponseType(typeof(VeiculoModel))]
        public IHttpActionResult DeleteVeiculoModel(long id)
        {
            VeiculoModel veiculoModel = db.VeiculoModels.Find(id);
            if (veiculoModel == null)
            {
                return NotFound();
            }

            db.VeiculoModels.Remove(veiculoModel);
            db.SaveChanges();

            return Ok(veiculoModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VeiculoModelExists(long id)
        {
            return db.VeiculoModels.Count(e => e.Id == id) > 0;
        }
    }
}