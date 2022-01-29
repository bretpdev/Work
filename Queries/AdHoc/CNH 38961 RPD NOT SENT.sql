set transaction isolation level read uncommitted
select
	*
from
	(
select distinct
	dwXX.bf_ssn,
	dwXX.ln_seq,
	lnXX.IC_LON_PGM,
	lc.lc,
	dwXX.WC_DW_LON_STA,
	LNXX.LD_LON_ACL_ADD,
	count(*) over(partition by dwXX.bf_ssn) as plc
	--count(distinct dwXX.bf_ssn) as c
from
	cdw..DWXX_DW_CLC_CLU dwXX
	inner join cdw..LNXX_LON lnXX
		on lnXX.BF_SSN = dwXX.bf_ssn
		and lnXX.LN_SEQ = dwXX.LN_SEQ
	--inner join cdw..RSXX_BR_RPD rsXX
	--	on rsXX.BF_SSN = dwXX.BF_SSN
	--	and rsXX.LN_RPS_SEQ = X
	--inner join cdw..LNXX_LON_RPS lnXX
	--	on lnXX.BF_SSN = dwXX.BF_SSN
	--	and lnXX.ln_seq = dwXX.ln_seq
	--	and rsXX.LN_RPS_SEQ = lnXX.LN_RPS_SEQ
	--	and lnXX.LN_RPS_SEQ = X
	inner join cdw..PDXX_PRS_NME pdXX
		on pdXX.DF_PRS_ID = dwXX.BF_SSN
	inner join
	(
		select
			bf_ssn,
			count(*) as lc
		from
			cdw..LNXX_LON lnXX
		where
			LNXX.LD_LON_ACL_ADD > 'XX/XX/XXXX'
			and lnXX.LA_CUR_PRI > X
			and lnXX.LC_STA_LONXX = 'r'
		group by
			bf_ssn
	)lc
		on lc.BF_SSN = dwXX.BF_SSN
	left join cdw..AYXX_BR_LON_ATY ayXX
		on ayXX.BF_SSN = dwXX.BF_SSN
		and ayXX.PF_REQ_ACT = 'MSRPD'
	left join cls.[print].PrintProcessing pp
		on pp.AccountNumber = pdXX.DF_SPE_ACC_ID
		and pp.ScriptDataId in (X,X)

where
	(
		dwXX.WC_DW_LON_STA  in ('XX','XX')
		or
		(dwXX.WC_DW_LON_STA = 'XX' and lNXX.IC_LON_PGM not IN ('DLPLGB', 'DLPLUS', 'PLUS', 'PLUSGB') )
	)
	
	and ayXX.BF_SSN is null
	and pp.AccountNumber is null
	AND LNXX.LD_LON_ACL_ADD > 'XX/XX/XXXX'
	and lnXX.LA_CUR_PRI > X
	and lnXX.LC_STA_LONXX = 'r'

--order by 
--	dwXX.bf_ssn,
--	dwXX.ln_seq
	) p
--where p.lc != p.plc