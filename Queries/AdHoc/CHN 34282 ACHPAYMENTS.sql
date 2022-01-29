SELECT * FROM OPENQUERY(LEGEND,
'
	SELECT 
		month(LD_CRT_REMT30) as m, 
		year(LD_CRT_REMT30) as y, 
		count(*) 
	FROM 
		PKUB.RM30_BR_RMT 
	where 
		year(LD_CRT_REMT30) >= 2017 
		and 
		LC_RMT_BCH_SRC_IPT = ''E''
	group by 
		month(LD_CRT_REMT30) , 
		year(LD_CRT_REMT30) 
	order by 
		year(LD_CRT_REMT30), 
		month(LD_CRT_REMT30) 
')

--select distinct bf_ssn from cdw..BR30_BR_EFT b where b.BC_EFT_STA = 'a'
