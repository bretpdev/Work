--sproc can’t be used due to ENCRYPTBYKEY logic 

USE UDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.BR30_BR_EFT
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.BR30_BR_EFT
--		'
--	)
	 
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
					DUSTER,
					''
						SELECT
							BR30.*
						FROM
							OLWHRM1.BR30_BR_EFT BR30
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
					''
				) 
		) D 
			ON D.BF_SSN = BR30.BF_SSN 
			AND D.BN_EFT_SEQ = BR30.BN_EFT_SEQ
	WHEN MATCHED THEN 
		UPDATE SET
			BR30.BF_EFT_ABA = ENCRYPTBYKEY(Key_GUID(''USHE_Financial_Data_Key''), D.BF_EFT_ABA),
			BR30.BF_EFT_ACC = ENCRYPTBYKEY(Key_GUID(''USHE_Financial_Data_Key''), D.BF_EFT_ACC),
			BR30.BC_EFT_TYP_ACC = D.BC_EFT_TYP_ACC,
			BR30.BC_EFT_STA = D.BC_EFT_STA,
			BR30.BD_EFT_STA = D.BD_EFT_STA,
			BR30.BF_LST_DTS_BR30 = D.BF_LST_DTS_BR30,
			BR30.BD_EFT_PNO_SNT = D.BD_EFT_PNO_SNT,
			BR30.BA_EFT_ADD_WDR = D.BA_EFT_ADD_WDR,
			BR30.BN_EFT_NSF_CTR = D.BN_EFT_NSF_CTR,
			BR30.BN_EFT_DAY_DUE = D.BN_EFT_DAY_DUE,
			BR30.BA_EFT_LST_WDR = D.BA_EFT_LST_WDR,
			BR30.BA_EFT_TOL = D.BA_EFT_TOL,
			BR30.BC_EFT_DNL_REA = D.BC_EFT_DNL_REA,
			BR30.DF_PRS_ID = D.DF_PRS_ID,
			BR30.BC_EFT_PAY_OPT = D.BC_EFT_PAY_OPT,
			BR30.BC_SRC_DIR_DBT_APL = D.BC_SRC_DIR_DBT_APL,
			BR30.BC_DDT_PAY_PRS_TYP = D.BC_DDT_PAY_PRS_TYP,
			BR30.BF_DDT_PAY_EDS = D.BF_DDT_PAY_EDS
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
			D.BF_SSN,
			D.BN_EFT_SEQ,
			ENCRYPTBYKEY(Key_GUID(''USHE_Financial_Data_Key''), D.BF_EFT_ABA),
			ENCRYPTBYKEY(Key_GUID(''USHE_Financial_Data_Key''), D.BF_EFT_ACC),
			D.BC_EFT_TYP_ACC,
			D.BC_EFT_STA,
			D.BD_EFT_STA,
			D.BF_LST_DTS_BR30,
			D.BD_EFT_PNO_SNT,
			D.BA_EFT_ADD_WDR,
			D.BN_EFT_NSF_CTR,
			D.BN_EFT_DAY_DUE,
			D.BA_EFT_LST_WDR,
			D.BA_EFT_TOL,
			D.BC_EFT_DNL_REA,
			D.DF_PRS_ID,
			D.BC_EFT_PAY_OPT,
			D.BC_SRC_DIR_DBT_APL,
			D.BC_DDT_PAY_PRS_TYP,
			D.BF_DDT_PAY_EDS
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
		DUSTER,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				OLWHRM1.BR30_BR_EFT BR30
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			UDW..BR30_BR_EFT BR30
	) L 
		ON 1 = 1
	
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('BR30_BR_EFT - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
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
						BR30.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.BR30_BR_EFT BR30
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
					UDW..BR30_BR_EFT BR30
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
			UDW..BR30_BR_EFT BR30
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = BR30.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END

