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
    public class PontoModelsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PontoModels
        public IQueryable<PontoModel> GetPontoModels()
        {
            var lista = db.PontoModels.ToList();
            return lista.AsQueryable();
        }

        // GET: api/PontoModels/5
        [ResponseType(typeof(PontoModel))]
        public IHttpActionResult GetPontoModel(long id)
        {
            PontoModel pontoModel = db.PontoModels.Find(id);
            if (pontoModel == null)
            {
                return NotFound();
            }

            return Ok(pontoModel);
        }

        // GET: api/PontoModels/ConectarPontos/?id={id}&destId={destId}
        [AcceptVerbs("GET")]
        [Route("api/PontoModels/ConectarPontos/{id}/{destId}")]
        [ResponseType(typeof(PontoModel))]
        public IHttpActionResult ConectarPontos(long id, long destId)
        {
            if (id == destId)
            {
                return BadRequest("Id de destino igual ao Id de origem");
            }
            PontoModel pontoModel = db.PontoModels.Find(id);
            PontoModel destPontoModel = db.PontoModels.Find(destId);


            if (pontoModel == null || destPontoModel == null)
            {
                return BadRequest("Id não encontrado");
            }

            if (pontoModel.PontosFilhosConectados == null)
            {
                pontoModel.PontosFilhosConectados = new List<PontoModel>() { destPontoModel };
            }
            else
            {
                pontoModel.PontosFilhosConectados.Add(destPontoModel);
            }

            
            db.Entry(pontoModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PontoModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            return Ok(pontoModel);
        }

       
        [HttpGet, ResponseType(typeof(void)), Route("api/PontoModels/{id}/ModificarTipo/")]
        public IHttpActionResult ChangePontoModel(long id, bool entrada, bool saida)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            if (!PontoModelExists(id))
            {
                return NotFound();
            }

            var pontoModel = db.PontoModels.Find(id);

            pontoModel.Entrada = entrada;
            pontoModel.Saida = saida;
            if (id != pontoModel.Id)
            {
                return BadRequest();
            }

            db.Entry(pontoModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PontoModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/PontoModels
        [ResponseType(typeof(PontoModel))]
        public IHttpActionResult PostPontoModel(PontoModel pontoModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PontoModels.Add(pontoModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pontoModel.Id }, pontoModel);
        }

        // DELETE: api/PontoModels/5
        [ResponseType(typeof(PontoModel))]
        public IHttpActionResult DeletePontoModel(long id)
        {
            PontoModel pontoModel = db.PontoModels.Find(id);
            if (pontoModel == null)
            {
                return NotFound();
            }

            db.PontoModels.Remove(pontoModel);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PontoModelExists(long id)
        {
            return db.PontoModels.Count(e => e.Id == id) > 0;
        }
    }
}