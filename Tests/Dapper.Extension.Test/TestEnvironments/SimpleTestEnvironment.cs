using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.Extension.Test.TestEnvironments
{
    public class SimpleTestEnvironment
    {
        public DatabaseAccessor DatabaseAccessor { get;  }

        //----------------------------------------------------------------//

        public SimpleTestEnvironment()
        {
            DatabaseAccessor = null;
        }

        //----------------------------------------------------------------//


    }
}
