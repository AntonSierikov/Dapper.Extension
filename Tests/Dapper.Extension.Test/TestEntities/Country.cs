﻿using Dapper.Extension.Attributes;
using System;

namespace Dapper.Extension.Test.TestEntities
{
    [TableName("country")]
    class Country
    {
        [PrimaryKey("country_code")]
        public Int32 CountryCode { get; set; }

        [Column("country_name")]
        public String CountryName { get; set; }

        [Column("iso_3166_code")]
        public String ISO_3166_code { get; set; }

        //----------------------------------------------------------------//

        public Country() { }

        //----------------------------------------------------------------//

        public Country(Int32 countryCode, String countryName, String iso_3166)
        {
            CountryCode = countryCode;
            CountryName = countryName;
            ISO_3166_code = iso_3166;
        }

        //----------------------------------------------------------------//

    }
}
