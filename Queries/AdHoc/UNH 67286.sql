select 
	
	Month(RM31.LD_RMT_BCH_INI) as [month],
	YEAR(RM31.LD_RMT_BCH_INI) as [year],
	LF_USR_UPD_SPS,
	COUNT(*)
from 
	uDW..RM31_BR_RMT_PST RM31
where 
	RM31.LD_RMT_BCH_INI between '09/01/2019' and '02/28/2020' 
	AND LF_USR_UPD_SPS != ''
GROUP BY
	LF_USR_UPD_SPS,
	Month(RM31.LD_RMT_BCH_INI),
	YEAR(RM31.LD_RMT_BCH_INI) 
order by
	YEAR(RM31.LD_RMT_BCH_INI),
	Month(RM31.LD_RMT_BCH_INI),
	LF_USR_UPD_SPS

select 
*
from 
	uDW..RM31_BR_RMT_PST RM31
where 
	RM31.LD_RMT_BCH_INI between '09/01/2019' and '02/28/2020' 
	AND LF_USR_UPD_SPS != ''
