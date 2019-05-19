using Dapper.Extension.Entities;
using Dapper.Extension.SqlGenerators;
using Dapper.Extension.Test.TestEntities;
using Dapper.Extension.TypeExtensions;
using NUnit.Framework;
using System;

namespace Dapper.Extension.Test.Tests.Base.Fixtures
{
    abstract class BaseCountryTestSQLGeneratorFixture
    {
        //----------------------------------------------------------------//

        private readonly BaseSqlGenerator Generator;
        private readonly DatabaseTypeInfo CountryTypeInfo;
        private readonly String CountryTableName;

        //----------------------------------------------------------------//

        public BaseCountryTestSQLGeneratorFixture(BaseSqlGenerator generator, String countryTableName)
        {
            Generator = generator;
            CountryTypeInfo = new DatabaseTypeInfo(typeof(Country));
            CountryTableName = countryTableName;
        }

        //----------------------------------------------------------------//

        [Test]
        public void Test_Generate_Insert_Query()
        {
            //arrange 
            String arrangeQuery = $@"INSERT INTO {CountryTableName}(country_code, country_name, iso_3166_code)
                                     VALUES (@{nameof(Country.CountryCode)}, @{nameof(Country.CountryName)}, @{nameof(Country.ISO_3166_code)})
                                     RETURNING (country_code)";

            //act
            String insertResultQuery = Generator.InsertQuery(CountryTypeInfo);

            //assert
            Assert.AreEqual(arrangeQuery.RemoveSpaces().RemoveNewLines(), 
                            insertResultQuery.RemoveSpaces().RemoveNewLines());
        }

        //----------------------------------------------------------------//

        [Test]
        public void Test_Generate_Update_Query()
        {
            //arrange
            String arrangeQuery = $@"UPDATE {CountryTableName} SET 
                                     country_code = @{nameof(Country.CountryCode)},
                                     country_name = @{nameof(Country.CountryName)},
                                     iso_3166_code = @{nameof(Country.ISO_3166_code)}
                                     WHERE country_code = @{nameof(Country.CountryCode)}";

            //act
            String updateResultQuery = Generator.UpdateQuery(CountryTypeInfo);

            //assert
            Assert.AreEqual(arrangeQuery.RemoveSpaces().RemoveNewLines(),
                            updateResultQuery.RemoveSpaces().RemoveNewLines());
        }

        //----------------------------------------------------------------//

        [Test]
        public void Test_Generate_Delete_Query()
        {
            //arrange
            Int32 CountryCode = 380;
            String arrangeQuery = $"DELETE FROM {CountryTableName} WHERE country_code = @{nameof(Country.CountryCode)}";

            //act
            String deleteResultQuery = Generator.DeleteQuery(CountryTypeInfo, new { CountryCode });

            //assert
            Assert.AreEqual(arrangeQuery.RemoveSpaces().RemoveNewLines(), 
                            deleteResultQuery.RemoveSpaces().RemoveNewLines());
        }

        //----------------------------------------------------------------//


    }
}
