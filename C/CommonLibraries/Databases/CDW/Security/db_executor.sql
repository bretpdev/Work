CREATE ROLE [db_executor]
    AUTHORIZATION [dbo];






GO
EXECUTE sp_addrolemember @rolename = N'db_executor', @membername = N'UHEAA\BatchScripts';


GO
EXECUTE sp_addrolemember @rolename = N'db_executor', @membername = N'UHEAA\SystemAnalysts';


GO
EXECUTE sp_addrolemember @rolename = N'db_executor', @membername = N'UHEAA\CornerStoneUsers';





GO
ALTER ROLE [db_executor] ADD MEMBER [NSCSQL];

