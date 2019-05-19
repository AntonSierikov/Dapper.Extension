using Dapper.Extension.Abstract;
using Dapper.Extension.Enums;
using Dapper.Extension.Test.TestEntities;
using NUnit.Framework;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Dapper.Extension.Test.Tests.Base.Fixtures
{
    public abstract class BaseCRUDFixture : BaseDatabaseFixture
    {
        public BaseCRUDFixture(SqlProvider provider, DbProviderFactory providerFactory) 
            : base(provider, providerFactory)
        {}


        //----------------------------------------------------------------//

        #region Tests

        [Test]
        public async Task Test_Insert()
        {
            //arrange
            Int32 countryCode = 380;
            String countryName = "Ukraine";
            String iso_3166 = "UA";
            Country country = new Country(countryCode, countryName, iso_3166);

            //act
            ICommand<Country, Int32> countryCommand = Session.CreateBaseCommand<Country, Int32>();
            Int32 insertedKey = await countryCommand.InsertAsync(country);

            //assert
            Assert.AreEqual(insertedKey, countryCode);
        }

        //----------------------------------------------------------------//


        #endregion

    }
}
