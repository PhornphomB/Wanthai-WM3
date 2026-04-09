using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Objects;
using System.Data.Common;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;

using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Data.Entity.Core.Metadata.Edm;

namespace Prototype.Providers
{
    public static class ContextExtension
    {
        #region Transaction


        public static void UndoingChangesDbContextLevel(this DbContext context)
        {
            var entries = context.ChangeTracker.Entries();

            UndoingChangesDbContextLevel(context, entries);
        }
        public static void UndoingChangesDbContextLevel(this DbContext context, System.Data.Entity.EntityState _state)
        {
            var entries = context.ChangeTracker.Entries().Where(x => x.State == _state);

            UndoingChangesDbContextLevel(context, entries);
        }
        private static void UndoingChangesDbContextLevel(this DbContext context, IEnumerable<DbEntityEntry> _entries)
        {
            foreach (DbEntityEntry entry in _entries)
            {
                switch (entry.State)
                {
                    case System.Data.Entity.EntityState.Added:
                        entry.State = System.Data.Entity.EntityState.Detached;
                        break;
                    case System.Data.Entity.EntityState.Modified:
                        entry.State = System.Data.Entity.EntityState.Unchanged;
                        break;
                    case System.Data.Entity.EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }


        public static bool Save(this DbContext _objContext, dynamic _object, Func<int> _statement)
        {
            return Save(_objContext, _object, _statement, global::Prototype.Providers.Message.Message_SaveSuccess);
        }
        public static bool Save(this DbContext _objContext, dynamic _object, Func<int> _statement, string _customMessage, string _concurrencyMessage = "")
        {
            try
            {
                _statement();

                if (!string.IsNullOrEmpty(_customMessage))
                {
                    _object.Logging = new Prototype.Providers.Logging(_object,
                        System.Diagnostics.EventLogEntryType.SuccessAudit,
                        1010,
                        _customMessage);
                    _object.RaiseLogging();
                }

                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (_concurrencyMessage == "")
                    _concurrencyMessage = Message.Message_Concurrency;

                _object.Logging = new Prototype.Providers.Logging(_object,
                       System.Diagnostics.EventLogEntryType.Warning,
                       1010,
                       _concurrencyMessage);
                _object.RaiseLogging();

                return false;
            }
            catch (DbEntityValidationException e)
            {
                var msg = string.Empty;
                var row = 1;

                foreach (var eve in e.EntityValidationErrors)
                {
                    msg += String.Format("\"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    msg += String.Format(Environment.NewLine + "Record {0} follow error" + Environment.NewLine, row);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        msg += String.Format("field: \"{0}\" {1}, " + Environment.NewLine, ve.PropertyName, ve.ErrorMessage.TrimEnd('.'));
                    }

                    msg = msg.TrimEnd().TrimEnd(',') + ".";
                    row++;
                }

                throw new Exception(msg);
            }
            catch (Exception exception)
            {
                _object.Logging = new Prototype.Providers.Logging(_object, exception,
                   System.Diagnostics.EventLogEntryType.Error,
                   1010,
                   global::Prototype.Providers.Message.Message_SaveFailure.LoggingText((object)_object),
                   global::Prototype.Providers.Message.Message_TryOrContact);
                _object.RaiseLogging();

                return false;
            }
        }

        public static bool Update(this DbContext _objContext, dynamic _object, Func<int> _statement)
        {
            return Update(_objContext, _object, _statement, global::Prototype.Providers.Message.Message_UpdateSuccess);
        }
        public static bool Update(this DbContext _objContext, dynamic _object, Func<int> _statement, string _customMessage, string _concurrencyMessage = "")
        {
            try
            {
                _statement();

                if (!string.IsNullOrEmpty(_customMessage))
                {
                    _object.Logging = new Prototype.Providers.Logging(_object,
                        System.Diagnostics.EventLogEntryType.SuccessAudit,
                        1020,
                        _customMessage);
                    _object.RaiseLogging();
                }

                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (_concurrencyMessage == "")
                    _concurrencyMessage = Message.Message_Concurrency;

                _object.Logging = new Prototype.Providers.Logging(_object,
                       System.Diagnostics.EventLogEntryType.Warning,
                       1010,
                       _concurrencyMessage);
                _object.RaiseLogging();

                return false;
            }
            catch (DbEntityValidationException e)
            {
                var msg = string.Empty;
                var row = 1;

                foreach (var eve in e.EntityValidationErrors)
                {
                    msg += String.Format("\"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    msg += String.Format(Environment.NewLine + "Record {0} follow error" + Environment.NewLine, row);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        msg += String.Format("field: \"{0}\" {1}, " + Environment.NewLine, ve.PropertyName, ve.ErrorMessage.TrimEnd('.'));
                    }

                    msg = msg.TrimEnd().TrimEnd(',') + ".";
                    row++;
                }

                throw new Exception(msg);
            }
            catch (Exception exception)
            {
                _object.Logging = new Prototype.Providers.Logging(_object, exception,
                    System.Diagnostics.EventLogEntryType.Error,
                    1020,
                    global::Prototype.Providers.Message.Message_UpdateFailure.LoggingText((object)_object),
                    global::Prototype.Providers.Message.Message_TryOrContact);
                _object.RaiseLogging();

                return false;
            }
        }

