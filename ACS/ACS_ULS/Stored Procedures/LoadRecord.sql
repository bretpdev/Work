CREATE PROCEDURE [acs].[LoadRecord]
	@PType CHAR(1),
	@SSN VARCHAR(9),
	@AddrDt VARCHAR(4),
	@FirstFour CHAR(4) = NULL,
	@BorName VARCHAR(48),
	@Addr1 VARCHAR(48),
	@City VARCHAR(32),
	@State VARCHAR(2),
	@Zip VARCHAR(9),
	@FileId VARCHAR(16),
	@NewAddressFull VARCHAR(150),
	@OldAddressFull VARCHAR(150)
AS
	
BEGIN
	BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

		IF EXISTS (SELECT DF_PRS_ID FROM ODW..PD01_PDM_INF WHERE DF_PRS_ID = @SSN)
			BEGIN
				IF NOT EXISTS 
				(
					SELECT 
						SSN 
					FROM 
						OLS.acs.OneLinkDemographics 
					WHERE 
						SSN = @SSN 
						AND PersonType = @PType 
						AND NewAddressFull = @NewAddressFull
						AND OldAddressFull = @OldAddressFull
						AND FileId = @FileId 
						AND DeletedAt IS NULL 
						AND 
						(
							ProcessedAt IS NULL
							OR ArcAddProcessingId IS NULL
						)
				)
					BEGIN
						INSERT INTO [OLS].[acs].[OneLinkDemographics] (PersonType, SSN, AddrDate, FirstFourName, FullName, Address1, City, [State], Zip, FileId, NewAddressFull, OldAddressFull, AddedBy, AddedAt) 
						VALUES(@PType,@SSN, @AddrDt, @FirstFour, @BorName, @Addr1, @City, @State, @Zip, @FileId, @NewAddressFull, @OldAddressFull, CURRENT_USER, GETDATE())
						SELECT @ERROR = @@ERROR, @ROWCOUNT = @@ROWCOUNT
					END
			END
		ELSE IF EXISTS (SELECT DF_PRS_ID FROM UDW..PD10_PRS_NME WHERE DF_PRS_ID = @SSN)
			BEGIN
				IF NOT EXISTS (
								SELECT 
									SSN
								FROM 
									ULS.acs.UheaaDemographics 
								WHERE 
									SSN = @SSN 
									AND PersonType = @PType 
									AND NewAddressFull = @NewAddressFull
									AND OldAddressFull = @OldAddressFull
									AND FileId = @FileId 
									AND DeletedAt IS NULL 
									AND 
									(
										ProcessedAt IS NULL
										OR ArcAddProcessingId IS NULL
									)
							 )
					BEGIN
						INSERT INTO [ULS].[acs].[UheaaDemographics] (PersonType, SSN, AddrDate, FullName, Address1, City, [State], Zip, FileId, NewAddressFull, OldAddressFull, AddedBy, AddedAt) 
						VALUES(@PType,@SSN, @AddrDt, @BorName, @Addr1, @City, @State, @Zip, @FileId, @NewAddressFull, @OldAddressFull, CURRENT_USER, GETDATE())
						SELECT @ERROR = @@ERROR, @ROWCOUNT = @@ROWCOUNT
					END
			END
	IF(@ERROR = 0 AND @ROWCOUNT = 1)
		BEGIN
			COMMIT TRANSACTION
			SELECT CAST(1 AS BIT) [WasSuccessful]
		END
	ELSE
		BEGIN
			ROLLBACK TRANSACTION
			SELECT CAST(0 AS BIT) [WasSuccessful]
		END
END