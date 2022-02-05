EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'UHEAA\Developers';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\!UHEAASSRS';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAAKenticoDev';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\SystemAnalysts';