        public static bool Delete(this DbContext _objContext, dynamic _object, Func<int> _statement)
        {
            return Update(_objContext, _object, _statement, global::Prototype.Providers.Message.Message_DeleteSuccess);
        }
        public static bool Delete(this DbContext _objContext, dynamic _object, Func<int> _statement, string _customMessage, string _concurrencyMessage = "")
        {
            try
            {
                var rows = _statement();
                _customMessage = string.IsNullOrEmpty(_customMessage) ? global::Prototype.Providers.Message.Message_DeleteSuccess : _customMessage;
                _object.Logging = new Prototype.Providers.Logging(_object,
                    System.Diagnostics.EventLogEntryType.SuccessAudit,
                    1010, _customMessage);
                _object.RaiseLogging();

                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (_concurrencyMessage == "")
                    _concurrencyMessage = Message.Message_Concurrency;

                _object.Logging = new Prototype.Providers.Logging(_object,
                       System.Diagnostics.EventLogEntryType.Warning,
                       1010,
                       _concurrencyMessage);
                _object.RaiseLogging();

                return false;
            }
            catch (DbEntityValidationException e)
            {
                var msg = string.Empty;
                var row = 1;

                foreach (var eve in e.EntityValidationErrors)
                {
                    msg += String.Format("\"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    msg += String.Format(Environment.NewLine + "Record {0} follow error" + Environment.NewLine, row);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        msg += String.Format("field: \"{0}\" {1}, " + Environment.NewLine, ve.PropertyName, ve.ErrorMessage.TrimEnd('.'));
                    }

                    msg = msg.TrimEnd().TrimEnd(',') + ".";
                    row++;
                }

                throw new Exception(msg);
            }
            catch (Exception exception)
            {
                _object.Logging = new Prototype.Providers.Logging(_object, exception,
                    System.Diagnostics.EventLogEntryType.Error,
                    1030,
                    global::Prototype.Providers.Message.Message_DeleteFailure.LoggingText((object)_object),
                    global::Prototype.Providers.Message.Message_TryOrContact);
                _object.RaiseLogging();

                return false;
            }
        }

        #endregion

        #region Functions Extension

        public static void CloneObject(this object _target, object _source)
        {
            _target.CloneObject(_source, false, new List<string>());
        }
        public static void CloneObject(this object _target, object _source, bool _copyNullValue)
        {
            _target.CloneObject(_source, _copyNullValue, new List<string>());
        }
        public static void CloneObject(this object _target, object _source, bool _copyNullValue
            , List<string> _fieldNameSkip)
        {
            if ((_target.IsNull()) || (_source.IsNull())) return;

            var propertyList = TypeDescriptor.GetProperties(_target);
            var props = _target.GetType().GetProperties().AsEnumerable().Where(x => !_fieldNameSkip.Contains(x.Name));

            // Filter EF6 
            props = props.Where(x => x.Module.Name != "<In Memory Module>");

            foreach (var prop in props)
            {
                var propertyDesc = propertyList.Find(prop.Name, true);
                if (propertyDesc.NotNull())
                {

                    object sourceValue = _source.GetPropertyValue(prop);
                    if ((sourceValue.NotNull()) || (_copyNullValue == true))
                        propertyDesc.SetValue(_target, sourceValue);
                }
            }
        }

