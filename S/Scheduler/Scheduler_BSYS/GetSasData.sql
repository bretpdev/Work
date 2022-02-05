CREATE PROCEDURE [scheduler].[GetSasData]
AS

	SELECT
		3 RequestTypeId,
		SAS.[Request] [Id],
		SAS.Title [Name],
		SAS.CurrentStatus [Status],
		CAST(CAST([Priority] AS DECIMAL(9, 2)) AS TINYINT) [Priority], 
		SAS.[Court],
		P.Programmer [AssignedProgrammer],
		T.Tester [AssignedTester]
	FROM
		[SCKR_DAT_SASRequests] SAS
		LEFT JOIN
		[SCKR_REF_Programmer] P 
			ON 
				SAS.[Request] = P.[Request]
			AND
				P.[Sequence] = 
				(
					SELECT
						TOP 1 [Sequence]
					FROM
						[SCKR_REF_Programmer]
					WHERE
						SAS.Request = Request 
						AND 
						Class = 'SAS'
						AND
						[End] IS NULL 
					ORDER BY 
						[Begin] DESC
				)
				LEFT JOIN
		[SCKR_REF_Tester] T 
			ON 
				SAS.[Request] = T.[Request]
			AND
				T.[Sequence] = 
				(
					SELECT
						TOP 1 [Sequence]
					FROM
						[SCKR_REF_Tester]
					WHERE
						SAS.Request = Request 
						AND 
						Class = 'SAS'
						AND
						[End] IS NULL 
					ORDER BY 
						[Begin] DESC
				)
RETURN 0
