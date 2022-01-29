select distinct
	lnXX.bf_ssn,
	FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(X)),X) AS AWARD_ID,
	'XX/XX/XXXX' as TRANSFER_DATE,
	max(lnXX.LD_NXT_PAY_DUE_AHD) as LD_NXT_PAY_DUE_AHD
from 
	cdw..LNXX_LON_BIL_CRF lnXX
	inner join cdw..CS_Transfer_EAXX eaXX
		on eaXX.BF_SSN = lnXX.bf_ssn
		and eaXX.TransferNumber = X
	INNER JOIN CDW..FSXX_DL_LON FSXX
		ON FSXX.BF_SSN = lnXX.BF_SSN
		AND FSXX.LN_SEQ = lnXX.LN_SEQ
where 
	LD_LST_DTS_LNXX > 'XX/XX/XXXX' 
	AND LD_NXT_PAY_DUE_AHD > 'XX/XX/XXXX'

group by
	lnXX.bf_ssn,
	FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(X)),X)

union

select distinct
lnXX.bf_ssn,
FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(X)),X) AS AWARD_ID,
'XX/XX/XXXX' as TRANSFER_DATE,
	max(lnXX.LD_NXT_PAY_DUE_AHD) as LD_BIL_DU_LON
from 
	cdw..LNXX_LON_BIL_CRF lnXX
	inner join cdw..CS_Transfer_EAXX eaXX
		on eaXX.BF_SSN = lnXX.bf_ssn
		and eaXX.TransferNumber = X
	INNER JOIN CDW..FSXX_DL_LON FSXX
		ON FSXX.BF_SSN = lnXX.BF_SSN
		AND FSXX.LN_SEQ = lnXX.LN_SEQ
where 
	LD_LST_DTS_LNXX > 'XX/XX/XXXX' 
	AND LD_NXT_PAY_DUE_AHD > 'XX/XX/XXXX'
group by
	lnXX.bf_ssn,
	FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(X)),X)

union

select distinct
	lnXX.bf_ssn,
	FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(X)),X) AS AWARD_ID,
	'XX/XX/XXXX' as TRANSFER_DATE,
	max(lnXX.LD_NXT_PAY_DUE_AHD) as LD_BIL_DU_LON
from 
	cdw..LNXX_LON_BIL_CRF lnXX
	inner join cdw..CS_Transfer_EAXX eaXX
		on eaXX.BF_SSN = lnXX.bf_ssn
		and eaXX.TransferNumber = X
	INNER JOIN CDW..FSXX_DL_LON FSXX
		ON FSXX.BF_SSN = lnXX.BF_SSN
		AND FSXX.LN_SEQ = lnXX.LN_SEQ
where 
	LD_LST_DTS_LNXX > 'XX/XX/XXXX' 
	AND LD_NXT_PAY_DUE_AHD > 'XX/XX/XXXX'
group by
	lnXX.bf_ssn,
	FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(X)),X)

union

select distinct
	lnXX.bf_ssn,
	FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(X)),X) AS AWARD_ID,
	'XX/XX/XXXX' as TRANSFER_DATE,
	max(lnXX.LD_NXT_PAY_DUE_AHD) as LD_BIL_DU_LON
from 
	cdw..LNXX_LON_BIL_CRF lnXX
	inner join cdw..CS_Transfer_EAXX eaXX
		on eaXX.BF_SSN = lnXX.bf_ssn
		and eaXX.TransferNumber = X
	INNER JOIN CDW..FSXX_DL_LON FSXX
		ON FSXX.BF_SSN = lnXX.BF_SSN
		AND FSXX.LN_SEQ = lnXX.LN_SEQ
where 
	LD_LST_DTS_LNXX > 'XX/XX/XXXX' 
	AND LD_NXT_PAY_DUE_AHD > 'XX/XX/XXXX'
group by
	lnXX.bf_ssn,
	FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(X)),X)

--XX/XX-X
--XX/XX-X
--XX/XX-X
--XX/XX-X