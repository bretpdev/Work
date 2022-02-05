--more than one field in primary key: ) L ON L.BF_SSN = RM03.BF_SSN AND L.BN_EFT_SEQ = RM03.BN_EFT_SEQ

USE CDW
GO

DECLARE 
	@SSNs VARCHAR(MAX) = '0', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(RM03.NF_ONL_PAY_DTS)), '1-1-1900 00:00:00'), 21) FROM RM03_ONL_PAY RM03)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
	'
                MERGE 
                    dbo.RM03_ONL_PAY RM03
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
										RM03.*
                                    FROM
                                        WEBFLS1.RM03_ONL_PAY RM03
                                    -- comment WHERE clause for full table refresh
                                    WHERE
                                        RM03.NF_ONL_PAY_DTS > ''''' + @LastRefresh + '''''
                                        OR
                                        (
											RM03.BF_SSN IN
											(
												' + @SSNs + '
											)
                                        )
                                ''
                            ) 
                    ) L 
						ON L.PF_SVC_CLI = RM03.PF_SVC_CLI 
						AND L.PF_IRL_GNR_ID = RM03.PF_IRL_GNR_ID 
						AND L.BF_SSN = RM03.BF_SSN
                WHEN MATCHED THEN 
                                UPDATE SET
									RM03.DF_PAY_ABA = ENCRYPTBYKEY(Key_GUID(''USHE_Financial_Data_Key''), L.DF_PAY_ABA),
									RM03.DF_BNK_ACC = ENCRYPTBYKEY(Key_GUID(''USHE_Financial_Data_Key''), L.DF_BNK_ACC),
									RM03.PF_SVC_CLI = L.PF_SVC_CLI,
									RM03.PF_IRL_GNR_ID = L.PF_IRL_GNR_ID,
									RM03.BF_SSN = L.BF_SSN,
									RM03.BA_PAY = L.BA_PAY,
									RM03.LD_PAY = L.LD_PAY,
									RM03.DF_ACC_TYP = L.DF_ACC_TYP,
									RM03.DF_EML = L.DF_EML,
									RM03.BN_1 = L.BN_1,
									RM03.BN_MI = L.BN_MI,
									RM03.BN_LST = L.BN_LST,
									RM03.DM_1_ACC_OWN = L.DM_1_ACC_OWN,
									RM03.DM_MI_ACC_OWN = L.DM_MI_ACC_OWN,
									RM03.DM_LST_ACC_OWN = L.DM_LST_ACC_OWN,
									RM03.NF_ONL_PAY_DTS = L.NF_ONL_PAY_DTS,
									RM03.NF_IPA = L.NF_IPA,
									RM03.NF_IPH = L.NF_IPH,
									RM03.BC_PAY_OPT = L.BC_PAY_OPT,
									RM03.NF_SBM_CPS_DTS = L.NF_SBM_CPS_DTS,
									RM03.DF_PAY_ACH = L.DF_PAY_ACH,
									RM03.NF_LST_USR_RM03 = L.NF_LST_USR_RM03,
									RM03.PC_STA = L.PC_STA,
									RM03.PD_STA_LST_CHG = L.PD_STA_LST_CHG,
									RM03.PI_BCH_EDT_FAL = L.PI_BCH_EDT_FAL,
									RM03.NC_PAY_PRC = L.NC_PAY_PRC,
									RM03.PI_PRE_UPL_ACC_INF = L.PI_PRE_UPL_ACC_INF,
									RM03.LI_NTF_ACC_CHG = L.LI_NTF_ACC_CHG,
									RM03.NF_USR_CRT_ONL_PAY = L.NF_USR_CRT_ONL_PAY,
									RM03.LC_NTF_ACC_CHG_REA = L.LC_NTF_ACC_CHG_REA,
									RM03.PI_RQR_CNF_PHN_PAY = L.PI_RQR_CNF_PHN_PAY,
									RM03.PC_OPS_FAT_TYP = L.PC_OPS_FAT_TYP,
									RM03.PC_OPS_FAT_SUB_TYP = L.PC_OPS_FAT_SUB_TYP,
									RM03.DF_OPS_PRS_PAY_SSN = L.DF_OPS_PRS_PAY_SSN,
									RM03.DC_OPS_PRS_PAY_TYP = L.DC_OPS_PRS_PAY_TYP
                WHEN NOT MATCHED THEN
                                INSERT 
                                (
									PF_SVC_CLI,
									PF_IRL_GNR_ID,
									BF_SSN,
									BA_PAY,
									LD_PAY,
									DF_PAY_ABA,
									DF_BNK_ACC,
									DF_ACC_TYP,
									DF_EML,
									BN_1,
									BN_MI,
									BN_LST,
									DM_1_ACC_OWN,
									DM_MI_ACC_OWN,
									DM_LST_ACC_OWN,
									NF_ONL_PAY_DTS,
									NF_IPA,
									NF_IPH,
									BC_PAY_OPT,
									NF_SBM_CPS_DTS,
									DF_PAY_ACH,
									NF_LST_USR_RM03,
									PC_STA,
									PD_STA_LST_CHG,
									PI_BCH_EDT_FAL,
									NC_PAY_PRC,
									PI_PRE_UPL_ACC_INF,
									LI_NTF_ACC_CHG,
									NF_USR_CRT_ONL_PAY,
									LC_NTF_ACC_CHG_REA,
									PI_RQR_CNF_PHN_PAY,
									PC_OPS_FAT_TYP,
									PC_OPS_FAT_SUB_TYP,
									DF_OPS_PRS_PAY_SSN,
									DC_OPS_PRS_PAY_TYP

                                )
                                VALUES 
                                (
									L.PF_SVC_CLI,
									L.PF_IRL_GNR_ID,
									L.BF_SSN,
									L.BA_PAY,
									L.LD_PAY,
                                    ENCRYPTBYKEY(Key_GUID(''USHE_Financial_Data_Key''), L.DF_PAY_ABA),
                                    ENCRYPTBYKEY(Key_GUID(''USHE_Financial_Data_Key''), L.DF_BNK_ACC),
									L.DF_ACC_TYP,
									L.DF_EML,
									L.BN_1,
									L.BN_MI,
									L.BN_LST,
									L.DM_1_ACC_OWN,
									L.DM_MI_ACC_OWN,
									L.DM_LST_ACC_OWN,
									L.NF_ONL_PAY_DTS,
									L.NF_IPA,
									L.NF_IPH,
									L.BC_PAY_OPT,
									L.NF_SBM_CPS_DTS,
									L.DF_PAY_ACH,
									L.NF_LST_USR_RM03,
									L.PC_STA,
									L.PD_STA_LST_CHG,
									L.PI_BCH_EDT_FAL,
									L.NC_PAY_PRC,
									L.PI_PRE_UPL_ACC_INF,
									L.LI_NTF_ACC_CHG,
									L.NF_USR_CRT_ONL_PAY,
									L.LC_NTF_ACC_CHG_REA,
									L.PI_RQR_CNF_PHN_PAY,
									L.PC_OPS_FAT_TYP,
									L.PC_OPS_FAT_SUB_TYP,
									L.DF_OPS_PRS_PAY_SSN,
									L.DC_OPS_PRS_PAY_TYP

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
				WEBFLS1.RM03_ONL_PAY RM03
		'               
    ) R
    FULL OUTER JOIN
    (
		SELECT
			COUNT(*) [LocalCount]
		FROM
			CDW..RM03_ONL_PAY RM03
    ) L 
		ON 1 = 1
                
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('RM03_ONL_PAY - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is requireL.', 16, 11, @CountDifference)
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
									RM03.BF_SSN,
									COUNT(*) AS "RemoteCount"
					FROM
									WEBFLS1.RM03_ONL_PAY RM03
					GROUP BY
									RM03.BF_SSN
				'               
			) R
			FULL OUTER JOIN
			(
				SELECT
					RM03.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					CDW..RM03_ONL_PAY RM03
				GROUP BY
					RM03.BF_SSN
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

		SELECT  @SSNs = LEFT(@SSNs, LEN(@SSNs) -1)

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local RM03 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			RM03
		FROM
			CDW..RM03_ONL_PAY RM03
			INNER JOIN @SSN_LIST SL 
				ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = RM03.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END