        public static void CloneObjectByInterface<IInterface>(this IInterface _target, IInterface _source, bool _copyNullValue
    , List<string> _fieldNameSkip = null)
        {
            if ((_target.IsNull()) || (_source.IsNull())) return;

            var propertyList = TypeDescriptor.GetProperties(_target);
            var props = GetInterfaceProperties(typeof(IInterface));

            if (_fieldNameSkip != null)
                props = props.Where(x => !_fieldNameSkip.Contains(x.Name));

            // Filter EF6 
            props = props.Where(x => x.Module.Name != "<In Memory Module>");

            foreach (var prop in props)
            {
                var propertyDesc = propertyList.Find(prop.Name, true);
                if (propertyDesc.NotNull())
                {

                    object sourceValue = _source.GetPropertyValue(prop);
                    if ((sourceValue.NotNull()) || (_copyNullValue == true))
                        propertyDesc.SetValue(_target, sourceValue);
                }
            }
        }

        public static IEnumerable<PropertyInfo> GetInterfaceProperties(this Type type)
        {
            if (!type.IsInterface)
                return type.GetProperties();

            return (new Type[] { type })
                   .Concat(type.GetInterfaces())
                   .SelectMany(i => i.GetProperties());
        }

        public static void SetPropertyValue(this object _component, string _propertyName, object _value)
        {
            if (_component.IsNull()) return;
            var propertyList = System.ComponentModel.TypeDescriptor.GetProperties(_component);

            var prop = _component.GetType().GetProperties().FirstOrDefault(qry => qry.Name == _propertyName);
            if (prop.NotNull())
            {
                var propertyDesc = propertyList.Find(prop.Name, true);
                if (propertyDesc.NotNull())
                    propertyDesc.SetValue(_component, _value);
            }
        }

        public static object GetPropertyValue(this object _component, string _propertyName)
        {
            if (_component.IsNull()) return null;
            var propertyList = System.ComponentModel.TypeDescriptor.GetProperties(_component);

            var prop = _component.GetType().GetProperties().FirstOrDefault(qry => qry.Name == _propertyName);
            if (prop.NotNull())
            {
                var propertyDesc = propertyList.Find(prop.Name, true);
                if (propertyDesc.NotNull())
                    return propertyDesc.GetValue(_component);
            }

            return null;
        }

        public static object GetPropertyValue(this object _component, System.Reflection.PropertyInfo _propertyInfo)
        {
            if (_component.IsNull()) return null;
            var propertyList = System.ComponentModel.TypeDescriptor.GetProperties(_component);
            var prop = _component.GetType().GetProperties().FirstOrDefault(qry => qry.Name == _propertyInfo.Name);
            if (prop.NotNull())
            {
                var propertyDesc = propertyList.Find(prop.Name, true);
                if (propertyDesc.NotNull())
                    return propertyDesc.GetValue(_component);
            }

            return null;
        }

        #endregion


        public static EdmProperty[] GetEntityKeys(this DbContext context, Type entityType)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            // Get the mapping between CLR types and metadata OSpace
            var oc = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get metadata for given CLR type
            var entityMetadata = metadata.GetItems<EntityType>(DataSpace.OSpace).Single(e => oc.GetClrType(e) == entityType);

