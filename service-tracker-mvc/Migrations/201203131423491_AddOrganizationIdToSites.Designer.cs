// <auto-generated />
namespace service_tracker_mvc.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    
    public sealed partial class AddOrganizationIdToSites : IMigrationMetadata
    {
        string IMigrationMetadata.Id
        {
            get { return "201203131423491_AddOrganizationIdToSites"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return "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"; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return "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"; }
        }
    }
}
