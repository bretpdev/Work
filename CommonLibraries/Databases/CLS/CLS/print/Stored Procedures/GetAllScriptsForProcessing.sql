CREATE PROCEDURE [print].[GetAllScriptsForProcessing]

AS
	SELECT 
		SD.ScriptDataId,
		SD.ScriptID,
		SD.SourceFile,
		L.Letter,
		DID.DocIdName,
		FH.FileHeader,
		FH.StateIndex as StateFieldIndex,
		SD.ProcessAllFiles,
		SD.IsEndorser,
		SD.EndorsersBorrowerSSNIndex,
		SD.[Priority],
		SD.DoNotProcessEcorr,
		SD.AddBarCodes
	FROM
		ScriptData SD
		INNER JOIN FileHeaders FH
			ON FH.FileHeaderId = SD.FileHeaderId
		INNER JOIN Letters L
			ON L.LetterId = SD.LetterId
		LEFT JOIN DocIds DID
			ON DID.DocIdId = SD.DocIdId
	WHERE
		SD.Active = 1
		and sd.SourceFile IS NOT NULL
	ORDER BY
		SD.[Priority]
RETURN 0