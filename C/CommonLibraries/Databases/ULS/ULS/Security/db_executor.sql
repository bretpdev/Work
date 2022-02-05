CREATE ROLE [db_executor]
    AUTHORIZATION [db_owner];






GO



GO



GO
EXECUTE sp_addrolemember @rolename = N'db_executor', @membername = N'UHEAA\SystemAnalysts';


GO
EXECUTE sp_addrolemember @rolename = N'db_executor', @membername = N'UHEAA\Developers';


GO
EXECUTE sp_addrolemember @rolename = N'db_executor', @membername = N'UHEAA\UHEAAUsers';


GO
EXECUTE sp_addrolemember @rolename = N'db_executor', @membername = N'UHEAA\BatchScripts';

