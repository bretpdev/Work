CREATE PROCEDURE [dbo].[BatchLettersUICommitChanges]
	@Data BatchLettersData READONLY
	
AS
	BEGIN TRANSACTION
	DELETE FROM BatchLetters

	SET IDENTITY_INSERT [dbo].[BatchLetters] ON 

	INSERT INTO BatchLetters(BatchLettersId,LetterId,SasFilePattern,StateFieldCodeName,AccountNumberFieldName,CostCenterFieldCodeName,IsDuplex,OkIfMissing,ProcessAllFiles,Arc,Comment,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,Active)
	SELECT
		BatchLettersId,
		LetterId,
		SasFilePattern,
		StateFieldCodeName,
		AccountNumberFieldName,
		CostCenterFieldCodeName,
		IsDuplex,
		OkIfMissing,
		ProcessAllFiles,
		Arc,
		Comment,
		CreatedAt,
		CreatedBy,
		UpdatedAt,
		UpdatedBy,
		Active
	FROM 
		@Data

	SET IDENTITY_INSERT [dbo].[BatchLetters] OFF

	COMMIT
RETURN 0
