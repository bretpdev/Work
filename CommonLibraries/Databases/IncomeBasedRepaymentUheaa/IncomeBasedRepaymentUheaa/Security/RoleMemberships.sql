EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'UHEAA\Developers';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\!UHEAASSRS';


GO



GO



GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\SystemAnalysts';


GO



GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'UHEAA\SystemAnalysts';

