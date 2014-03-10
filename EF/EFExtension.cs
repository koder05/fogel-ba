using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace EF
{
    public static class EFExtension
    {
        /// <summary>
        /// Tuple as {EntityState, EntityName, KeyValue, PropertyName, NewPropValue, OldPropValue}
        /// </summary>
        public static IEnumerable<Tuple<string, string, object, string, object, object>> GetSavedValues(this DbContext context)
        {
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            context.ChangeTracker.DetectChanges();
            return GetSavedValues(objectContext, EntityState.Added, (e) => GetAddedEntityValues(e))
                .Union(GetSavedValues(objectContext, EntityState.Modified, (e) => GetUpdatedEntityValues(e)))
                .Union(GetSavedValues(objectContext, EntityState.Deleted, (e) => GetDeletedEntityValues(e)));
        }


        private static IEnumerable<Tuple<string, string, object, string, object, object>> GetSavedValues(ObjectContext context, EntityState state
            , Func<ObjectStateEntry, IEnumerable<Tuple<string, object, object>>> getValues)
        {
            return context
                .ObjectStateManager
                .GetObjectStateEntries(state)
                .SelectMany(
                    e =>
                    {
                        string entityname = e.EntitySet.Name;
                        object id = null;
                        if (e.EntityKey != null && e.EntityKey.EntityKeyValues != null)
                            id = e.EntityKey.EntityKeyValues.Select(k => k.Value).FirstOrDefault();
                        return getValues(e).Select(t => Tuple.Create(state.ToString(), entityname, id, t.Item1, t.Item2, t.Item3));
                    }
               );
        }

        private static IEnumerable<Tuple<string, object, object>> GetAddedEntityValues(ObjectStateEntry entry)
        {
            foreach (var propinfo in entry.CurrentValues.DataRecordInfo.FieldMetadata)
            {
                object newval = entry.CurrentValues[propinfo.FieldType.Name];
                if (newval.GetType() == typeof(EntityKey))
                {
                    newval = ((EntityKey)newval).EntityKeyValues.Select(k => k.Value).FirstOrDefault();    
                }
                yield return Tuple.Create(propinfo.FieldType.Name, newval, (object)null);
            }
        }

        private static IEnumerable<Tuple<string, object, object>> GetUpdatedEntityValues(ObjectStateEntry entry)
        {
            foreach (string propname in entry.GetModifiedProperties())
            {
                object newval = entry.CurrentValues[propname];
                object oldval = entry.OriginalValues[propname];
                yield return Tuple.Create(propname, newval, oldval);
            }
        }

        private static IEnumerable<Tuple<string, object, object>> GetDeletedEntityValues(ObjectStateEntry entry)
        {
            yield return Tuple.Create(string.Empty, (object)null, (object)null);
        }
    }
}
