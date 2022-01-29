CREATE PROCEDURE [cslsltrfed].[GetFileHeader]
	@Letter AS VARCHAR(20),
	@ScriptId AS VARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		FH.FileHeader
	FROM
		CLS.[print].Letters L
		INNER JOIN CLS.[print].[ScriptData] SD
			ON L.LetterId = SD.LetterId
			AND SD.ScriptId = @ScriptId
		INNER JOIN CLS.[print].[FileHeaders] FH
			ON SD.FileHeaderId = FH.FileHeaderId
	WHERE
		SD.Active = 1
		AND L.Letter = @Letter
END