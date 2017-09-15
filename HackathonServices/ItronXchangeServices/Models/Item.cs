using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItronXchangeServices.Models
{
    public class Item
    {
        public virtual int ItemID { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual int Price  { get; set; }
        public virtual string ImageIDs { get; set; }
        public virtual string Category { get; set; }
        
    }
   

}