select top 1000 
	* 
from 
	cdw..RM31_BR_RMT_PST  
where 
	 LD_RMT_PST_PST between '03-01-2020' and '04-01-2020'
	--and LC_RMT_BCH_SRC_IPT = 's'
	and LC_RMT_STA_PST = 's'