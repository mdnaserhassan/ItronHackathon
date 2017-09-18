using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.WindowsAzure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using Microsoft.Azure; //Namespace for CloudConfigurationManager
using ItronXchangeServices.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ItronXchangeServices.Controllers
{
    [RoutePrefix("api/Upload")]
    public class UploadController : ApiController
    {


        [Route("PostUserImage")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<HttpResponseMessage> PostImage()
        {
            Guid g;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
    CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("images");
            Dictionary<string, object> dict = new Dictionary<string, object>();
            List<int> images = new List<int>();
            string json = string.Empty;
            Image img=null;
            try
            {

                var httpRequest = HttpContext.Current.Request;
                string filename = string.Empty;
                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 10; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                             
                        }
                        else
                        {

                            g = Guid.NewGuid();
                            g.ToString();
                            filename = g.ToString()+ postedFile.FileName;
                             
                            //var filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + postedFile.FileName + extension);
                            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
                            //postedFile.SaveAs(filePath);
                               
                            blockBlob.UploadFromStream(postedFile.InputStream);
                            using (var session = NHibernateHelper.OpenSession())
                            {
                                using (var transaction = session.BeginTransaction())
                                {
                                   img= new Image() { URL = filename };
                                    session.Save(img);
                                    transaction.Commit();

                                }
                            }

                        }
                    }
                    images.Add(img.ImageID);
                    
                    json = JsonConvert.SerializeObject(images, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                    var message1 = string.Format("Image Updated Successfully at {0}.", filename);
                    
                    
                }
                
                if (httpRequest.Files.Count==0)
                {
                    var res = string.Format("Please Upload a image.");
                    dict.Add("error", res);
                    return Request.CreateResponse(HttpStatusCode.NotFound, dict);
                }
                else
                {
                    json = JsonConvert.SerializeObject(images, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                    return Request.CreateErrorResponse(HttpStatusCode.Created, json);
                }
               
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }


        }


    }
    //class Image
    //{
    //    public string Path { get; set; }

    //}
}
