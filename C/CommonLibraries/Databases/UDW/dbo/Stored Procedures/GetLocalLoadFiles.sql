CREATE PROCEDURE [dbo].[GetLocalLoadFiles]

AS
	SELECT 
		LLD.LocalLoadDataID,
		LocalLoadFile,
		ReportNumber,
		SasCodeName
	FROM
		LocalLoadData LLD
	INNER JOIN  LocalLoadFiles LLF
		ON LLD.LocalLoadFileID = LLF.LocalLoadFileID
RETURN 0