using Dapper.Extension.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace Dapper.Extension.TypeMappers
{
    public class TypeMapper : SqlMapper.ITypeMap
    {
        private readonly DefaultTypeMap _defaultTypeMap;
        private readonly CustomPropertyTypeMap _customPropertyTypeMap;

        //----------------------------------------------------------------//

        public TypeMapper(Type type)
        {
            _defaultTypeMap = new DefaultTypeMap(type);
            _customPropertyTypeMap = new CustomPropertyTypeMap(type, SelectProperty);
        }

        //----------------------------------------------------------------//

        #region public methods

        public ConstructorInfo FindConstructor(String[] names, Type[] types)
        {
            return _defaultTypeMap.FindConstructor(names, types);
        }

        //----------------------------------------------------------------//

        public ConstructorInfo FindExplicitConstructor()
        {
            return _defaultTypeMap.FindExplicitConstructor();
        }

        //----------------------------------------------------------------//

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, String columnName)
        {
            return _defaultTypeMap.GetConstructorParameter(constructor, columnName);
        }

        //----------------------------------------------------------------//

        public SqlMapper.IMemberMap GetMember(String columnName)
        {
            return _customPropertyTypeMap.GetMember(columnName);
        }

        #endregion

        //----------------------------------------------------------------//

        #region private methods

        private PropertyInfo SelectProperty(Type type, String columnName)
        {
            PropertyInfo[] propertiesInfo = type.GetProperties();
            return propertiesInfo.FirstOrDefault(p => HasPropertyColumnAttribute(p, columnName)) ??
                   propertiesInfo.FirstOrDefault(p => p.Name.Equals(columnName));
        }

        //----------------------------------------------------------------//

        private Boolean HasPropertyColumnAttribute(PropertyInfo info, String columnName)
        {
            return (info.GetCustomAttribute<ColumnAttribute>()?.ColumnName).Equals(columnName);
        }

        //----------------------------------------------------------------//

        #endregion
    }
}
