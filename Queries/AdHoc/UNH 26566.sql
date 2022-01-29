USE [NobleCalls]
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

UPDATE
	NobleCalls.dbo.CallCampaigns 
SET 
	RegionId = 2 
WHERE 
	CallCampaign = 'VABU'

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

UPDATE 
	NobleCalls.dbo.NobleCallHistory 
SET 
	Deleted = 0, 
	DeletedBy = NULL, 
	DeletedAt = NULL, 
	RegionId = 2, 
	ArcAddProcessingId = NULL 
where CallCampaign = 'VABU' AND CallType != 5 AND PhoneNumber NOT IN ('8012593828','8015181733','8013217288','8013668485','8017596121','8013668418')

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = 41 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
