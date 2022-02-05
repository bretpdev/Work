



EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'UHEAA\Developers';





GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\SystemAnalysts';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'SSRS';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'!UHEAASSRS';




GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\!UHEAASSRS';


GO



GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\SQL - OPSDEV - All Databases RW';



GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'UHEAA\SystemAnalysts';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'UHEAA\SQL - OPSDEV - All Databases RW';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'UHEAA\SQL - OPSDEV - Developers DBOwner Access';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'UHEAA\BatchScripts';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'jryan';

