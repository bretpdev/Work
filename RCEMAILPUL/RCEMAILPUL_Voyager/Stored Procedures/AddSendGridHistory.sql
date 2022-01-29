CREATE PROCEDURE [rcemailpul].[AddSendGridHistory]
	@FromEmail VARCHAR(320),
    @MsgId VARCHAR(50), 
    @Subject VARCHAR(200), 
    @ToEmail VARCHAR(320), 
    @Status VARCHAR(50), 
    @OpensCount INT, 
    @ClicksCount INT, 
    @LastEventTime DATETIME
AS
	
	IF NOT EXISTS(SELECT * FROM rcemailpul.SendGridHistory WHERE MsgId = @MsgId)
	BEGIN
		INSERT INTO rcemailpul.SendGridHistory (FromEmail, MsgId, [Subject], ToEmail, [Status], OpensCount, ClicksCount, LastEventTime)
		VALUES (@FromEmail, @MsgId, @Subject, @ToEmail, @Status, @OpensCount, @ClicksCount, @LastEventTime)
	END
	ELSE
	BEGIN
		
		--send old data back before updating
		SELECT
			SendGridHistoryId,
			FromEmail, 
			MsgId, 
			[Subject], 
			ToEmail, 
			[Status], 
			OpensCount, 
			ClicksCount, 
			LastEventTime
		FROM
			rcemailpul.SendGridHistory
		WHERE
			MsgId = @MsgId
		

		UPDATE
			rcemailpul.SendGridHistory
		SET
			FromEmail = @FromEmail, 
			MsgId = @MsgId, 
			[Subject] = @Subject, 
			ToEmail = @ToEmail, 
			[Status] = @Status, 
			OpensCount = @OpensCount, 
			ClicksCount = @ClicksCount, 
			LastEventTime = @LastEventTime
		WHERE
			MsgId = @MsgId
	END

RETURN 0
