CREATE PROCEDURE [scheduler].[MoveLetterToCourt]
	@RequestId int,
	@CourtFullName varchar(100)
AS
	
	DECLARE @ExistingStatus VARCHAR(50)
	DECLARE @History VARCHAR(MAX)
	DECLARE @NewStatus VARCHAR(50)
	SELECT 
		@ExistingStatus = CurrentStatus,
		@History = History
	FROM
		[LTDB_Dat_Requests]
	WHERE
		[Request] = @RequestId

	IF @ExistingStatus = 'Programmer Assignment' 
		SET @NewStatus = 'Programmer Queue'
	ELSE IF @ExistingStatus = 'Tester Assignment'
		SET @NewStatus = 'Tester Queue'
	ELSE
		RAISERROR('This stored procedure only supports moving tickets from Programmer/Tester Assignment to Programmer/Tester Queue', 16, 11)

	DECLARE @FormattedTimestamp VARCHAR(50) = CONVERT(VARCHAR(10),GETDATE(), 101) + ' ' + STUFF(RIGHT(CONVERT(VARCHAR(32),GETDATE(),100),8), 7, 0, ' ')
	DECLARE @Newline CHAR(2) = CHAR(13) + CHAR(10)
	SET @History = @CourtFullName + ' - ' + @FormattedTimestamp + ' - ' + @NewStatus + @Newline
	             + 'Self-assigned using Scheduler.' + @Newline
				 + @Newline
				 + @History


	UPDATE
		[LTDB_Dat_Requests]
	SET
		[Court] = @CourtFullName,
		[CurrentStatus] = @NewStatus,
		[PreviousStatus] = [CurrentStatus],
		[StatusDate] = GETDATE(),
		[History] = @History
	WHERE
		[Request] = @RequestId

RETURN 0