            return entityMetadata.KeyProperties.ToArray();
        }

        public static EdmProperty[] GetEntityAttrs(this DbContext context, Type entityType)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            var props = metadata.GetItems(DataSpace.SSpace).OfType<EntityType>().FirstOrDefault(x => x.Name == entityType.Name);

            return props.Properties.ToArray();
        }
    }

    public static class DataExtension
    {
        public static EntityT GetDataEntityBy<EntityT>(this object _objContext, dynamic _object, Func<EntityT> _statement)
        {
            try
            {
                return _statement();
            }
            catch (Exception exception)
            {
                _object.Logging = new Prototype.Providers.Logging(_object, exception,
                    System.Diagnostics.EventLogEntryType.Error,
                    1050,
                    global::Prototype.Providers.Message.Message_GetFailure.LoggingText((object)_object),
                    global::Prototype.Providers.Message.Message_TryOrContact);
                _object.RaiseLogging();

                return default(EntityT);
            }
        }
        public static List<EntityT> GetDataEntityListBy<EntityT>(this object _objContext, dynamic _object, Func<List<EntityT>> _statement)
        {
            try
            {
                return _statement();
            }
            catch (Exception exception)
            {
                _object.Logging = new Prototype.Providers.Logging(_object, exception,
                    System.Diagnostics.EventLogEntryType.Error,
                    1050,
                    global::Prototype.Providers.Message.Message_GetFailure.LoggingText((object)_object),
                    global::Prototype.Providers.Message.Message_TryOrContact);
                _object.RaiseLogging();

                return new List<EntityT>();
            }
        }
        public static dynamic GetDataBy(this object _objContext, dynamic _object, Func<dynamic> _statement)
        {
            try
            {
                return _statement();
            }
            catch (Exception exception)
            {
                _object.Logging = new Prototype.Providers.Logging(_object, exception,
                    System.Diagnostics.EventLogEntryType.Error,
                    1050,
                    global::Prototype.Providers.Message.Message_GetFailure.LoggingText((object)_object),
                    global::Prototype.Providers.Message.Message_TryOrContact);
                _object.RaiseLogging();

                return null;
            }
        }

        public static System.Data.DataTable GetDataTable(this object _objContext, dynamic _object, Func<DataTable> _statement)
        {
            try
            {
                return _statement();
            }
            catch (Exception exception)
            {
                _object.Logging = new Prototype.Providers.Logging(_object, exception,
                    System.Diagnostics.EventLogEntryType.Error,
                    1060,
                    global::Prototype.Providers.Message.Message_ListFailure,
                    global::Prototype.Providers.Message.Message_TryOrContact);
                _object.RaiseLogging();
                return null;
            }
        }
        public static System.Data.DataTable GetDataTable<T>(this object _objContext, dynamic _object, Func<IEnumerable<T>> _queryable)
        {
            try
            {
                return _queryable().ToDataTable();
            }
            catch (Exception exception)
            {
                _object.Logging = new Prototype.Providers.Logging(_object, exception,
                    System.Diagnostics.EventLogEntryType.Error,
                    1060,
                    global::Prototype.Providers.Message.Message_ListFailure,
                    global::Prototype.Providers.Message.Message_TryOrContact);
                _object.RaiseLogging();
                return null;
            }
        }
        public static DataTable GetDataTable<T>(this object _objContext, dynamic _object, IQueryable<T> _queryable)
        {
            try
            {
                return DataFilter.GetTable(_queryable, (_object.FilterList as FilterList));
            }
            catch (Exception exception)
            {
                _object.Logging = new Logging(_object, exception,
                    System.Diagnostics.EventLogEntryType.Error,
                    1060,
                    Message.Message_ListFailure.LoggingText((object)_object),
                    Message.Message_TryOrContact);
                _object.RaiseLogging();
                return null;
            }
        }
        public static DataTable GetDataTable<T>(this object _objContext, dynamic _object, IQueryable<T> _queryable, int _takeLimit)
        {
            try
            {
                return DataFilter.GetTable(_queryable, (_object.FilterList as FilterList), _takeLimit);
            }
            catch (Exception exception)
            {
                _object.Logging = new Logging(_object, exception,
                    System.Diagnostics.EventLogEntryType.Error,
                    1060,
                    Message.Message_ListFailure.LoggingText((object)_object),
                    Message.Message_TryOrContact);
                _object.RaiseLogging();
                return null;
            }
        }
        public static IQueryable<T> GetQueryable<T>(this object _objContext, dynamic _object, IQueryable<T> _queryable, bool _withFilter)
        {
            try
            {
                if (_withFilter)
                    return _queryable.ToFilterBy((_object.FilterList as FilterList));
                else
                    return _queryable;
            }
            catch (Exception exception)
            {
                _object.Logging = new Logging(_object, exception,
                    System.Diagnostics.EventLogEntryType.Error,
                    1060,
                    Message.Message_ListFailure.LoggingText((object)_object),
                    Message.Message_TryOrContact);
                _object.RaiseLogging();
                return null;
            }
        }
        public static IQueryable<T> GetObjectList<T>(this object _objContext, dynamic _object, IQueryable<T> _queryable)
        {
            try
            {
                return DataFilter.GetQuery(_queryable, (_object.FilterList as FilterList));
            }
            catch (Exception exception)
            {
                _object.Logging = new Logging(_object, exception,
                    System.Diagnostics.EventLogEntryType.Error,
                    1060,
                    Message.Message_ListFailure.LoggingText((object)_object),
                    Message.Message_TryOrContact);
                _object.RaiseLogging();
                return null;
            }
        }

        public static List<Prototype.Providers.Property> GetPropertyList(this object _objContext, dynamic _object, Func<dynamic> _statement)
        {
            try
            {
                _object.PropertyList = new List<Prototype.Providers.Property>();
                return _statement();
            }
            catch (Exception exception)
            {
                _object.Logging = new Prototype.Providers.Logging(_object, exception,
                    System.Diagnostics.EventLogEntryType.Error,
                    1070,
                    global::Prototype.Providers.Message.Message_PropertyListFailure,
                    global::Prototype.Providers.Message.Message_TryOrContact);
                _object.RaiseLogging();
                return null;
            }
        }
    }

    public static class MessageExtension
    {
        public static bool Existed(this object _objContext, dynamic _object, Func<bool> _statement)
        {
            return Existed(_objContext, _object, _statement, global::Prototype.Providers.Message.Message_Duplicated);
        }
        public static bool Existed(this object _objContext, dynamic _object, Func<bool> _statement, string _customMessage)
        {
            try
            {
                var hasExited = _statement();

                if (hasExited && !string.IsNullOrEmpty(_customMessage))
                {
                    _object.Logging = new Prototype.Providers.Logging(_object,
                      System.Diagnostics.EventLogEntryType.Warning,
                      1040, _customMessage);
                    _object.RaiseLogging();
                }

                return hasExited;
            }
            catch (Exception exception)
            {
                _object.Logging = new Prototype.Providers.Logging(_object, exception,
                    System.Diagnostics.EventLogEntryType.Error,
                    1040,
                    global::Prototype.Providers.Message.Message_ExistedFailure.LoggingText((object)_object),
                    global::Prototype.Providers.Message.Message_TryOrContact);
                _object.RaiseLogging();
                return false;
            }
        }

        public static bool Engaged(this object _objContext, dynamic _object, Func<bool> _statement)
        {
            return Engaged(_objContext, _object, _statement, global::Prototype.Providers.Message.Message_NoneActiveFailure);
        }
        public static bool Engaged(this object _objContext, dynamic _object, Func<bool> _statement, string _customMessage)
        {
            try
            {
                var hasEngage = _statement();

                if (hasEngage && !string.IsNullOrEmpty(_customMessage))
                {
                    _object.Logging = new Prototype.Providers.Logging(_object,
                      System.Diagnostics.EventLogEntryType.Warning,
                      1050,
                      _customMessage);
                    _object.RaiseLogging();
                }

                return hasEngage;
            }
            catch (Exception exception)
            {
                _object.Logging = new Prototype.Providers.Logging(_object, exception,
                    System.Diagnostics.EventLogEntryType.Error,
                    1050,
                    global::Prototype.Providers.Message.Message_ExistedFailure.LoggingText((object)_object),
                    global::Prototype.Providers.Message.Message_TryOrContact);
                _object.RaiseLogging();
                return false;
            }
        }

        public static void RaiseNotFound(this object _objContext, dynamic _object)
        {
            _object.Logging = new Prototype.Providers.Logging(_object,
                System.Diagnostics.EventLogEntryType.Warning,
                1080,
                global::Prototype.Providers.Message.Message_NotFound.LoggingText((object)_object),
                global::Prototype.Providers.Message.Message_TryOrContact);
            _object.RaiseLogging();
        }
        public static void RaiseDuplicate(this object _objContext, dynamic _object)
        {
            _object.Logging = new Prototype.Providers.Logging(_object,
                System.Diagnostics.EventLogEntryType.Warning,
                1080,
                global::Prototype.Providers.Message.Message_Duplicated.LoggingText((object)_object),
                global::Prototype.Providers.Message.Message_TryOrContact);
            _object.RaiseLogging();
        }

        public static void MessageInfo(this object _this, dynamic _thisObject, string _customMessage)
        {
            _thisObject.Logging = new Prototype.Providers.Logging(_thisObject,
                  System.Diagnostics.EventLogEntryType.Information,
                  0,
                  _customMessage);
            _thisObject.RaiseLogging();
        }
        public static void MessageSuccess(this object _this, dynamic _thisObject, string _customMessage)
        {
            _thisObject.Logging = new Prototype.Providers.Logging(_thisObject,
                  System.Diagnostics.EventLogEntryType.SuccessAudit,
                  0,
                  _customMessage);
            _thisObject.RaiseLogging();
        }
        public static void MessageWarning(this object _this, dynamic _thisObject, string _customMessage)
        {
            _thisObject.Logging = new Prototype.Providers.Logging(_thisObject,
                  System.Diagnostics.EventLogEntryType.Warning,
                  0,
                  _customMessage);
            _thisObject.RaiseLogging();
        }

        public static string LoggingText(this string _string, object _object)
        {
            return string.Format(_string, ((string)(_object.GetType().Name)).AddSpace().AddGuot(), ((string)_object.ToString()).AddGuot());
        }
    }
}
