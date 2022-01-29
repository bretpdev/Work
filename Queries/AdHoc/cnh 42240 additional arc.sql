select distinct
	ln10.bf_ssn
from
	cdw..[CNH42240] cnh
	inner join cdw..ln10_lon ln10
		on ln10.bf_ssn = cnh.[9 digit ssn]
	left join cdw..AY10_BR_LON_ATY ay10
		on ay10.bf_ssn = cnh.[9 digit ssn]
		and ay10.pf_req_act = 'RCBDF'
where ln10.la_cur_pri > 0 and ay10.bf_ssn is null