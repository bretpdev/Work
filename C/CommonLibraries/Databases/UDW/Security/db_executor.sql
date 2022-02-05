CREATE ROLE [db_executor]
    AUTHORIZATION [dbo];


GO
ALTER ROLE [db_executor] ADD MEMBER [UHEAA\BatchScripts];


GO
ALTER ROLE [db_executor] ADD MEMBER [!UHEAASSRS];


GO
ALTER ROLE [db_executor] ADD MEMBER [UHEAA\UHEAAUsers];


GO
ALTER ROLE [db_executor] ADD MEMBER [UHEAA\CornerStoneUsers];


GO
ALTER ROLE [db_executor] ADD MEMBER [UHEAA\SystemAnalysts];


GO
ALTER ROLE [db_executor] ADD MEMBER [NSCSQL];


GO
ALTER ROLE [db_executor] ADD MEMBER [UHEAA\Imaging Users];

