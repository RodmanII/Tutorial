using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Tutorial.Utils
{
    public static class ExtensionMethod
    {
        public static List<T> Translate<T>(this DbSet<T> set, DbDataReader reader) where T : class
        {
            var entityList = new List<T>();
            if (reader == null || reader.HasRows == false) return entityList;
            var entityType = set.GetService<IModel>().FindEntityType(typeof(T));
            var valueBufferParameter = Expression.Parameter(typeof(ValueBuffer));
            var entityMaterializerSource = set.GetService<IEntityMaterializerSource>();
            var valueBufferFactory = set.GetService<IRelationalValueBufferFactoryFactory>().Create(new[] { typeof(T) }, null);
            var stateManager = set.GetService<IStateManager>() as StateManager;
            Func<ValueBuffer, T> materializer = Expression.Lambda<Func<ValueBuffer, T>>(
                    entityMaterializerSource.CreateMaterializeExpression(entityType, valueBufferParameter), valueBufferParameter)
                .Compile();
            stateManager.BeginTrackingQuery();
            while (reader.Read())
            {
                ValueBuffer valueBuffer = valueBufferFactory.Create(reader);
                var entity = materializer.Invoke(valueBuffer);
                var entry = stateManager.StartTrackingFromQuery(entityType, entity, valueBuffer, null);
                entityList.Add((T)entry.Entity);
            }
            return entityList;
        }
    }
}
