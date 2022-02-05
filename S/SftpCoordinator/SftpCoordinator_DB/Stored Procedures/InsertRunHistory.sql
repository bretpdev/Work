CREATE PROCEDURE [dbo].[InsertRunHistory]
    @RunBy NVARCHAR(50)	
AS

INSERT INTO [dbo].[RunHistory] (RunBy)
VALUES(@RunBy)

DECLARE @id INT = SCOPE_IDENTITY()
SELECT 
	RunHistoryId, 
	StartedOn 
FROM
	RunHistory
WHERE 
	RunHistoryId = @id
