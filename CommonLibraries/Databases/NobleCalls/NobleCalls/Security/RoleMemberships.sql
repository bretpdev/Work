EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\!UHEAASSRS';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAAKenticoDev';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\SQL - OPSDEV - All Databases RW';






GO
ALTER ROLE [db_securityadmin] ADD MEMBER [UHEAA\ROLE - Application Development - Manager];


GO
ALTER ROLE [db_securityadmin] ADD MEMBER [UHEAA\SQL - OPSDEV - Sysadmin];


GO
ALTER ROLE [db_owner] ADD MEMBER [UHEAA\ROLE - Application Development - Manager];


GO
ALTER ROLE [db_owner] ADD MEMBER [UHEAA\SQL - OPSDEV - Sysadmin];


GO
ALTER ROLE [db_owner] ADD MEMBER [UHEAA\BatchScripts];


GO
ALTER ROLE [db_owner] ADD MEMBER [UHEAA\CornerStoneUsers];


GO
ALTER ROLE [db_owner] ADD MEMBER [UHEAA\Developers];

