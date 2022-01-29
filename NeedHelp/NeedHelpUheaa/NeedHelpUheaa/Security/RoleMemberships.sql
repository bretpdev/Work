ALTER ROLE [db_owner] ADD MEMBER [UHEAA\SQL - OPSDEV - Developers DBOwner Access];


GO
ALTER ROLE [db_owner] ADD MEMBER [UHEAA\Developers];


GO
ALTER ROLE [db_datawriter] ADD MEMBER [UHEAA\SQL - OPSDEV - All Databases RW];


GO
ALTER ROLE [db_datawriter] ADD MEMBER [UHEAA\SystemAnalysts];


GO
ALTER ROLE [db_datareader] ADD MEMBER [UHEAA\BatchScripts];


GO
ALTER ROLE [db_datareader] ADD MEMBER [UHEAA\SQL - OPSDEV - All Databases RO];


GO
ALTER ROLE [db_datareader] ADD MEMBER [UHEAA\SQL - OPSDEV - All Databases RW];


GO
ALTER ROLE [db_datareader] ADD MEMBER [UHEAA\!UHEAASSRS];


GO
ALTER ROLE [db_datareader] ADD MEMBER [UHEAA\SystemAnalysts];

