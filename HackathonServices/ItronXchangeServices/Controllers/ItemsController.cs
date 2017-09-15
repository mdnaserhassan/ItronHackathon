using ItronXchangeServices.Models;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ItronXchangeServices.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Items")]
    public class ItemsController : ApiController
    {
        // GET api/values
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetAllItem()
        {
            var query = "SELECT * FROM ITEM";
            IList<Item> items = null;
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {

                    var sqlquery = session.CreateSQLQuery(query)
                         .SetResultTransformer(Transformers.AliasToBean<Item>());
                     items=sqlquery.List<Item>();
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // GET api/values/5
        [HttpGet]
        [Route("{id:int}")]
        public HttpResponseMessage GetAllItemByID(int id)
        {
            var query = "SELECT * FROM ITEM WHERE ITEMID="+id;
            Item item = null;
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {

                    var sqlquery = session.CreateSQLQuery(query)
                         .SetResultTransformer(Transformers.AliasToBean<Item>());
                    item = sqlquery.List<Item>().FirstOrDefault();
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpGet]
        [Route("{name}")]
        public HttpResponseMessage GetAllItemByName(string name)
        {
            var query = "SELECT * FROM ITEM WHERE TITLE = '" + name+"'";
            IList<Item> items = null;
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {

                    var sqlquery = session.CreateSQLQuery(query)
                         .SetResultTransformer(Transformers.AliasToBean<Item>());
                    items = sqlquery.List<Item>();
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // POST api/values
        [HttpPost]
        [Route("")]
        public void AddItem([FromBody]Item value)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(value);
                    transaction.Commit();
                }
            }

        }

        // PUT api/values/5
        [HttpPut]
        [Route("{id}")]
        public void UpdateItem(int id, [FromBody]Item value)
        {

            var query = "SELECT * FROM ITEM WHERE ITEMID=" + id;
            Item item = null;
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var sqlquery = session.CreateSQLQuery(query)
                        .SetResultTransformer(Transformers.AliasToBean<Item>());
                    item = sqlquery.List<Item>().FirstOrDefault();
                    item.Category = value.Category;
                    item.Description = value.Description;
                    item.ImageIDs = value.ImageIDs;
                    item.Phone = value.Phone;
                    item.Price = value.Price;
                    item.Title = value.Title;                    
                    session.SaveOrUpdate(item);
                    transaction.Commit();
                }
            }
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {
            var query = "SELECT * FROM ITEM WHERE ITEMID=" + id;
            Item item = null;
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var sqlquery = session.CreateSQLQuery(query)
                        .SetResultTransformer(Transformers.AliasToBean<Item>());
                    item = sqlquery.List<Item>().FirstOrDefault();
                    
                    session.Delete(item);
                    transaction.Commit();
                }
            }
        }
    }
}
