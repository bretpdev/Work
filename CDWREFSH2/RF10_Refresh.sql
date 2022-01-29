USE CDW
GO

----add table to OPSDEV:
--SELECT TOP 1
--	*
--INTO 
--	dbo.RF10_RFR
--FROM
--	OPENQUERY
--	(
--		LEGEND,
--		'
--			SELECT
--				*
--			FROM
--				PKUB.RF10_RFR
--		'
--	)

----change CHAR to VARCHAR if length > 1 and not an SSN or Account Number
--ALTER TABLE [CDW].[dbo].[RF10_RFR]
--ALTER COLUMN [BC_RFR_REL_BR] VARCHAR(2)

--ALTER TABLE [CDW].[dbo].[RF10_RFR]
--ALTER COLUMN [BF_RFR] VARCHAR(9)

--ALTER TABLE [CDW].[dbo].[RF10_RFR]
--ALTER COLUMN [BF_LST_USR_HST_RFR] VARCHAR(8)

DECLARE 
	@SSNs VARCHAR(MAX) = '0', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(RF10.BF_LST_DTS_RF10), '1-1-1900 00:00:00'), 21) FROM RF10_RFR RF10)
PRINT 'Last Refreshed at: ' + @LastRefresh


DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.RF10_RFR RF10
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
							*
						FROM
							PKUB.RF10_RFR RF10
						-- comment WHERE clause for full table refresh
						WHERE
							RF10.BF_LST_DTS_RF10 > ''''' + @LastRefresh + '''''
							OR
							(
								RF10.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) L 
			ON L.BF_SSN = RF10.BF_SSN 
			AND L.BN_SEQ_RFR = RF10.BN_SEQ_RFR 
	WHEN MATCHED THEN 
		UPDATE SET 
			 RF10.BF_SSN			 = L.BF_SSN
			,RF10.BN_SEQ_RFR		 = L.BN_SEQ_RFR
			,RF10.BC_STA_REFR10		 = L.BC_STA_REFR10
			,RF10.BI_ATH_3_PTY		 = L.BI_ATH_3_PTY
			,RF10.BC_RFR_REL_BR		 = L.BC_RFR_REL_BR
			,RF10.BD_EFF_RFR		 = L.BD_EFF_RFR
			,RF10.BF_RFR			 = L.BF_RFR
			,RF10.BC_RFR_TYP		 = L.BC_RFR_TYP
			,RF10.BF_LST_DTS_RF10	 = L.BF_LST_DTS_RF10
			,RF10.BD_EFF_RFR_HST	 = L.BD_EFF_RFR_HST
			,RF10.BC_REA_RFR_HST	 = L.BC_REA_RFR_HST
			,RF10.BF_LST_USR_HST_RFR = L.BF_LST_USR_HST_RFR
			,RF10.BD_ATH_3_PTY_END	 = L.BD_ATH_3_PTY_END
	WHEN NOT MATCHED THEN
		INSERT 
		(
			 BF_SSN			
			,BN_SEQ_RFR		
			,BC_STA_REFR10	    
			,BI_ATH_3_PTY	    
			,BC_RFR_REL_BR	    
			,BD_EFF_RFR		
			,BF_RFR			
			,BC_RFR_TYP		
			,BF_LST_DTS_RF10   
			,BD_EFF_RFR_HST	
			,BC_REA_RFR_HST	
			,BF_LST_USR_HST_RFR
			,BD_ATH_3_PTY_END	
		)
		VALUES 
		(
			 L.BF_SSN
			,L.BN_SEQ_RFR
			,L.BC_STA_REFR10
			,L.BI_ATH_3_PTY
			,L.BC_RFR_REL_BR
			,L.BD_EFF_RFR
			,L.BF_RFR
			,L.BC_RFR_TYP
			,L.BF_LST_DTS_RF10
			,L.BD_EFF_RFR_HST
			,L.BC_REA_RFR_HST
			,L.BF_LST_USR_HST_RFR
			,L.BD_ATH_3_PTY_END
		)
	-- !!! uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	    --DELETE
	;
'
--PRINT @SQLStatement
EXEC (@SQLStatement)

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
				PKUB.RF10_RFR
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			RF10_RFR
	) L 
		ON 1 = 1
	
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('RF10_RFR - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
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
						RF10.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						PKUB.RF10_RFR RF10
					GROUP BY
						RF10.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					RF10.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					CDW..RF10_RFR RF10
				GROUP BY
					RF10.BF_SSN
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

		PRINT 'The local record count for some SSNs do not match the remote warehouse count.  Deleting all local RF10 records for these borrowers and fullying refreshing from the remote warehouse.'

		DELETE FROM
			RF10
		FROM
			CDW..RF10_RFR RF10
			INNER JOIN @SSN_LIST SL
				ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = RF10.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END