EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'UHEAA\bpehrson';

GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'UHEAA\elynes';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'UHEAA\Developers';



GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\SystemAnalysts';

GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\!UHEAASSRS';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\SQL - OPSDEV - All Databases RW';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'UHEAA\SystemAnalysts';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'UHEAA\SQL - OPSDEV - All Databases RW';

