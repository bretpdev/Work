select distinct 
	PDXX.DF_SPE_ACC_ID,
	RS.LN_SEQ,
	RS.LC_TYP_SCH_DIS,
	RS.LN_RPS_TRM
from 
	cdw.calc.RepaymentSchedules RS
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = RS.BF_SSN
	INNER JOIN CDW..LKXX_LS_CDE_LKP LKXX
		ON LKXX.PM_ATR = 'LC-TYP-SCH-DIS'
		AND LKXX.PX_ATR_VAL = LC_TYP_SCH_DIS
where
	rs.LN_GRD_RPS_SEQ = X
	and rs.LC_TYP_SCH_DIS in 
	(
		'IX',
		'CA',
		'IB',
		'IX',
		'CX',
		'CX',
		'CX'
	)
	and rs.LN_RPS_TRM > XX
	and rs.LA_CUR_PRI > X.XX