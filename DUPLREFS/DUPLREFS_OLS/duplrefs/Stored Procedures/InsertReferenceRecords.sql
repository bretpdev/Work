CREATE PROCEDURE [duplrefs].[InsertReferenceRecords]
	@BorrowerQueueId INT,
	@ReferenceRecords ReferenceRecord READONLY

AS

	BEGIN 
		BEGIN TRANSACTION
		DECLARE @ERROR INT = 0

		INSERT INTO duplrefs.ReferenceQueue(BorrowerQueueId,RefId,RefName,RefAddress1,RefAddress2,RefCity,RefState,RefZip,RefCountry,RefPhone,RefStatus,ValidAddress,ValidPhone,DemosChanged,ZipChanged,Duplicate,PossibleDuplicate)
		SELECT
			@BorrowerQueueId,
			REF_NEW.RefId,
			REF_NEW.RefName,
			REF_NEW.RefAddress1,
			REF_NEW.RefAddress2,
			REF_NEW.RefCity,
			REF_NEW.RefState,
			REF_NEW.RefZip,
			REF_NEW.RefCountry,
			REF_NEW.RefPhone,
			REF_NEW.RefStatus,
			REF_NEW.ValidAddress,
			REF_NEW.ValidPhone,
			REF_NEW.DemosChanged,
			REF_NEW.ZipChanged,
			REF_NEW.Duplicate,
			REF_NEW.PossibleDuplicate
		FROM
			@ReferenceRecords REF_NEW
			LEFT JOIN duplrefs.ReferenceQueue REF_EXISTING
				ON REF_EXISTING.BorrowerQueueId = REF_NEW.BorrowerQueueId
				AND REF_EXISTING.RefId = REF_NEW.RefId
				AND REF_EXISTING.DeletedAt IS NULL
				AND REF_EXISTING.ProcessedAt IS NULL
		WHERE
			REF_EXISTING.RefId IS NULL

		SELECT @ERROR = @@ERROR

		IF (@ERROR = 0)
			BEGIN
				PRINT 'Transaction committed. Records inserted into duplrefs.ReferencesQueue'
				COMMIT TRANSACTION
			END
		ELSE
			BEGIN
				PRINT 'ERROR(S):  ' + CAST(@ERROR AS VARCHAR(10))
				PRINT 'Transaction NOT committed'
				ROLLBACK TRANSACTION
			END
	END

RETURN 0
