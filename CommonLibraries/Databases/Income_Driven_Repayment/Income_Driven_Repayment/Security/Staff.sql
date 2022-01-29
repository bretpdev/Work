CREATE ROLE [Staff]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'Staff', @membername = N'UHEAA\BatchScripts';


GO
EXECUTE sp_addrolemember @rolename = N'Staff', @membername = N'UHEAA\SQL - OPSDEV - All Databases RO';


GO
EXECUTE sp_addrolemember @rolename = N'Staff', @membername = N'UHEAA\SQL - OPSDEV - Developers DBOwner Access';


GO
EXECUTE sp_addrolemember @rolename = N'Staff', @membername = N'UHEAA\SQL - OPSDEV - All Databases RW';


GO
EXECUTE sp_addrolemember @rolename = N'Staff', @membername = N'testLogin';


GO
EXECUTE sp_addrolemember @rolename = N'Staff', @membername = N'UHEAA\!UHEAASSRS';


GO
EXECUTE sp_addrolemember @rolename = N'Staff', @membername = N'UHEAA\SystemAnalysts';


GO
EXECUTE sp_addrolemember @rolename = N'Staff', @membername = N'UHEAA\Developers';


GO
EXECUTE sp_addrolemember @rolename = N'Staff', @membername = N'UHEAA\CornerStoneUsers';

