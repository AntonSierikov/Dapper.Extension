using Dapper.Extension.Test.TestEntities;
using NUnit.Framework;

namespace Dapper.Extension.Test.Tests.Base.FixtureSources
{
    internal class CountrySchemaFixtureSource
    {

        //----------------------------------------------------------------//

        internal static TestFixtureData[] CountryFixtureSource
        {
            get
            {
                return new TestFixtureData[]
                {
                    new TestFixtureData(380, "Ukraine", "UA"),
                    new TestFixtureData(48, "Poland", "PL"),
                    new TestFixtureData(372, "Estonia", "EST")
                };
            }
        }

        //----------------------------------------------------------------//

    }
}
