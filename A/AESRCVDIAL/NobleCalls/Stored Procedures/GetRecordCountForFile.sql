CREATE PROCEDURE [aesrcvdial].[GetRecordCountForFile]
	@FileName VARCHAR(250)
AS
	SELECT
		COUNT(*)
	FROM
		aesrcvdial.OnelinkDialFileInput
	WHERE
		CAST(AddedAt AS DATE) = CAST(GETDATE() AS DATE)
		AND [FileName] LIKE @FileName + '%'