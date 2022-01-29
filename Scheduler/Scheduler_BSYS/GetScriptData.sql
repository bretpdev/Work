CREATE PROCEDURE [scheduler].[GetScriptData]
AS

	SELECT
		2 RequestTypeId,
		SCR.[Request] [Id],
		SCR.Title [Name],
		SCR.CurrentStatus [Status],
		CAST(CAST([Priority] AS DECIMAL(9, 2)) AS TINYINT) [Priority], 
		SCR.[Court],
		P.Programmer [AssignedProgrammer],
		T.Tester [AssignedTester]
	FROM
		[SCKR_DAT_ScriptRequests] SCR
		LEFT JOIN
		[SCKR_REF_Programmer] P 
			ON 
				SCR.[Request] = P.[Request]
			AND
				P.[Sequence] = 
				(
					SELECT
						TOP 1 [Sequence]
					FROM
						[SCKR_REF_Programmer]
					WHERE
						SCR.Request = Request 
						AND 
						Class = 'Scr'
						AND
						[End] IS NULL 
					ORDER BY 
						[Begin] DESC
				)
				LEFT JOIN
		[SCKR_REF_Tester] T 
			ON 
				SCR.[Request] = T.[Request]
			AND
				T.[Sequence] = 
				(
					SELECT
						TOP 1 [Sequence]
					FROM
						[SCKR_REF_Tester]
					WHERE
						SCR.Request = Request 
						AND 
						Class = 'Scr'
						AND
						[End] IS NULL 
					ORDER BY 
						[Begin] DESC
				)

RETURN 0
