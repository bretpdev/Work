declare @EOM date = 'XX/XX/XXXX'

select
	a.bf_ssn,
	a.ln_seq,
	convert(varchar, a.LD_RPT_CRB, XXX) as LD_RPT_CRB,
	forb.FORB_AT_EOM
from
	cdw.[dbo].[cnh XXXXX june eom] a
	left join
	(
		SELECT distinct
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			case when LNXX.LD_FOR_BEG <= @EOM then FBXX.LC_FOR_TYP END AS FORB_AT_EOM,
			row_number() over(partition by lnXX.bf_ssn, lnXX.ln_seq order by lnXX.LF_FOR_CTL_NUM desc) as rn

		FROM
			CDW..FBXX_BR_FOR_REQ FBXX
			INNER JOIN CDW..LNXX_BR_FOR_APV LNXX
				ON LNXX.BF_SSN = FBXX.BF_SSN
				AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
	
		WHERE
	
			FBXX.LC_FOR_STA = 'A' --denied records cant have this active
			AND LNXX.LC_FOR_RSP != 'XXX'
			AND @EOM BETWEEN CAST(LNXX.LD_FOR_BEG AS DATE) AND CAST(LNXX.LD_FOR_END AS DATE)
		) forb
			on forb.BF_SSN = a.bf_ssn
			and forb.ln_seq = a.ln_seq
			and forb.rn = X
order by
	bf_ssn,
	ln_seq