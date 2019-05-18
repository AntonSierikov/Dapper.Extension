using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Extension.Entities;

namespace Dapper.Extension.SqlGenerators
{
    internal class SqlClientGenerator : BaseSqlGenerator
    {

        //----------------------------------------------------------------//

        public override string InsertQuery(DatabaseTypeInfo databaseTypeInfo)
        {
            throw new NotImplementedException();
        }

        //----------------------------------------------------------------//

    }
}
