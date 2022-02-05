CREATE PROCEDURE [fp].[GetPendingAccountsForScript]
	@ScriptId varchar(10)
AS
	SELECT
		FP.FileProcessingId,
		FP.GroupKey,
		SF.SourceFile,
		FP.ProcessedAt,
		L.Letter AS LetterId,
		D.DocIdName AS DocId,
		FH.FileHeader,
		A.Arc,
		A.Comment
	FROM
		ScriptFiles SF
		LEFT JOIN FileHeaders FH
			ON FH.FileHeaderId = SF.FileHeaderId
		LEFT JOIN Letters L
			ON L.LetterId = SF.LetterId
		LEFT JOIN DocIds D
			ON D.DocId = SF.DocId
		LEFT JOIN fp.FileProcessing FP
			ON FP.ScriptFileId = SF.ScriptFileId
		LEFT JOIN Arcs A
			ON A.ArcId = SF.ArcId
	WHERE
		sf.ScriptID = @ScriptId
		AND FP.ProcessedAt IS NULL
		AND FP.Active = 1
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[fp].[GetPendingAccountsForScript] TO [db_executor]
    AS [UHEAA\Developers];

