﻿


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'UHEAA\SQL - OPSDEV - All Databases RW';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'UHEAA\SQL - OPSDEV - All Databases RW';

