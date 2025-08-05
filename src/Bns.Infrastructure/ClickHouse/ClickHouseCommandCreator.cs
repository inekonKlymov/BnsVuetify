using Bns.Infrastructure.ClickHouse.Columns;
using Bns.Infrastructure.ClickHouse.Databases;
using Bns.Infrastructure.ClickHouse.Dictionaries;
using Bns.Infrastructure.ClickHouse.Functions;
using Bns.Infrastructure.ClickHouse.NamedCollections;
using Bns.Infrastructure.ClickHouse.Partitions;
using Bns.Infrastructure.ClickHouse.Quotas;
using Bns.Infrastructure.ClickHouse.Roles;
using Bns.Infrastructure.ClickHouse.RowPolicies;
using Bns.Infrastructure.ClickHouse.Rows;
using Bns.Infrastructure.ClickHouse.Settings;
using Bns.Infrastructure.ClickHouse.SettingsProfiles;
using Bns.Infrastructure.ClickHouse.Systems;
using Bns.Infrastructure.ClickHouse.Table;
using Bns.Infrastructure.ClickHouse.Table.Alter;
using Bns.Infrastructure.ClickHouse.Table.Alter.Constraint;
using Bns.Infrastructure.ClickHouse.Table.Alter.Index;
using Bns.Infrastructure.ClickHouse.Table.Alter.Statistics;
using Bns.Infrastructure.ClickHouse.Users;
using Bns.Infrastructure.ClickHouse.Views;

namespace Bns.Infrastructure.ClickHouse;

public static class ClickHouseCommandCreator
{
    public static class Columns
    {
        public static ClickHouseAlterColumnCommandBuilder Alter() => new();
        public static ClickHouseShowColumnsCommandBuilder Show() => new();
    }
    public static class Databases
    {
        public static ClickHouseCreateDatabaseCommandBuilder Create() => new();
        public static ClickHouseDropDatabaseCommandBuilder Drop() => new();
        public static ClickHouseShowDatabasesCommandBuilder Show() => new();
    }
    public static class Dictionaries
    {
        public static ClickHouseCreateDictionaryCommandBuilder Create() => new();
        //public static ClickHouseAlterDictionaryCommandBuilder Alter() => new();
        public static ClickHouseDropDictionaryCommandBuilder Drop() => new();
        public static ClickHouseShowDictionariesCommandBuilder Show() => new();
    }
    public static class Functions
    {
        public static ClickHouseCreateFunctionCommandBuilder Create() => new();
        //public static ClickHouseAlterFunctionCommandBuilder Alter() => new();
        public static ClickHouseDropFunctionCommandBuilder Drop() => new();
        public static ClickHouseShowFunctionsCommandBuilder Show() => new();
    }
    public static class NamedCollections
    {
        public static ClickHouseCreateNamedCollectionCommandBuilder Create() => new();
        public static ClickHouseAlterNamedCollectionCommandBuilder Alter() => new();
        public static ClickHouseDropNamedCollectionCommandBuilder Drop() => new();

    }
    public static class Partitions
    {
        public static ClickHouseAlterPartitionCommandBuilder Alter() => new();
    }

    public static class Quotas
    {
        public static ClickHouseAlterQuotaCommandBuilder Alter() => new();
        public static ClickHouseCreateQuotaCommandBuilder Create() => new();
        public static ClickHouseDropQuotaCommandBuilder Drop() => new();
        public static ClickHouseShowCreateQuotaCommandBuilder ShowCreate() => new();
        public static ClickHouseShowQuotaCommandBuilder ShowQuota() => new();
        public static ClickHouseShowQuotasCommandBuilder ShowQuotas() => new();
        // ...
    }
    public static class Roles
    {
        public static ClickHouseAlterRoleCommandBuilder Alter() => new();
        public static ClickHouseCreateRoleCommandBuilder Create() => new();
        public static ClickHouseDropRoleCommandBuilder Drop() => new();
        public static ClickHouseShowCreateRoleCommandBuilder ShowCreate() => new();
        public static ClickHouseShowRolesCommandBuilder Show() => new();
        // ...
    }
    public static class RowPolicies
    {
        public static ClickHouseAlterRowPolicyCommandBuilder Alter() => new();
        public static ClickHouseCreateRowPolicyCommandBuilder Create() => new();
        public static ClickHouseDropRowPolicyCommandBuilder Drop() => new();
        public static ClickHouseShowCreateRowPolicyCommandBuilder ShowCreate() => new();
        public static ClickHouseShowRowPoliciesCommandBuilder ShowRowPolicies() => new();

    }
    public static class Rows
    {
        public static ClickHouseInsertRowCommandBuilder Insert() => new();
        public static ClickHouseSelectRowCommandBuilder Select() => new();

