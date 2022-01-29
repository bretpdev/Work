use cdw

go

select
	BF_SSN,
	LN_SEQ
from
	CDW..LNXX_LON lnXX
where
	LNXX.LF_DOE_SCL_ORG like 'XXXXXX%'