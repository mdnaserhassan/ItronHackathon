using ItronXchangeServices.Models;
using ItronXchangeServices.ResponseType;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
            var query = @"SELECT it.*,img.URL,usr.itronID CreatedBy,usr.Email FROM ITEM it
                        join dbo.image img on it.ImageID=img.ImageID
                        join dbo.[User] usr on usr.UserID=it.UserID
                        ";
            IList<Item_Result> items = null;
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {

                    var sqlquery = session.CreateSQLQuery(query)
                         .SetResultTransformer(Transformers.AliasToBean<Item_Result>());
                    items = sqlquery.List<Item_Result>();
                }
            }
            var result = items.GroupBy(x => x.ItemID).Select(
                        x => new ItemResult
                        {
                            ItemID = x.Key,
                            Category = x.FirstOrDefault().Category,
                            Phone = x.FirstOrDefault().Phone,
                            Description = x.FirstOrDefault().Description,
                            Price = x.FirstOrDefault().Price,
                            Images = x.Select(obj => obj.URL).ToList(),
                            Title = x.FirstOrDefault().Title,
                            UserID = x.FirstOrDefault().userid,
                            CreatedBy = x.FirstOrDefault().CreatedBy,
                            Email = x.FirstOrDefault().Email,
                            

                        }
                );
            var json = JsonConvert.SerializeObject(result, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

        // GET api/values/5
        [HttpGet]
        [Route("user/{id}")]
        public HttpResponseMessage GetAllItemByID(string id)
        {
            var query = @"SELECT it.*,img.URL,usr.itronID CreatedBy,usr.Email FROM ITEM it
                        join dbo.image img on it.ImageID=img.ImageID
                        join dbo.[User] usr on usr.UserID=it.UserID
                        WHERE usr.UserID='" + id + "'";
            IList<Item_Result> item = null;
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {

                    var sqlquery = session.CreateSQLQuery(query)
                         .SetResultTransformer(Transformers.AliasToBean<Item_Result>());
                    item = sqlquery.List<Item_Result>();
                }
            }
            ItemResult result = new ItemResult();
            result.Category = item.FirstOrDefault().Category;
            result.Description = item.FirstOrDefault().Description;
            result.Title = item.FirstOrDefault().Title;
            result.ItemID = item.FirstOrDefault().ItemID;
            result.Phone = item.FirstOrDefault().Phone;
            result.Price = item.FirstOrDefault().Price;
            result.UserID = item.FirstOrDefault().userid;
            result.CreatedBy = item.FirstOrDefault().CreatedBy;
            result.Email = item.FirstOrDefault().Email;
            result.Images = item.Select(obj => obj.URL).ToList();


            var result2 = item.GroupBy(x => x.ItemID).Select(
                       x => new ItemResult
                       {
                           ItemID = x.Key,
                           Category = x.FirstOrDefault().Category,
                           Phone = x.FirstOrDefault().Phone,
                           Description = x.FirstOrDefault().Description,
                           Price = x.FirstOrDefault().Price,
                           Images = x.Select(obj => obj.URL).ToList(),
                           Title = x.FirstOrDefault().Title,
                           UserID = x.FirstOrDefault().userid,
                           CreatedBy = x.FirstOrDefault().CreatedBy,
                           Email = x.FirstOrDefault().Email,


                       }
               );

            var json = JsonConvert.SerializeObject(result2, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
        [HttpGet]
        [Route("{identity}/{name}")]
        public HttpResponseMessage GetAllItemByNameAndIdentity(string name, string identity)
        {
            var query = @"SELECT it.*,img.URL,usr.itronID CreatedBy,usr.Email FROM ITEM it
                        join dbo.image img on it.ImageID=img.ImageID
                        join dbo.[User] usr on usr.UserID=it.UserID

                        WHERE Category='" + name + "'" +
                        "and usr.userID='" + identity + "'";
            ;
            IList<Item_Result> items = null;
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {

                    var sqlquery = session.CreateSQLQuery(query)
                         .SetResultTransformer(Transformers.AliasToBean<Item_Result>());
                    items = sqlquery.List<Item_Result>();
                }
            }
            var json = JsonConvert.SerializeObject(items, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return Request.CreateResponse(HttpStatusCode.OK, json);

        }
        [HttpGet]
        [Route("category/{name}")]
        public HttpResponseMessage GetAllItemByName(string name)
        {
            var query = @"SELECT it.*,img.URL,usr.itronID CreatedBy,usr.Email FROM ITEM it
                        join dbo.image img on it.ImageID=img.ImageID
                        join dbo.[User] usr on usr.UserID=it.UserID

                        WHERE Category='" + name + "'"

                        ;
            string json = string.Empty;
            IList<Item_Result> items = null;
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {

                    var sqlquery = session.CreateSQLQuery(query)
                         .SetResultTransformer(Transformers.AliasToBean<Item_Result>());


                    items = sqlquery.List<Item_Result>();

                    var result = items.GroupBy(x => x.ItemID).Select(
                       x => new ItemResult
                       {
                           ItemID = x.Key,
                           Category = x.FirstOrDefault().Category,
                           Phone = x.FirstOrDefault().Phone,
                           Description = x.FirstOrDefault().Description,
                           Price = x.FirstOrDefault().Price,
                           Images = x.Select(obj => obj.URL).ToList(),
                           Title = x.FirstOrDefault().Title,
                           UserID = x.FirstOrDefault().userid,
                           CreatedBy = x.FirstOrDefault().CreatedBy,
                           Email = x.FirstOrDefault().Email,


                       }
               );
                   json=JsonConvert.SerializeObject(result, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                }
            }

//            var json = JsonConvert.SerializeObject(result, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return Request.CreateResponse(HttpStatusCode.OK, json);

        }

        // POST api/values
        [HttpPost]
        [Route("")]
        public HttpResponseMessage AddItem([FromBody]ItemRequest value)
        {
            int id=0;

            Item item = null;
            int count = 0;
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {

                   
                    foreach (var image in value.Images)
                    {
                        item = new Item();
                        item.Phone = value.Phone;
                        item.Description = value.Description;
                        item.Price = value.Price;
                        item.Title = value.Title;
                        item.userid = value.UserID;
                        item.ImageID = image;
                        item.Category = value.Category;
                        //if (id!=0)
                        //{
                        //    session.CreateSQLQuery("SET IDENTITY_INSERT dbo.Item ON").UniqueResult();
                        //    item.ItemID = id;
                        //}
                        session.Save(item);
                        //id = item.ItemID;

                    }
                    transaction.Commit();
                    id = item.ItemID;
                }
            }

            string message = "http://itronxchangeapi.azurewebsites.net/api/items/" + id;
            return Request.CreateResponse(HttpStatusCode.Created, message);




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
                    item.ImageID = value.ImageID;
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
