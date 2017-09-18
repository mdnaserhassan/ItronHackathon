using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItronXchangeServices.Models
{
    public class Image
    {
        public virtual int ImageID { get; set; }
        public virtual string URL { get; set; }
        public virtual string Description { get; set; }
        public virtual string Resolution { get; set; }
       

    }
   

}