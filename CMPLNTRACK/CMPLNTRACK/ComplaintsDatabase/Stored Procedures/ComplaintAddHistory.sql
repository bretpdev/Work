CREATE PROCEDURE [complaints].[ComplaintAddHistory]
	@ComplaintId INT,
	@HistoryDetail NVARCHAR(4000),
	@UpdateResolvesComplaint BIT
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	INSERT INTO [complaints].[ComplaintHistory] (ComplaintId, HistoryDetail)
	VALUES (@ComplaintId, @HistoryDetail)

	DECLARE @Id INT = CAST(SCOPE_IDENTITY() AS INT)

	IF @UpdateResolvesComplaint = 1
	BEGIN
		UPDATE
			[complaints].Complaints
		SET
			ResolutionComplaintHistoryId = @Id
		WHERE
			ComplaintId = @ComplaintId
	END

	SELECT @Id

RETURN 0