SELECT * FROM OPENQUERY (DUSTER,'SELECT * FROM OLWHRM1.PD10_PRS_NME WHERE DF_SPE_ACC_ID = ''5192556927''');
SELECT * FROM OPENQUERY (DUSTER,'
	SELECT DISTINCT 
			LN10.BF_SSN				AS SSN
			,LN10.LN_SEQ			AS LOAN
			,LN10.LF_LON_CUR_OWN	AS OWNER
			,LN10.LD_LON_EFF_ADD	AS PCV_DECON
			,LN10.LD_LON_ACL_ADD	AS CONVERTED
			,LN72.LD_ITR_EFF_BEG
			,LN83.LD_EFT_EFF_BEG
			,RP30.PR_EFT_RIR		AS ACH_RIR
			,LN72.LR_ITR
	FROM 
		OLWHRM1.LN10_LON LN10
		INNER JOIN OLWHRM1.PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN OLWHRM1.RP30_EFT_RIR_PAR RP30
			ON RP30.IF_OWN = LN10.LF_LON_CUR_OWN
		INNER JOIN OLWHRM1.LN83_EFT_TO_LON LN83
			ON LN10.BF_SSN = LN83.BF_SSN
			AND LN10.LN_SEQ = LN83.LN_SEQ
	INNER JOIN (
					SELECT DISTINCT
						A.BF_SSN
						,A.LN_SEQ
						,A.LR_ITR
						,B.LD_ITR_EFF_BEG
					FROM
						OLWHRM1.LN72_INT_RTE_HST A
					INNER JOIN 
						(
							SELECT DISTINCT
								BF_SSN
								,LN_SEQ
								,MAX(LD_ITR_EFF_BEG) AS LD_ITR_EFF_BEG
							FROM
								OLWHRM1.LN72_INT_RTE_HST
							WHERE
								LC_STA_LON72 = ''A''
							GROUP BY 
								BF_SSN
								,LN_SEQ
						)B
						ON A.BF_SSN = B.BF_SSN
						AND A.LN_SEQ = B.LN_SEQ
						AND A.LD_ITR_EFF_BEG = B.LD_ITR_EFF_BEG
					WHERE
						LC_STA_LON72 = ''A''
						AND A.BF_SSN=''001502868''
					)LN72
					ON LN10.BF_SSN = LN72.BF_SSN
					AND LN10.LN_SEQ = LN72.LN_SEQ
	WHERE
		LN10.LF_LON_CUR_OWN LIKE ''8297690%''
		AND LN83.LD_EFT_EFF_BEG >= LN10.LD_LON_ACL_ADD
		AND RP30.PC_EFT_RIR_STA = ''A''
		AND LN72.LD_ITR_EFF_BEG = LN83.LD_EFT_EFF_BEG -- < vs <=
');