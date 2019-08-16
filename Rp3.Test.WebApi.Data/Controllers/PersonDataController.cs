using Rp3.Test.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rp3.Test.WebApi.Data.Controllers
{
    public class PersonDataController : ApiController
    {
        /// <summary>
        /// Método para devolver todas las personas en estado activo del sistema
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(bool? active = null)
        {
            List<Rp3.Test.Common.Models.Person> commonModel = new List<Common.Models.Person>();

            using (DataService service = new DataService())
            {
                var query = service.Persons.GetQueryable();

                if (active.HasValue)
                    query = query.Where(p => p.Active == active.Value);

                commonModel = query.Select(p => new Common.Models.Person()
                {
                    Active = p.Active,
                    PersonId = p.PersonId,
                    Name = p.Name
                }).ToList();
            }

            return Ok(commonModel);
        }
    }
}