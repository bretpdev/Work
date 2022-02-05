CREATE PROCEDURE [docid].[InsertProcessedRecord]
	@AccountIdentifier char(10),
	@Document char(5),
	@Source char(2),
	@ArcAddProcessingId int,
	@AddedBy varchar(50)
AS

	DECLARE @DocumentId INT = (SELECT DocumentsId FROM docid.Documents WHERE Document = @Document)
	DECLARE @SourceId INT = (SELECT SourceId FROM docid.Sources WHERE Source = @Source)

	INSERT INTO docid.DocumentsProcessed(AccountIdentifier, DocumentsId, SourceId, ArcAddProcessingId, AddedBy)
	VALUES(@AccountIdentifier, @DocumentId, @SourceId, @ArcAddProcessingId, @AddedBy)
	
RETURN 0