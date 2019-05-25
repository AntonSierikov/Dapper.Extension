using Dapper.Extension.Abstract;
using Dapper.Extension.Enums;
using Dapper.Extension.Test.TestEntities;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dapper.Extension.Test.Tests.Base.Fixtures
{
    public abstract class BaseCRUDFixture : BaseDatabaseFixture
    {
        public BaseCRUDFixture(SqlProvider provider) 
            : base(provider)
        {}


        //----------------------------------------------------------------//

        #region Tests

        [Test]
        public async Task Test_Insert()
        {
            Console.WriteLine($"{nameof(Test_Insert)} is started");

            //arrange
            Int32 countryCode = 49;
            String countryName = "Germany";
            String iso_3166 = "GER";
            Country country = new Country(countryCode, countryName, iso_3166);
            Int32 insertedKey = default(Int32);
            String errorMessage = null;

            //act
            try
            {
                ICommand<Country, Int32> countryCommand = Session.CreateBaseCommand<Country, Int32>();
                insertedKey = await countryCommand.InsertAsync(country);
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
            }

            //assert
            Assert.IsTrue(String.IsNullOrEmpty(errorMessage));
            Assert.AreEqual(insertedKey, countryCode);

            Console.WriteLine($"{nameof(Test_Insert)} is finished");
        }

        //----------------------------------------------------------------//

        [Test]
        public async Task Test_Update()
        {
            Console.WriteLine($"{nameof(Test_Update)} is started");

            //arrange
            Country entity = new Country(380, "Ukraine", "UA");
            entity.ISO_3166_code = "UA_UPD";
            String errorMessage = null;
            Int32 rowsUpdated = default(Int32);
            String getQuery = $"SELECT * FROM country WHERE country_code = @{entity.CountryCode}";
            Country updatedEntity = null;

            //act 
            try
            {
                ICommand<Country, Int32> countryCommand = Session.CreateBaseCommand<Country, Int32>();
                rowsUpdated = await countryCommand.UpdateAsync(entity);
                updatedEntity = (await ExecuteQueryAsync<Country>(getQuery, entity)).First();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            //assert
            Assert.IsTrue(String.IsNullOrEmpty(errorMessage));
            Assert.AreEqual(rowsUpdated, 1);
            Assert.AreEqual(updatedEntity, entity);

            Console.WriteLine($"{nameof(Test_Update)} is finished");
        }

        //----------------------------------------------------------------//

        [Test]
        public async Task Test_Delete()
        {
            Console.WriteLine($"{nameof(Test_Delete)} is started");

            //arrage
            Int32 countryCode = 380;
            String errorMessage = null;
            Int32 rowsDeleted = default(Int32);
            String getQuery = $"SELECT * FROM country WHERE country_code = @{nameof(countryCode)}";
            Country country = null;

            //act
            try
            {
                ICommand<Country, Int32> command = Session.CreateBaseCommand<Country, Int32>();
                Object parameters = new { CountryCode = countryCode };
                rowsDeleted = await command.DeleteAsync(parameters);
                country = (await ExecuteQueryAsync<Country>(getQuery, parameters)).FirstOrDefault();
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
            }

            //assert
            Assert.IsTrue(String.IsNullOrEmpty(errorMessage));
            Assert.AreEqual(rowsDeleted, 1);
            Assert.IsNull(country);

            Console.WriteLine($"{nameof(Test_Delete)} is finished");
        }

        //----------------------------------------------------------------//

        [Test]
        public async Task Test_Get()
        {
            Console.WriteLine($"{nameof(Test_Get)} is started");

            //arrange
            Int32 countryCode = 380;
            String errorMessage = null;
            Country expectedCountry = new Country(countryCode, "Ukraine", "UA");
            Country selectedCountry = null;

            //act
            try
            {
                IQuery<Country> query = Session.CreateBaseQuery<Country>();
                selectedCountry = await query.GetAsync(new { CountryCode = countryCode });
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            //assert
            Assert.IsTrue(String.IsNullOrEmpty(errorMessage));
            Assert.AreEqual(expectedCountry, selectedCountry);

            Console.WriteLine($"{nameof(Test_Get)} is finished");
        }

        //----------------------------------------------------------------//

        #endregion

    }
}
