CREATE ROLE [db_executor]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'db_executor', @membername = N'UHEAA\Imaging Users';


GO
EXECUTE sp_addrolemember @rolename = N'db_executor', @membername = N'UHEAA\SystemAnalysts';


GO
EXECUTE sp_addrolemember @rolename = N'db_executor', @membername = N'UHEAA\UHEAAUsers';