        // ...
    }
    public static class SettingsProfiles
    {
        public static ClickHouseAlterSettingsProfileCommandBuilder Alter() => new();
        public static ClickHouseCreateSettingsProfileCommandBuilder Create() => new();
        public static ClickHouseDropSettingsProfileCommandBuilder Drop() => new();
        public static ClickHouseShowCreateSettingsProfileCommandBuilder ShowCreate() => new();
        public static ClickHouseShowSettingsProfilesCommandBuilder Show() => new();
    }

    public static class Systems
    {
        public static ClickHouseShowClusterCommandBuilder ShowCluster() => new();
        public static ClickHouseShowClustersCommandBuilder ShowClusters() => new();
        public static ClickHouseShowEnginesCommandBuilder ShowEngines() => new();
        public static ClickHouseShowFilesystemCachesCommandBuilder ShowFilesystem() => new();
        public static ClickHouseShowMergesCommandBuilder ShowMerges() => new();
        public static ClickHouseShowProcesslistCommandBuilder ShowProcessList() => new();
        public static ClickHouseShowSettingCommandBuilder ShowSetting() => new();
        public static ClickHouseShowSettingsCommandBuilder ShowSettings() => new();

    }
    public static class Table
    {
        public static ClickHouseCreateTableCommandBuilder Create() => new();
        public static ClickHouseShowTablesCommandBuilder Show() => new();
        public static ClickHouseShowTableIndexesCommandBuilder ShowIndexes()=> new();
        public static ClickHouseDropTableCommandBuilder Drop() => new();
        public static class Alter
        {
            public static ClickHouseAlterTableApplyDeletedMaskCommandBuilder ApplyDeletedMask() => new();
            public static ClickHouseAlterTableDeleteWhereCommandBuilder DeleteWhere() => new();
            public static ClickHouseAlterTableModifyOrderByCommandBuilder ModifyOrderBuilder() => new();
            public static ClickHouseAlterTableSampleByCommandBuilder SampleBy() => new();
            public static ClickHouseAlterTableSettingsCommandBuilder Settings() => new();
            public static ClickHouseAlterTableTtlCommandBuilder Ttl() => new();
            public static ClickHouseAlterTableUpdateCommandBuilder Update() => new();
            public static class Constraints
            {
                public static ClickHouseAlterTableAddConstraintCommandBuilder AddConstraint() => new();
                public static ClickHouseAlterTableDropConstraintCommandBuilder DropConstraint() => new();
            }
            public static class Index
            {

                public static ClickHouseAlterTableAddIndexCommandBuilder Add() => new();
                public static ClickHouseAlterTableClearIndexCommandBuilder Clear() => new();
                public static ClickHouseAlterTableDropIndexCommandBuilder Drop() => new();
                public static ClickHouseAlterTableMaterializeIndexCommandBuilder Materialize() => new();
            }
            public static class Statistics
            {

                public static ClickHouseAlterTableAddStatisticsCommandBuilder Add() => new();
                public static ClickHouseAlterTableClearStatisticsCommandBuilder Clear() => new();
                public static ClickHouseAlterTableDropStatisticsCommandBuilder Drop() => new();
                public static ClickHouseAlterTableMaterializeStatisticsCommandBuilder Materialize() => new();
                public static ClickHouseAlterTableModifyStatisticsCommandBuilder Modify() => new();
            }
        }

    }
    public static class Users
    {
        public static ClickHouseAlterUserCommandBuilder Alter() => new();
        public static ClickHouseCreateUserCommandBuilder Create() => new();
        public static ClickHouseDropUserCommandBuilder Drop() => new();

        public static ClickHouseShowAccessCommandBuilder ShowAccess() => new();
        public static ClickHouseShowCreateUserCommandBuilder ShowCreateUser() => new();
        public static ClickHouseShowGrantsCommandBuilder ShowGrants() => new();
        public static ClickHouseShowUsersCommandBuilder Show() => new();
    }
    public static class Views
    {
        public static ClickHouseCreateViewCommandBuilder Create() => new();
        //public static ClickHouseAlterViewCommandBuilder Alter() => new();
        public static ClickHouseDropViewCommandBuilder Drop() => new();

        // ...
    }
    // Добавьте аналогичные вложенные классы для других сущностей (User, Role, Database, NamedCollection и т.д.)
}
