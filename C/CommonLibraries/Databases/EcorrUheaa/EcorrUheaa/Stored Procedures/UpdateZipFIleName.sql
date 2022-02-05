CREATE PROCEDURE [dbo].[UpdateZipFIleName]
	 @DDIds as Ids readonly,
	 @ZipFIleName varchar(300)
AS
	UPDATE DD
	SET 
		ZipFileName = @ZipFIleName
	FROM
		DocumentDetails DD
	INNER JOIN @DDIds IDS
		ON IDS.DocumentDetailsId = DD.DocumentDetailsId


RETURN 0
