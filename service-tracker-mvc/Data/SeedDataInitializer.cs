using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Transactions;

namespace service_tracker_mvc.Data
{
    public class SeedDataInitializer<T> : IDatabaseInitializer<T> where T : DbContext
    {
        private enum Status
        {
            Compatable,
            Invalid,
            Missing
        }

        public void InitializeDatabase(T context)
        {
            // Get metadata hash
            string hash = EdmMetadata.TryGetModelHash(context);

            bool exists;
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = context.Database.Exists();
            }

            if (exists)
            {

                ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

                var dbHash = GetHashFromDatabase(objectContext);

                Status compatability =
                    string.IsNullOrEmpty(dbHash) ?
                        Status.Missing :
                        (dbHash != hash) ?
                            Status.Invalid :
                            Status.Compatable;

                if (compatability == Status.Missing)
                {
                    ClearDatabase(objectContext);
                    CreateTables(objectContext);
                    SaveHash(objectContext, hash);
                }
                else if (compatability == Status.Invalid)
                {
                    throw new Exception(
                        "EdmMetadata does not match, manually update the database, expected: " +
                        Environment.NewLine +
                        "<[(" + hash + ")}>"
                    );
                }
            }
            else
            {
                context.Database.Create();
                context.SaveChanges();
            }

        }


        private void ClearDatabase(ObjectContext objectContext)
        {
            objectContext.ExecuteStoreCommand(DropAllObjects);
        }

        private void CreateTables(ObjectContext objectContext)
        {
            string dataBaseCreateScript = objectContext.CreateDatabaseScript();
            objectContext.ExecuteStoreCommand(dataBaseCreateScript);
        }

        private void SaveHash(ObjectContext objectContext, string hash)
        {
            objectContext.ExecuteStoreCommand(string.Format(UpdateEdmMetaDataTable, hash.Replace("'", "''")));
        }

        private string GetHashFromDatabase(ObjectContext objectContext)
        {
            foreach (var item in objectContext.ExecuteStoreQuery<string>(GetEdmMetaDataTable))
            {
                return item;
            }

            return string.Empty;
        }

        private const string UpdateEdmMetaDataTable = @"
            Delete From EdmMetadata;
            Insert Into EdmMetadata (ModelHash) Values ('{0}');";

        private const string GetEdmMetaDataTable = @"
            If Exists (Select * From INFORMATION_SCHEMA.TABLES tables where tables.TABLE_NAME = 'EdmMetaData')
                Select Top 1 ModelHash From EdmMetadata;
            Else
                Select '';";

        private const string DropAllObjects = @"
            declare @n char(1)
            set @n = char(10)

            declare @stmt nvarchar(max)

            -- procedures
            select @stmt = isnull( @stmt + @n, '' ) +
                'drop procedure [' + name + ']'
            from sys.procedures

            -- check constraints
            select @stmt = isnull( @stmt + @n, '' ) +
                'alter table [' + object_name( parent_object_id ) + '] drop constraint [' + name + ']'
            from sys.check_constraints

            -- functions
            select @stmt = isnull( @stmt + @n, '' ) +
                'drop function [' + name + ']'
            from sys.objects
            where type in ( 'FN', 'IF', 'TF' )

            -- views
            select @stmt = isnull( @stmt + @n, '' ) +
                'drop view [' + name + ']'
            from sys.views

            -- foreign keys
            select @stmt = isnull( @stmt + @n, '' ) +
                'alter table [' + object_name( parent_object_id ) + '] drop constraint [' + name + ']'
            from sys.foreign_keys

            -- tables
            select @stmt = isnull( @stmt + @n, '' ) +
                'drop table [' + name + ']'
            from sys.tables

            -- user defined types
            select @stmt = isnull( @stmt + @n, '' ) +
                'drop type [' + name + ']'
            from sys.types
            where is_user_defined = 1

            exec sp_executesql @stmt";
    }
}
