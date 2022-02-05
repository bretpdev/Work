USE ODW
GO

DECLARE 
	@LoopCount TINYINT = 0

RefreshStart:

--does a full refresh because DC02_BAL_INT doesn't have last update date/timestamp
--DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,CAST(MAX(DC02.LF_CRT_DTS_DC10) AS DATETIME)), '1-1-1900 00:00:00'), 21) FROM DC02_BAL_INT DC02)
--PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
		MERGE 
			dbo.DC02_BAL_INT LOCAL
		USING
			(
				SELECT
					BF_SSN,
					LF_CLM_ID,
					LA_CLM_BAL,
					LA_CLM_INT_ACR,
					LA_CLM_PRJ_COL_CST,
					AF_APL_ID,
					AF_APL_ID_SFX,
					LF_CRT_DTS_DC10,
					LD_ETR,
					LR_CUR_INT
				FROM
					OPENQUERY
					(
						DUSTER,
						''
							SELECT
								REMOTE.BF_SSN,
								REMOTE.LF_CLM_ID,
								REMOTE.LA_CLM_BAL,
								REMOTE.LA_CLM_INT_ACR,
								REMOTE.LA_CLM_PRJ_COL_CST,
								REMOTE.AF_APL_ID,
								REMOTE.AF_APL_ID_SFX,
								REMOTE.LF_CRT_DTS_DC10,
								REMOTE.LD_ETR,
								REMOTE.LR_CUR_INT
							FROM
								OLWHRM1.DC02_BAL_INT REMOTE
						''

					)
			) R
				ON R.BF_SSN = LOCAL.BF_SSN 
				AND R.AF_APL_ID = LOCAL.AF_APL_ID 
				AND R.AF_APL_ID_SFX = LOCAL.AF_APL_ID_SFX 
				AND R.LF_CRT_DTS_DC10 = LOCAL.LF_CRT_DTS_DC10
		WHEN MATCHED THEN 
			UPDATE SET
				LOCAL.BF_SSN = R.BF_SSN,
				LOCAL.LF_CLM_ID = R.LF_CLM_ID,
				LOCAL.LA_CLM_BAL = R.LA_CLM_BAL,
				LOCAL.LA_CLM_INT_ACR = R.LA_CLM_INT_ACR,
				LOCAL.LA_CLM_PRJ_COL_CST = R.LA_CLM_PRJ_COL_CST,
				LOCAL.AF_APL_ID = R.AF_APL_ID,
				LOCAL.AF_APL_ID_SFX = R.AF_APL_ID_SFX,
				LOCAL.LF_CRT_DTS_DC10 = R.LF_CRT_DTS_DC10,
				LOCAL.LD_ETR = R.LD_ETR,
				LOCAL.LR_CUR_INT = R.LR_CUR_INT
		WHEN NOT MATCHED THEN
			INSERT 
			(
				BF_SSN,
				LF_CLM_ID,
				LA_CLM_BAL,
				LA_CLM_INT_ACR,
				LA_CLM_PRJ_COL_CST,
				AF_APL_ID,
				AF_APL_ID_SFX,
				LF_CRT_DTS_DC10,
				LD_ETR,
				LR_CUR_INT
			)
			VALUES 
			(
				R.BF_SSN,
				R.LF_CLM_ID,
				R.LA_CLM_BAL,
				R.LA_CLM_INT_ACR,
				R.LA_CLM_PRJ_COL_CST,
				R.AF_APL_ID,
				R.AF_APL_ID_SFX,
				R.LF_CRT_DTS_DC10,
				R.LD_ETR,
				R.LR_CUR_INT
			)
			-- !!! full table refresh 
			WHEN NOT MATCHED BY SOURCE THEN
				DELETE
			
;
'


PRINT @SQLStatement
EXEC (@SQLStatement)


-- ###### VALIDATION
DECLARE 
	@CountDifference INT

SELECT
	@CountDifference = L.LocalCount - R.RemoteCount
FROM
	OPENQUERY
	(
		duster,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				OLWHRM1. DC02_BAL_INT DC02
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) AS [LocalCount]
		FROM
			dbo. DC02_BAL_INT DC02
	) L ON 1 = 1
	
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR(' DC02_BAL_INT - The remote and local record counts do not match.  The local count is off by %i records.  Rerun refresh query.', 16, 11, @CountDifference)
	END
ELSE IF @CountDifference != 0 AND @LoopCount = 0
	BEGIN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		SET @LoopCount = @LoopCount + 1

		GOTO RefreshStart;

	END
