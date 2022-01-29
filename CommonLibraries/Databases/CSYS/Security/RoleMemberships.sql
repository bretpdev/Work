EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'UHEAA\UHEAAUsers';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'UHEAA\Developers';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'UHEAA\SystemAnalysts';



GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'UHEAA\BatchScripts';





GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\UHEAAUsers';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\SystemAnalysts';










GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\!UHEAASSRS';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'UHEAA\UHEAAUsers';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'UHEAA\SystemAnalysts';



