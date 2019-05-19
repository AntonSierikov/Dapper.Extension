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
                (colAttr, property) => new ColumnDefinition(colAttr.ColumnName, property.Name, colAttr.IsNeedUseDefaultValue),
                (property) => new ColumnDefinition(property.Name, property.Name));
        }

        //----------------------------------------------------------------//

        internal static Dictionary<String, PrimaryKeyDefinition> GetPrimaryKeyByPropertiesInfo(PropertyInfo[] propertiesInfo)
        {
            return GetPropertyEntityMap<PrimaryKeyDefinition, PrimaryKeyAttribute>(
                propertiesInfo,
                (prKeyAttr, property) => new PrimaryKeyDefinition(
                    prKeyAttr.ColumnName, 
                    property.Name,
                    prKeyAttr.IsNeedUseDefaultValue,
                    prKeyAttr.PrimaryKeyConstraintName));

        }

        #endregion

        #region Private Methods

        //----------------------------------------------------------------//

        public static Dictionary<String, TEntity> GetPropertyEntityMap<TEntity, TAttribute>(
            PropertyInfo[] propertiesInfo, 
            Func<TAttribute, PropertyInfo, TEntity> mapByPropertyAttributeIfExist, 
            Func<PropertyInfo, TEntity> mapByProperty = null) 
            where TAttribute: Attribute
        {
            Dictionary<String, TEntity> propertyEntityMap = new Dictionary<String, TEntity>();

            foreach(PropertyInfo propertyInfo in propertiesInfo)
            {
                TEntity mapEntity = GetEntityMap(propertyInfo, mapByPropertyAttributeIfExist, mapByProperty);

                if(!Object.Equals(mapEntity, default(TEntity)))
                {
                    propertyEntityMap[propertyInfo.Name] = mapEntity;
                }
            }

            return propertyEntityMap;
        }

        //----------------------------------------------------------------//

        public static TEntity GetEntityMap<TEntity, TAttribute>(
            PropertyInfo propertyInfo, 
            Func<TAttribute, PropertyInfo, TEntity> mapByPropertyAttributeIfExist,
            Func<PropertyInfo, TEntity> mapByProperty)
            where TAttribute: Attribute
        {
            TEntity entity = default(TEntity);
            TAttribute columnAttribute = propertyInfo.GetCustomAttribute<TAttribute>();

            if(columnAttribute != null)
            {
                entity = mapByPropertyAttributeIfExist(columnAttribute, propertyInfo);
            }
            else if(mapByProperty != null)
            {
                entity = mapByProperty(propertyInfo);
            }

            return entity;
        }

        //----------------------------------------------------------------//

        #endregion
    }
}
