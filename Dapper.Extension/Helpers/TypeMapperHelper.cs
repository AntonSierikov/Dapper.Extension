using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Dapper.Extension.Attributes;
using Dapper.Extension.Entities;

namespace Dapper.Extension.Helpers
{
    public static class TypeMapperHelper
    {

        #region Public Methods 

        //----------------------------------------------------------------//


        internal static Dictionary<String, ColumnDefinition> GetFieldByPropertiesInfo(PropertyInfo[] propertiesInfo)
        {
            return GetPropertyEntityMap<ColumnDefinition, ColumnAttribute>(
                propertiesInfo,
                (colAttr) => new ColumnDefinition(colAttr.ColumnName, colAttr.IsNeedUseDefaultValue),
                (property) => new ColumnDefinition(property.Name)).ToDictionary(k => k.Key, v => v.Value);
        }

        //----------------------------------------------------------------//

        internal static Dictionary<String, PrimaryKeyDefinition> GetPrimaryKeyByPropertiesInfo(PropertyInfo[] propertiesInfo)
        {
            return GetPropertyEntityMap<PrimaryKeyDefinition, PrimaryKeyAttribute>(
                propertiesInfo,
                (prKeyAttr) => new PrimaryKeyDefinition(prKeyAttr.ColumnName, prKeyAttr.IsNeedUseDefaultValue, prKeyAttr.PrimaryKeyConstraintName),
                (property) => new PrimaryKeyDefinition(property.Name)).ToDictionary(k => k.Key, v => v.Value);

        }

        #endregion

        #region Private Methods

        //----------------------------------------------------------------//

        public static IEnumerable<KeyValuePair<String, TEntity>> GetPropertyEntityMap<TEntity, TAttribute>(
            PropertyInfo[] propertiesInfo, 
            Func<TAttribute, TEntity> mapByPropertyAttributeIfExist, 
            Func<PropertyInfo, TEntity> mapByProperty) 
            where TAttribute: Attribute
        {
            return propertiesInfo.ToDictionary(p => p.Name, p => GetEntityMap(p, mapByPropertyAttributeIfExist, mapByProperty))
                                 .Where(p => p.Equals(default(KeyValuePair<String, TEntity>)));
        }

        //----------------------------------------------------------------//

        public static TEntity GetEntityMap<TEntity, TAttribute>(
            PropertyInfo propertyInfo, 
            Func<TAttribute, TEntity> mapByPropertyAttributeIfExist,
            Func<PropertyInfo, TEntity> mapByProperty)
            where TAttribute: Attribute
        {
            TEntity entity;
            TAttribute columnAttribute = propertyInfo.GetCustomAttribute<TAttribute>();

            if(columnAttribute != null)
            {
                entity = mapByPropertyAttributeIfExist(columnAttribute);
            }
            else 
            {
                entity = mapByProperty(propertyInfo);
            }

            return entity;
        }

        //----------------------------------------------------------------//

        #endregion
    }
}
