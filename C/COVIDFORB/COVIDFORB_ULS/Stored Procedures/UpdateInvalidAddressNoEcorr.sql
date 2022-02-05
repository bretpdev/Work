CREATE PROCEDURE [covidforb].[UpdateInvalidAddressNoEcorr]
	@ForbearanceProcessingId int
AS
	BEGIN TRANSACTION
BEGIN TRY
	
	UPDATE 
		[covidforb].ForbearanceProcessing 
	SET 
		InvalidAddressNotOnEcorr = 1
	WHERE 
		ForbearanceProcessingId = @ForbearanceProcessingId

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	THROW
END CATCH
RETURN 0
