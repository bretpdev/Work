CREATE PROCEDURE [fp].[GetPendingAccountsForScript]
	@ScriptId varchar(10)
AS
	SELECT
		FP.FileProcessingId,
		FP.GroupKey AS [AccountNumber],
		SF.SourceFile,
		FP.ProcessedAt,
		L.Letter AS LetterId,
		D.DocIdName AS DocId,
		FH.FileHeader,
		A.Arc,
		A.Comment,
		FP.OnEcorr
	FROM
		ScriptFiles SF
		INNER JOIN FileHeaders FH
			ON FH.FileHeaderId = SF.FileHeaderId
		INNER JOIN Letters L
			ON L.LetterId = SF.LetterId
		LEFT JOIN DocIds D
			ON D.DocId = SF.DocId
		INNER JOIN fp.FileProcessing FP
			ON FP.ScriptFileId = SF.ScriptFileId
		LEFT JOIN Arcs A
			ON A.ArcId = SF.ArcId
	WHERE
		SF.ScriptID = @ScriptId
		AND FP.ProcessedAt IS NULL
		AND FP.DeletedAt IS NULL
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[fp].[GetPendingAccountsForScript] TO [db_executor]
    AS [dbo];

