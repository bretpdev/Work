CREATE PROCEDURE [aesrcvdial].[GetDialerFiles]
AS
	SELECT
		[FileName],
		OutputFileName,
		IsRequired
	FROM
		aesrcvdial.DialerFiles
	WHERE
		Active = 1