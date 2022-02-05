--This can not use the sproc because it loos at 2 fields
USE UDW
GO
DECLARE 
	@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(LN90.LF_LST_DTS_LN90)), '1-1-1900 00:00:00'), 21) FROM LN90_FIN_ATY LN90)
DECLARE @LastRefreshStatusDate VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(LN90.LD_STA_LON90)), '1-1-1900 00:00:00'), 21) FROM LN90_FIN_ATY LN90)

PRINT 'Last Refreshed timestamp at: ' + @LastRefresh
PRINT 'Last Refreshed status date at: ' + @LastRefreshStatusDate


DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.LN90_FIN_ATY LN90
	USING
		(
			SELECT
				*
			FROM
				OPENQUERY
				(
					DUSTER,
					''
						SELECT
							*
						FROM
							OLWHRM1.LN90_FIN_ATY LN90
						-- comment WHERE clause for full table refresh
						WHERE
							LN90.LF_LST_DTS_LN90 > ''''' + @LastRefresh + '''''
							OR LN90.LD_STA_LON90 >= ''''' + @LastRefreshStatusDate + '''''
							OR
							(
								LN90.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) L 
			ON L.BF_SSN = LN90.BF_SSN 
			AND L.LN_SEQ = LN90.LN_SEQ 
			AND L.LN_FAT_SEQ = LN90.LN_FAT_SEQ
	WHEN MATCHED THEN 
		UPDATE SET 
			LN90.LC_FAT_REV_REA = L.LC_FAT_REV_REA,
			LN90.LD_FAT_APL = L.LD_FAT_APL,
			LN90.LD_FAT_PST = L.LD_FAT_PST,
			LN90.LD_FAT_EFF = L.LD_FAT_EFF,
			LN90.LD_FAT_DPS = L.LD_FAT_DPS,
			LN90.LC_CSH_ADV = L.LC_CSH_ADV,
			LN90.LD_STA_LON90 = L.LD_STA_LON90,
			LN90.LC_STA_LON90 = L.LC_STA_LON90,
			LN90.LA_FAT_PCL_FEE = L.LA_FAT_PCL_FEE,
			LN90.LA_FAT_NSI = L.LA_FAT_NSI,
			LN90.LA_FAT_LTE_FEE = L.LA_FAT_LTE_FEE,
			LN90.LA_FAT_ILG_PRI = L.LA_FAT_ILG_PRI,
			LN90.LA_FAT_CUR_PRI = L.LA_FAT_CUR_PRI,
			LN90.LF_LST_DTS_LN90 = L.LF_LST_DTS_LN90,
			LN90.PC_FAT_TYP = L.PC_FAT_TYP,
			LN90.PC_FAT_SUB_TYP = L.PC_FAT_SUB_TYP,
			LN90.LA_FAT_NSI_ACR = L.LA_FAT_NSI_ACR,
			LN90.LI_FAT_RAP = L.LI_FAT_RAP,
			LN90.LN_FAT_SEQ_REV = L.LN_FAT_SEQ_REV,
			LN90.LI_EFT_NSF_OVR = L.LI_EFT_NSF_OVR,
			LN90.LF_USR_EFT_NSF_OVR = L.LF_USR_EFT_NSF_OVR,
			LN90.LA_FAT_MSC_FEE = L.LA_FAT_MSC_FEE,
			LN90.LA_FAT_MSC_FEE_PCV = L.LA_FAT_MSC_FEE_PCV,
			LN90.LA_FAT_DL_REB = L.LA_FAT_DL_REB
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_SEQ,
			LN_FAT_SEQ,
			LC_FAT_REV_REA,
			LD_FAT_APL,
			LD_FAT_PST,
			LD_FAT_EFF,
			LD_FAT_DPS,
			LC_CSH_ADV,
			LD_STA_LON90,
			LC_STA_LON90,
			LA_FAT_PCL_FEE,
			LA_FAT_NSI,
			LA_FAT_LTE_FEE,
			LA_FAT_ILG_PRI,
			LA_FAT_CUR_PRI,
			LF_LST_DTS_LN90,
			PC_FAT_TYP,
			PC_FAT_SUB_TYP,
			LA_FAT_NSI_ACR,
			LI_FAT_RAP,
			LN_FAT_SEQ_REV,
			LI_EFT_NSF_OVR,
			LF_USR_EFT_NSF_OVR,
			LA_FAT_MSC_FEE,
			LA_FAT_MSC_FEE_PCV,
			LA_FAT_DL_REB
		)
		VALUES 
		(
			L.BF_SSN,
			L.LN_SEQ,
			L.LN_FAT_SEQ,
			L.LC_FAT_REV_REA,
			L.LD_FAT_APL,
			L.LD_FAT_PST,
			L.LD_FAT_EFF,
			L.LD_FAT_DPS,
			L.LC_CSH_ADV,
			L.LD_STA_LON90,
			L.LC_STA_LON90,
			L.LA_FAT_PCL_FEE,
			L.LA_FAT_NSI,
			L.LA_FAT_LTE_FEE,
			L.LA_FAT_ILG_PRI,
			L.LA_FAT_CUR_PRI,
			L.LF_LST_DTS_LN90,
			L.PC_FAT_TYP,
			L.PC_FAT_SUB_TYP,
			L.LA_FAT_NSI_ACR,
			L.LI_FAT_RAP,
			L.LN_FAT_SEQ_REV,
			L.LI_EFT_NSF_OVR,
			L.LF_USR_EFT_NSF_OVR,
			L.LA_FAT_MSC_FEE,
			L.LA_FAT_MSC_FEE_PCV,
			L.LA_FAT_DL_REB
		)
	-- !!!  uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	    --DELETE
	;
'
--select @SQLStatement
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
		DUSTER,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				OLWHRM1.LN90_FIN_ATY LN90
		'	
	) R
	LEFT OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			LN90_FIN_ATY LN90
	) L 
		ON 1 = 1
	
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('LN90_FIN_ATY - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
	END
ELSE IF @CountDifference != 0 AND @LoopCount = 0
	BEGIN

		SET @LoopCount = @LoopCount + 1
		
		DECLARE @SSN_LIST TABLE
		(
			R_BF_SSN CHAR(9),
			L_BF_SSN CHAR(9)
		)

		PRINT 'Insert SSN with inconsistent counts'
		INSERT INTO
			@SSN_LIST
		SELECT TOP 20
			R.BF_SSN,
			L.BF_SSN
		FROM
			OPENQUERY
			(
				DUSTER,	
				'
					SELECT
						LN90.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.LN90_FIN_ATY LN90
					GROUP BY
						LN90.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					LN90.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					UDW..LN90_FIN_ATY LN90
				GROUP BY
					LN90.BF_SSN
			) L 
				ON L.BF_SSN = R.BF_SSN
		WHERE
			ISNULL(L.LocalCount, 0) != ISNULL(R.RemoteCount, 0)

		SELECT
			@SSNs = 
			(
				SELECT
					'''''' + COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) + ''''',' AS [text()]
				FROM
					@SSN_LIST SL
				ORDER BY
					COALESCE(SL.L_BF_SSN, SL.R_BF_SSN)
				FOR XML PATH ('')
			)

		SELECT	@SSNs = LEFT(@SSNs, LEN(@SSNs) -1)

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local LN90 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			LN90
		FROM
			UDW..LN90_FIN_ATY LN90
			INNER JOIN @SSN_LIST SL 
				ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = LN90.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END
