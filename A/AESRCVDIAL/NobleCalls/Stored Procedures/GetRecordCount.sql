CREATE PROCEDURE [aesrcvdial].[GetRecordCount]
	@FileName VARCHAR(250)
AS
	SELECT
		COUNT(*)
	FROM
		aesrcvdial.OnelinkDialFileInput
	WHERE
		[FileName] = @FileName
		AND CAST(AddedAt AS DATE) = CAST(GETDATE() AS DATE)
		AND DeletedAt IS NULL