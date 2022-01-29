select 
	
	Month(RMXX.LD_RMT_BCH_INI) as [month],
	YEAR(RMXX.LD_RMT_BCH_INI) as [year],
	LF_USR_UPD_SPS,
	COUNT(*)
from 
	CDW..RMXX_BR_RMT_PST RMXX
where 
	RMXX.LD_RMT_BCH_INI between 'XX/XX/XXXX' and 'XX/XX/XXXX' 
	AND LF_USR_UPD_SPS != ''
GROUP BY
	LF_USR_UPD_SPS,
	Month(RMXX.LD_RMT_BCH_INI),
	YEAR(RMXX.LD_RMT_BCH_INI) 
order by
	YEAR(RMXX.LD_RMT_BCH_INI),
	Month(RMXX.LD_RMT_BCH_INI),
	LF_USR_UPD_SPS

select 
*
from 
	CDW..RMXX_BR_RMT_PST RMXX
where 
	RMXX.LD_RMT_BCH_INI between 'XX/XX/XXXX' and 'XX/XX/XXXX' 
	AND LF_USR_UPD_SPS != ''
