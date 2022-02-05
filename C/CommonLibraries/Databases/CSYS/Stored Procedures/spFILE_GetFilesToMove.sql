
CREATE PROCEDURE [dbo].[spFILE_GetFilesToMove]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT	DISTINCT
		A.FileNameDescription
		,A.FilePathOriginal
		,A.FilePathArchiveTo
		,A.FilePathCopyTo
FROM	dbo.FILE_DAT_FilesToMove A
		INNER JOIN dbo.FILE_DAT_WhenToCheck B ON
			A.FileNameDescription = B.FileNameDescription
WHERE	B.TimeOfDayToCheck < CONVERT (TIME, CURRENT_TIMESTAMP)
		AND (
			 B.TimeOfDayToCheck > CONVERT (TIME, A.LastProcessed)
			 OR A.LastProcessed IS NULL
			 OR CONVERT (TIME, A.LastProcessed) > CONVERT (TIME, CURRENT_TIMESTAMP)
			 OR CONVERT (DATE, A.LastProcessed) < CONVERT (DATE, CURRENT_TIMESTAMP)
			)

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFILE_GetFilesToMove] TO [db_executor]
    AS [dbo];

