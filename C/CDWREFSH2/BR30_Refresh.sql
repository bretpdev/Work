--sproc cannot be used due to ENCRYPTBYKEY functionality

USE CDW
GO

DECLARE 
	@SSNs VARCHAR(MAX) = '0', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(BR30.BF_LST_DTS_BR30)), '1-1-1900 00:00:00'), 21) FROM BR30_BR_EFT BR30)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.BR30_BR_EFT BR30
	USING
		(
			SELECT
				*
			FROM
				OPENQUERY
				(
					LEGEND,
					''
						SELECT
							BR30.*
						FROM
							PKUB.BR30_BR_EFT BR30
						-- comment WHERE clause for full table refresh
						WHERE
							BR30.BF_LST_DTS_BR30 > ''''' + @LastRefresh + '''''
							OR
							(
								BR30.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
							FOR READ ONLY WITH UR
					''
				) 
		) L 
			ON L.BF_SSN = BR30.BF_SSN 
			AND L.BN_EFT_SEQ = BR30.BN_EFT_SEQ
	WHEN MATCHED THEN 
		UPDATE SET
			BR30.BF_EFT_ABA = ENCRYPTBYKEY(Key_GUID(''USHE_Financial_Data_Key''), L.BF_EFT_ABA),
			BR30.BF_EFT_ACC = ENCRYPTBYKEY(Key_GUID(''USHE_Financial_Data_Key''), L.BF_EFT_ACC),
			BR30.BC_EFT_TYP_ACC = L.BC_EFT_TYP_ACC,
			BR30.BC_EFT_STA = L.BC_EFT_STA,
			BR30.BD_EFT_STA = L.BD_EFT_STA,
			BR30.BF_LST_DTS_BR30 = L.BF_LST_DTS_BR30,
			BR30.BD_EFT_PNO_SNT = L.BD_EFT_PNO_SNT,
			BR30.BA_EFT_ADD_WDR = L.BA_EFT_ADD_WDR,
			BR30.BN_EFT_NSF_CTR = L.BN_EFT_NSF_CTR,
			BR30.BN_EFT_DAY_DUE = L.BN_EFT_DAY_DUE,
			BR30.BA_EFT_LST_WDR = L.BA_EFT_LST_WDR,
			BR30.BA_EFT_TOL = L.BA_EFT_TOL,
			BR30.BC_EFT_DNL_REA = L.BC_EFT_DNL_REA,
			BR30.DF_PRS_ID = L.DF_PRS_ID,
			BR30.BC_EFT_PAY_OPT = L.BC_EFT_PAY_OPT,
			BR30.BC_SRC_DIR_DBT_APL = L.BC_SRC_DIR_DBT_APL,
			BR30.BC_DDT_PAY_PRS_TYP = L.BC_DDT_PAY_PRS_TYP,
			BR30.BF_DDT_PAY_EDS = L.BF_DDT_PAY_EDS
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			BN_EFT_SEQ,
			BF_EFT_ABA,
			BF_EFT_ACC,
			BC_EFT_TYP_ACC,
			BC_EFT_STA,
			BD_EFT_STA,
			BF_LST_DTS_BR30,
			BD_EFT_PNO_SNT,
			BA_EFT_ADD_WDR,
			BN_EFT_NSF_CTR,
			BN_EFT_DAY_DUE,
			BA_EFT_LST_WDR,
			BA_EFT_TOL,
			BC_EFT_DNL_REA,
			DF_PRS_ID,
			BC_EFT_PAY_OPT,
			BC_SRC_DIR_DBT_APL,
			BC_DDT_PAY_PRS_TYP,
			BF_DDT_PAY_EDS
		)
		VALUES 
		(
			L.BF_SSN,
			L.BN_EFT_SEQ,
			ENCRYPTBYKEY(Key_GUID(''USHE_Financial_Data_Key''), L.BF_EFT_ABA),
			ENCRYPTBYKEY(Key_GUID(''USHE_Financial_Data_Key''), L.BF_EFT_ACC),
			L.BC_EFT_TYP_ACC,
			L.BC_EFT_STA,
			L.BD_EFT_STA,
			L.BF_LST_DTS_BR30,
			L.BD_EFT_PNO_SNT,
			L.BA_EFT_ADD_WDR,
			L.BN_EFT_NSF_CTR,
			L.BN_EFT_DAY_DUE,
			L.BA_EFT_LST_WDR,
			L.BA_EFT_TOL,
			L.BC_EFT_DNL_REA,
			L.DF_PRS_ID,
			L.BC_EFT_PAY_OPT,
			L.BC_SRC_DIR_DBT_APL,
			L.BC_DDT_PAY_PRS_TYP,
			L.BF_DDT_PAY_EDS
		)
	-- !!! uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	--    DELETE
	;
'

PRINT @SQLStatement
OPEN SYMMETRIC KEY USHE_Financial_Data_Key DECRYPTION BY CERTIFICATE USHE_Financial_Encryption_Certificate;
EXEC (@SQLStatement)
CLOSE SYMMETRIC KEY USHE_Financial_Data_Key;


-- ###### VALIDATION
DECLARE 
	@CountDifference INT

SELECT
	@CountDifference = L.LocalCount - R.RemoteCount
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				PKUB.BR30_BR_EFT BR30
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			CDW..BR30_BR_EFT BR30
	) L 
		ON 1 = 1
	
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('BR30_BR_EFT - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is requireL.', 16, 11, @CountDifference)
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
				LEGEND,	
				'
					SELECT
						BR30.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						PKUB.BR30_BR_EFT BR30
					GROUP BY
						BR30.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					BR30.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					CDW..BR30_BR_EFT BR30
				GROUP BY
					BR30.BF_SSN
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local BR30 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			BR30
		FROM
			CDW..BR30_BR_EFT BR30
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = BR30.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END