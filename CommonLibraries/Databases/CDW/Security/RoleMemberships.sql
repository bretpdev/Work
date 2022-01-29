EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'UHEAA\Developers';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'UHEAA\SystemAnalysts';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'UHEAA\CornerStoneUsers';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\BatchScripts';





GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\SystemAnalysts';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\CornerStoneUsers';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\!UHEAASSRS';




GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\kferre';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'UHEAA\BatchScripts';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'UHEAA\SystemAnalysts';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'UHEAA\CornerStoneUsers';





GO
ALTER ROLE [db_owner] ADD MEMBER [UHEAA\SQL - OPSDEV - Developers DBOwner Access];


GO
ALTER ROLE [db_datawriter] ADD MEMBER [UHEAA\SQL - OPSDEV - All Databases RW];


GO
ALTER ROLE [db_datawriter] ADD MEMBER [NSCSQL];


GO
ALTER ROLE [db_datareader] ADD MEMBER [UHEAA\SQL - OPSDEV - All Databases RO];


GO
ALTER ROLE [db_datareader] ADD MEMBER [UHEAA\SQL - OPSDEV - All Databases RW];


GO
ALTER ROLE [db_datareader] ADD MEMBER [DevKofaxFedSQL];


GO
ALTER ROLE [db_datareader] ADD MEMBER [!UHEAASSRS];


GO
ALTER ROLE [db_datareader] ADD MEMBER [NSCSQL];

