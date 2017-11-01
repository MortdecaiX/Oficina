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
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ParkingManagerServer.Controllers
{
    public class EstacionamentoModelsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/EstacionamentoModels
        public IQueryable<EstacionamentoModel> GetEstacionamentoModels()
        {
            List<EstacionamentoModel> lista = db.EstacionamentoModels.ToList() ;
            return lista.AsQueryable();
        }

        [AcceptVerbs("POST")]
        [Route("api/EstacionamentoModel/{id}/AdicionarPonto")]
        [ResponseType(typeof(int))]
        public IHttpActionResult PostPontoModel(long id, PontoModel ponto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EstacionamentoModel est =  db.EstacionamentoModels.Find(id);
            if(est == null)
            {
                return BadRequest("Estacionamento não encontrado.");
            }
            est.Pontos.Add(ponto);

            db.Entry(est).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstacionamentoModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { Id = ponto.Id });
        }

        // GET: api/EstacionamentoModels/5
        [ResponseType(typeof(EstacionamentoModel))]
        public IHttpActionResult GetEstacionamentoModel(long id)
        {
            EstacionamentoModel estacionamentoModel = db.EstacionamentoModels.Find(id);
            if (estacionamentoModel == null)
            {
                return NotFound();
            }

            return Ok(estacionamentoModel);
        }


        [HttpPost, Route("api/EstacionamentoModels/{id}/Imagem")]
        public string  Upload(long id, UploadData data)
        {
            
                var estacionamento = db.EstacionamentoModels.Find(id);
                var base64 = data.Data.Split(',')[1];
                estacionamento.ImagemBase64 = base64;
                estacionamento.NEBoundImagem = new PosicaoGeografica(data.neBoundLat, data.neBoundLng, 0);
                estacionamento.SWBoundImagem = new PosicaoGeografica(data.swBoundLat, data.swBoundLng, 0);
                var path = Path.Combine(HostingEnvironment.MapPath("~/Uploads/"), new Random(DateTime.Now.Millisecond).Next() + "_" + id + "_" + data.Filename);
                var directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.WriteAllBytes(path, data.DataToByteArray());
                data.url = MapURL(path);
                estacionamento.ImagemURL = data.url;

                db.Entry(estacionamento).State = EntityState.Modified;

                db.SaveChanges();

                return Newtonsoft.Json.JsonConvert.SerializeObject(data);
           
        }

       


        private string MapURL(string path)
        {
            string appPath = HostingEnvironment.MapPath("/").ToLower();
            return string.Format("/{0}", path.ToLower().Replace(appPath, "").Replace(@"\", "/"));
        }
        [AcceptVerbs("GET")]
        [Route("api/EstacionamentoModels/{termo}")]
        public List<EstacionamentoModel> GetEstacionamentoModel(string termo)
        {
            var estacionamentos = db.EstacionamentoModels.Where(x=>x.Nome.ToLower().Contains(termo.ToLower())).ToList<EstacionamentoModel>();
            return estacionamentos;
        }

        // PUT: api/EstacionamentoModels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEstacionamentoModel(long id, EstacionamentoModel estacionamentoModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != estacionamentoModel.Id)
            {
                return BadRequest();
            }

            db.Entry(estacionamentoModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstacionamentoModelExists(id))
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

        // POST: api/EstacionamentoModels
        [ResponseType(typeof(EstacionamentoModel))]
        public IHttpActionResult PostEstacionamentoModel(EstacionamentoModel estacionamentoModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EstacionamentoModels.Add(estacionamentoModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = estacionamentoModel.Id }, estacionamentoModel);
        }

        // DELETE: api/EstacionamentoModels/5
        [ResponseType(typeof(EstacionamentoModel))]
        public IHttpActionResult DeleteEstacionamentoModel(long id)
        {
            EstacionamentoModel estacionamentoModel = db.EstacionamentoModels.Find(id);
            if (estacionamentoModel == null)
            {
                return NotFound();
            }

           

            

            db.EstacionamentoModels.Remove(estacionamentoModel);
            db.SaveChanges();

            return Ok(estacionamentoModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EstacionamentoModelExists(long id)
        {
            return db.EstacionamentoModels.Count(e => e.Id == id) > 0;
        }
    }
}