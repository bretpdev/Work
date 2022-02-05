CREATE PROCEDURE [dbo].[UpdateDatawarehouseRun]
	@LocalLoadDataID int 
AS
	UPDATE
		LocalLoadData
	SET
		LastSuccessfulRun = GETDATE()
	WHERE
		LocalLoadDataID = @LocalLoadDataID
RETURN 0
