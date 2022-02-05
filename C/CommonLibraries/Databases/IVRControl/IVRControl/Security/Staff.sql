CREATE ROLE [Staff]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'Staff', @membername = N'UHEAA\!UHEAASSRS';


GO
EXECUTE sp_addrolemember @rolename = N'Staff', @membername = N'UHEAAKenticoDev';


GO
EXECUTE sp_addrolemember @rolename = N'Staff', @membername = N'testLogin';


GO
EXECUTE sp_addrolemember @rolename = N'Staff', @membername = N'UHEAA\SQL - OPSDEV - All Databases RW';


GO
EXECUTE sp_addrolemember @rolename = N'Staff', @membername = N'UHEAA\BatchScripts';

