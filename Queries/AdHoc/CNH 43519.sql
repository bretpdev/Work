set transaction isolation level read uncommitted 
		SELECT DISTINCT
			LNXX.BF_SSN,
			WQXX.PF_REQ_ACT,
			wqXX.WX_MSG_X_TSK,
			wqXX.*
			
		FROM
			CDW..LNXX_FIN_ATY LNXX
			inner join cdw..CS_TransferX TX
				ON TX.BF_SSN = LNXX.BF_SSN
			INNER JOIN CDW..LNXX_LON LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LA_CUR_PRI >= X.XX
				AND LNXX.LC_FED_PGM_YR = 'LNC'
				AND LNXX.LD_PIF_RPT IS NULL
			INNER JOIN CDW..WQXX_TSK_QUE WQXX
				ON WQXX.BF_SSN = LNXX.BF_SSN
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND COALESCE(LNXX.LC_FAT_REV_REA,'') = ''
			AND LNXX.PC_FAT_TYP = 'XX'
			AND LNXX.PC_FAT_SUB_TYP = 'XX'
			and wqXX.PF_REQ_ACT in ('CODCA','CODPA')