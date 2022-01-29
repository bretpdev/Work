
SELECT
	*
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT DISTINCT 
				FSXX.BF_SSN,
				FSXX.LN_SEQ,
				FSXX.LD_LON_ENT_RPYE,
				LNXXB.LC_TYP_SCH_DIS AS "INACTIVATED SCHEDULE TYPE",
				LNXXb.LD_CRT_LONXX AS "DATE SCHEDULE INACTIVATED",
				LNXXA.LC_TYP_SCH_DIS AS "CURRENT SCHEDULE TYPE",
				LNXXa.LD_CRT_LONXX AS "DATE CURRENT SCHEDULE DISCLOSED"
			FROM  
				PKUB.FSXX_DL_LON FSXX   
				INNER JOIN PKUB.LNXX_LON_RPS LNXXa ON LNXXa.BF_SSN = FSXX.BF_SSN AND LNXXa.LN_SEQ = FSXX.LN_SEQ
				RIGHT OUTER JOIN PKUB.LNXX_LON_RPS LNXXb ON LNXXa.BF_SSN = LNXXb.BF_SSN AND LNXXa.LN_SEQ=LNXXb.LN_SEQ AND LNXXa.LC_STA_LONXX = ''A''
				INNER JOIN PKUB.LNXX_LON LNXX ON LNXXA.BF_SSN = LNXX.BF_SSN AND LNXXA.LN_SEQ = LNXX.LN_SEQ
			WHERE    
				LNXX.LA_CUR_PRI > X
				AND LNXX.LC_STA_LONXX = ''R''
				AND LNXX.LC_CAM_LON_STA <> ''XX''
				AND LNXXB.LC_RPD_INA_REA = ''G''
				AND LNXXB.LC_TYP_SCH_DIS in (''CX'', ''CA'', ''IX'', ''IB'', ''IX'')
				AND LNXXA.LC_TYP_SCH_DIS NOT IN (''CX'', ''CA'', ''IX'', ''IB'', ''IX'')
				AND LNXXB.LD_CRT_LONXX BETWEEN ''XXXX-XX-XX'' AND ''XXXX-XX-XX''
		'
	)


--Output = 
--FSXX.BF_SSN
--FSXX.LN_SEQ
--FSXX.LD_LON_ENT_RPYE
--For the second query, reapplying the schedule right away will allow COMPASS to maintain the prior schedule information.


--PREVIOUS DAY, REPAYMENT SCHEDULES INACTIVATED BY ADJUSTMENTS
SELECT
	*
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT DISTINCT 
				LNXXb.BF_SSN,
				LNXXB.LN_SEQ,
				LNXXB.LC_TYP_SCH_DIS AS "INACTIVATED SCHEDULE TYPE",
				LNXXb.LD_CRT_LONXX AS "DATE SCHEDULE INACTIVATED",
				LNXXA.LC_TYP_SCH_DIS AS "CURRENT SCHEDULE TYPE",
				LNXXa.LD_CRT_LONXX AS "DATE CURRENT SCHEDULE DISCLOSED"
			FROM     
				PKUB.LNXX_LON_RPS LNXXa
				RIGHT OUTER JOIN PKUB.LNXX_LON_RPS LNXXb ON LNXXa.BF_SSN=LNXXb.BF_SSN AND LNXXa.LN_SEQ=LNXXb.LN_SEQ AND LNXXa.LC_STA_LONXX = ''A''
				INNER JOIN PKUB.LNXX_LON LNXX ON LNXXA.BF_SSN = LNXX.BF_SSN AND LNXXA.LN_SEQ = LNXX.LN_SEQ
			WHERE    
				LNXX.LA_CUR_PRI > X
				AND LNXX.LC_STA_LONXX = ''R''
				AND LNXX.LC_CAM_LON_STA <> ''XX''
				AND LNXXB.LC_RPD_INA_REA = ''G''
				AND LNXXB.LC_TYP_SCH_DIS in (''CX'', ''CA'', ''IX'', ''IB'', ''IX'')
				AND LNXXA.LC_TYP_SCH_DIS NOT IN (''CX'', ''CA'', ''IX'', ''IB'', ''IX'')
				AND LNXXB.LD_CRT_LONXX = (CURRENT_DATE -X DAY)
		'
	)
