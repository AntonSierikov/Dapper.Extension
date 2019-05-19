using Dapper.Extension.Attributes;
using System;

namespace Dapper.Extension.Test.TestEntities
{
    [TableName("region")]
    class Region
    {
        [PrimaryKey("iso_3166_2_code")]
        public String ISO_3166_2_code { get; set; }

        [Column("region_name")]
        public String RegionName { get; set; }
    }
}
