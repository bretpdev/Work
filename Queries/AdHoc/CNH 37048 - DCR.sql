USE ServicerInventoryMetrics
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XXX

DELETE FROM MetricsSummary WHERE ServicerMetricsId = XX --XX rows

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR


--XX rows
INSERT INTO MetricsSummary(ServicerMetricsId,CompliantRecords,TotalRecords,MetricMonth,MetricYear,AverageBacklogAge,UpdatedAt,UpdatedBy)
VALUES
(X,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --AccountResearch
(X,XXXX,XXXX,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --BorrowerEmail
(X,XXXX,XXXX,X,XXXX,X,GETDATE(),'CNH XXXXX'), --BorrowerEmail
(X,XXXX,XXXX,X,XXXX,X,GETDATE(),'CNH XXXXX'), --BorrowerEmail
(X,XXXX,XXXX,X,XXXX,X,GETDATE(),'CNH XXXXX'), --BorrowerEmail
(X,XXXX,XXXX,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --BorrowerEmail
(XX,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --ClosedSchoolNotif
(XX,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --ClosedSchoolNotif
(XX,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --ClosedSchoolNotif
(XX,X,X,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --ClosedSchoolNotif
(XX,X,X,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --ClosedSchoolNotif
(XX,X,X,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --ClosedSchoolNotif
(XX,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --ClosedSchoolNotif
(XX,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --ClosedSchoolNotif
(X,X,X,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --ControlMail
(X,X,X,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --ControlMail
(X,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --ControlMail
(X,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --ControlMail
(X,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --ControlMail
(XX,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --DisabilityDischarge
(XX,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --DisabilityDischarge
(XX,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --DisabilityDischarge
(XX,X,X,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --DisabilityDischarge
(XX,X,X,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --DisabilityDischarge
(XX,X,X,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --DisabilityDischarge
(XX,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --DisabilityDischarge
(XX,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --DisabilityDischarge
(XX,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --DisabilityDischarge
(X,X,X,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --FOIAPrivacy
(X,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --FOIAPrivacy
(XX,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --FraudServicerAging
(XX,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --FraudServicerAging
(XX,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --FraudServicerAging
(XX,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --FraudServicerAging
(XX,X,X,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --FraudServicerAging
(XX,X,X,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --FraudServicerAging
(X,XXXX,XXXX,X,XXXX,X,GETDATE(),'CNH XXXXX'), --ImagingIndexing
(X,XXX,XXX,X,XXXX,X,GETDATE(),'CNH XXXXX'), --MailRoomDocs
(X,XXXX,XXXX,X,XXXX,X,GETDATE(),'CNH XXXXX'), --MailRoomDocs
(X,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --Ombudsman
(X,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --Ombudsman
(X,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --Ombudsman
(X,XX,XX,X,XXXX,X,GETDATE(),'CNH XXXXX'), --Ombudsman
(X,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --Ombudsman
(X,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --Ombudsman
(X,X,X,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --Ombudsman
(X,XX,XX,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --Ombudsman
(X,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --OtherEscalatedMail
(X,X,X,XX,XXXX,X,GETDATE(),'CNH XXXXX'), --OtherEscalatedMail
(X,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --OtherEscalatedMail
(X,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --OtherEscalatedMail
(X,X,X,X,XXXX,X,GETDATE(),'CNH XXXXX'), --OtherEscalatedMail
(XX,-XXXXX,-XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,-XXXXX,-XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXX,XXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,XX,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXX,XXXX,XX,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,XX,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXX,XXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXX,XXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXXX,XXXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXX,XXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,XX,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,XX,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,XX,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXX,XXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXXX,XXXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXX,XXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXX,XXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXX,XXXX,XX,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,XX,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,XX,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXXX,XXXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,XX,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXXX,XXXXXX,XX,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXX,XXXXX,XX,XXXX,X,GETDATE(), 'CNH XXXXX'), --PaymentSuspense
(XX,XXXXXX,XXXXXX,X,XXXX,X,GETDATE(), 'CNH XXXXX') --PaymentSuspense

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END