CREATE PROCEDURE [projectrequest].[InsertScore]
(
	@ProjectId INT,
	@ScoreTypeId INT,
	@Score INT,
	@ScoreId INT NULL
)
AS

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @NEWSCOREID INT
	DECLARE @SCORETYPE VARCHAR(50)
	DECLARE @EXPECTEDROWCOUNT INT

SELECT @EXPECTEDROWCOUNT = (SELECT CASE WHEN @ScoreId IS NULL THEN 2 ELSE 3 END)
SELECT @SCORETYPE = ScoreType FROM [projectrequest].ScoreTypes WHERE ScoreTypeId = @ScoreTypeId

INSERT INTO [projectrequest].[Scores]
	(ProjectId, 
	ScoreTypeId,
	Score,
	Scorer,
	ScoreDate,
	Active,
	PreviousScoreId)
VALUES
	(@ProjectId,
	@ScoreTypeId,
	@Score,
	SUSER_NAME(),
	GETDATE(),
	1,
	@ScoreId)

SELECT @NEWSCOREID = @@IDENTITY, @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR =  @ERROR + @@ERROR

IF @SCORETYPE = 'Finance Score'
	UPDATE 
		[projectrequest].[Projects]
	SET
		[FinanceScoreId] = @NEWSCOREID
	WHERE
		[ProjectId] = @ProjectId
ELSE IF @SCORETYPE = 'Requestor Score'
	UPDATE 
		[projectrequest].[Projects]
	SET
		[RequestorScoreId] = @NEWSCOREID
	WHERE
		[ProjectId] = @ProjectId
ELSE IF @SCORETYPE = 'Urgency Score'
	UPDATE 
		[projectrequest].[Projects]
	SET
		[UrgencyScoreId] = @NEWSCOREID
	WHERE
		[ProjectId] = @ProjectId
ELSE IF @SCORETYPE = 'Resources Score'
	UPDATE 
		[projectrequest].[Projects]
	SET
		[ResourcesScoreId] = @NEWSCOREID
	WHERE
		[ProjectId] = @ProjectId
ELSE IF @SCORETYPE = 'Risk Score'
	UPDATE 
		[projectrequest].[Projects]
	SET
		[RiskScoreId] = @NEWSCOREID
	WHERE
		[ProjectId] = @ProjectId

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR =  @ERROR + @@ERROR

UPDATE [projectrequest].[Scores]
SET
	[Active] = 0
WHERE
	ScoreId = @ScoreId

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR =  @ERROR + @@ERROR
	
IF @ROWCOUNT = @EXPECTEDROWCOUNT AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END