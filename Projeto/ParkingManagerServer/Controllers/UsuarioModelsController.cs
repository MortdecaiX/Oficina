using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ParkingManagerServer.Models;

namespace ParkingManagerServer.Controllers
{
    public class UsuarioModelsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: api/UsuarioModels
        public IQueryable<UsuarioModel> GetUsuarioModels()
        {
            var usuarios = db.UsuarioModels;
            foreach(var usuario in usuarios)
            {
                usuario.Senha = null;
            }
            return db.UsuarioModels;
        }


        // GET: api/UsuarioModels/5
        [AcceptVerbs("POST")]
        [Route("api/UsuarioModels/Logon")]
        public UsuarioModel GetUsuarioModel(DadosLogon dadosLogon)
        {
            UsuarioModel usuarioModel = null;

            try
            {
                usuarioModel = db.UsuarioModels.Where(x => (x.Email == dadosLogon.Email && x.Senha == dadosLogon.Senha)).First();
                usuarioModel.Senha = null;
            }catch(Exception ex)
            {

            }

            return usuarioModel;
        }


        // GET: api/UsuarioModels/5
        [ResponseType(typeof(UsuarioModel))]
        public IHttpActionResult GetUsuarioModel(long id)
        {
            UsuarioModel usuarioModel = db.UsuarioModels.Find(id);
            usuarioModel.Senha = null;
            if (usuarioModel == null)
            {
                return NotFound();
            }

            return Ok(usuarioModel);
        }

        // GET: api/VeiculoModels/{placa}
        [Route("api/UsuarioModels/{id}/Veiculos")]
        public IQueryable<VeiculoModel> GetVeiculos(long id)
        {
            var usuario =  db.UsuarioModels.Find(id);
            if(usuario!=null)
             return usuario.Veiculos.AsQueryable();
             return new List<VeiculoModel>().AsQueryable();
        }

        // PUT: api/UsuarioModels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsuarioModel(long id, UsuarioModel usuarioModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuarioModel.Id)
            {
                return BadRequest();
            }

            db.Entry(usuarioModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioModelExists(id))
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

        // POST: api/UsuarioModels
        [ResponseType(typeof(UsuarioModel))]
        public IHttpActionResult PostUsuarioModel(UsuarioModel usuarioModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UsuarioModels.Add(usuarioModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = usuarioModel.Id }, usuarioModel);
        }

        // DELETE: api/UsuarioModels/5
        [ResponseType(typeof(UsuarioModel))]
        public IHttpActionResult DeleteUsuarioModel(long id)
        {
            UsuarioModel usuarioModel = db.UsuarioModels.Find(id);
            if (usuarioModel == null)
            {
                return NotFound();
            }

            db.UsuarioModels.Remove(usuarioModel);
            db.SaveChanges();

            return Ok(usuarioModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioModelExists(long id)
        {
            return db.UsuarioModels.Count(e => e.Id == id) > 0;
        }
    }
}