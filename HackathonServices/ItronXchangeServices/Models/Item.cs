using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItronXchangeServices.Models
{
    public class Item_Result
    {
        public virtual int ItemID { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual int Price  { get; set; }
        public virtual int ImageID { get; set; }
        public virtual string URL { get; set; }
        public virtual string Category { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string Email { get; set; }
        public virtual string userid { get; set; }
        //public List<string> Images { get; set; }

    }
    public class Item
    {
        public virtual int ItemID { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual int Price { get; set; }
       
        public virtual string Category { get; set; }
        public virtual int ImageID { get; set; }
        //public virtual string Email { get; set; }
        public virtual string userid { get; set; }
        public virtual List<string> Images { get; set; }

    }

    public class ItemRequest
    {
        public virtual int ItemID { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual int Price { get; set; }

        public virtual string Category { get; set; }
        public virtual List<int> Images { get; set; }
        //public virtual string Email { get; set; }
        public virtual string UserID { get; set; }
        //public List<string> Images { get; set; }

    }


}