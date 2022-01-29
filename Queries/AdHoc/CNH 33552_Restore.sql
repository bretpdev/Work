USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	-- Restoring CLS.complaints.ComplaintHistory for ComplaintId XXX, XXX
	SET IDENTITY_INSERT CLS.complaints.ComplaintHistory ON

	INSERT INTO CLS.complaints.ComplaintHistory
		SELECT
			*
		FROM
			CLS_X.complaints.ComplaintHistory
		WHERE 
			ComplaintId IN (XXX,XXX)

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	SET IDENTITY_INSERT CLS.complaints.ComplaintHistory OFF

	
	-- Restoring CLS.complaints.ComplaintFlags for ComplaintId XXX, XXX
	SET IDENTITY_INSERT CLS.complaints.ComplaintFlags ON

	INSERT INTO CLS.complaints.ComplaintFlags
		SELECT
			*
		FROM
			CLS_X.complaints.ComplaintFlags
		WHERE 
			ComplaintId IN (XXX,XXX)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	SET IDENTITY_INSERT CLS.complaints.ComplaintFlags OFF

	
	-- Restoring CLS.complaints.ComplaintComplaints for ComplaintId XXX, XXX
	-- Set the ResolutionComplaintHistoryId to NULL. There is a circular reference with the ComplaintsHistory table
	SET IDENTITY_INSERT CLS.complaints.Complaints ON

	INSERT INTO CLS.complaints.Complaints(ComplaintId, AccountNumber, BorrowerName, ComplaintTypeId, ComplaintPartyId, ComplaintGroupId, ComplaintDate, ControlMailNumber, DaysToRespond, NeedHelpTicketNumber, ResolutionComplaintHistoryId, ComplaintDescription, AddedOn, AddedBy)
		SELECT
			ComplaintId, AccountNumber, BorrowerName, ComplaintTypeId, ComplaintPartyId, ComplaintGroupId, ComplaintDate, ControlMailNumber, DaysToRespond, NeedHelpTicketNumber, NULL, ComplaintDescription, AddedOn, AddedBy
		FROM
			CLS_X.complaints.Complaints
		WHERE 
			ComplaintId IN (XXX,XXX)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	SET IDENTITY_INSERT CLS.complaints.Complaints OFF


	-- Updating the ResolutionComplaintHistoryId for ComplaintId XXX
	UPDATE
		CLS.complaints.Complaints
	SET
		ResolutionComplaintHistoryId = (SELECT ResolutionComplaintHistoryId FROM CLS_X.complaints.Complaints WHERE ComplaintId = XXX)
	WHERE
		ComplaintId = XXX

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	
	-- Updating the ResolutionComplaintHistoryId for ComplaintId XXX
	UPDATE
		CLS.complaints.Complaints
	SET
		ResolutionComplaintHistoryId = (SELECT ResolutionComplaintHistoryId FROM CLS_X.complaints.Complaints WHERE ComplaintId = XXX)
	WHERE
		ComplaintId = XXX

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	-- Updating the ResolutionComplaintHistoryId to NULL for record XXX so it can be deleted
	UPDATE
		CLS.complaints.Complaints
	SET
		ResolutionComplaintHistoryId = NULL
	WHERE
		ComplaintId IN (XXX, XXX)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	
	-- Deleting the ComplaintFlags records for ComplaintId XXX, XXX
	DELETE FROM CLS.complaints.ComplaintFlags
	WHERE ComplaintId IN (XXX, XXX)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	-- Deleting the ComplaintHistory records for ComplaintId XXX, XXX
	DELETE FROM CLS.complaints.ComplaintHistory
	WHERE ComplaintId IN (XXX, XXX)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	
	-- Deleting the Complaint records for ComplaintId XXX, XXX
	DELETE FROM CLS.complaints.Complaints
	WHERE ComplaintId IN (XXX, XXX)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


IF @ROWCOUNT = XX AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END