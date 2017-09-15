using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItronXchangeServices.Models
{
    public class ItemMap : ClassMap<Item>
    {
        public ItemMap()
        {
            Schema("dbo");
            Table("Item");
            Id(x => x.ItemID).Column("ItemID");
            Map(x => x.ImageIDs).Column("ImageIDs");
            Map(x => x.Category).Column("Category");
            Map(x => x.Description).Column("Description");
            Map(x => x.Phone).Column("Phone");
            Map(x => x.Price).Column("Price");
            Map(x => x.Title).Column("Title");


        }
    }
}