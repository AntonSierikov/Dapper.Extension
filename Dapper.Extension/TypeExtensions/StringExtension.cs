using Dapper.Extension.Constants;
using System;

namespace Dapper.Extension.TypeExtensions
{
    public static class StringExtension
    {

        //----------------------------------------------------------------//

        public static String RemoveSpaces(this String str)
        {
            return str.Replace(StringConstants.SPACE, String.Empty);
        }

        //----------------------------------------------------------------//

        public static String RemoveNewLines(this String str)
        {
            return str.Replace(Environment.NewLine, String.Empty);
        }

        //----------------------------------------------------------------//

    }
}
