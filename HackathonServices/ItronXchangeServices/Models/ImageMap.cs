using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItronXchangeServices.Models
{
    public class ImageMap : ClassMap<Image>
    {
        public ImageMap()
        {
            Schema("dbo");
            Table("Image");
            Id(x => x.ImageID).Column("ImageID");
            Map(x => x.Description).Column("Description");
            Map(x => x.Resolution).Column("Resolution");
            Map(x => x.URL).Column("URL");
           


        }
    }
}