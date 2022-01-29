set transaction isolation level read uncommitted
select distinct
	e.bf_ssn
from
	cdw..[CNH43724] cnh
	inner join cdw..cs_transfer_ea80 e
		on e.bf_ssn = cnh.[9 digit ssn]
	left join cdw..AY10_BR_LON_ATY ay10
		on ay10.bf_ssn = cnh.[9 digit ssn]
		and ay10.pf_req_act = 'RCBDF'
where ay10.bf_ssn is null