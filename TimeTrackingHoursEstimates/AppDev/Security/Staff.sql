CREATE ROLE [Staff]
    AUTHORIZATION [dbo];


GO
ALTER ROLE [Staff] ADD MEMBER [UHEAA\SQL - OPSDEV - Sysadmin];


GO
ALTER ROLE [Staff] ADD MEMBER [ROLE - Application Development - Manager];

