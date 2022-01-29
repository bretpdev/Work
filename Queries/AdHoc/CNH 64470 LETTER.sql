select 
	pdXX.DF_SPE_ACC_ID,
	ld_aty_req_rcv as letter_sent_date
from 
	udw..AYXX_BR_LON_ATY ayXX 
inner join udw..pdXX_prs_nme pdXX 
	on pdXX.df_prs_id = ayXX.bf_ssn 
where 
	PF_REQ_ACT = 'DLXXX' 
	and LD_ATY_REQ_RCV < cast('XXXX-XX-XX XX:XX:XX.XXX' as date) 
order by 
	ld_aty_req_rcv desc