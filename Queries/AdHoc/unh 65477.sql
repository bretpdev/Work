drop table #data
drop table #final
select 
	AccountNumber, 
	substring(letterdata, 20, 120) as ld,
	scriptdataid as sd,
	 count(*) as cou
into #data
from 
	uls.[print].PrintProcessing 
where 
	--DeletedBy = 'UNH 64895'
	cast(AddedAt as date)  between '12-24-2019' and '12-27-2019'
	--and AccountNumber = '4168066235'
	AND ScriptDataId IN (	6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75)
	
group by
	AccountNumber, 
	substring(letterdata, 20, 120),
	scriptdataid
--having count(*) > 1
order by AccountNumber



select
	AccountNumber,
	sd,
	count(*) as c
into #final
from #data
group by accountnumber, sd
having count(*) > 1
order by accountnumber

select 
	f.AccountNumber, 
	f.sd,
	--substring(letterdata, 20, 120) as ld,
	 count(*) as cou

from 
	uls.[print].PrintProcessing pp
	inner join #final f
		on pp.AccountNumber = f.AccountNumber
where 
	DeletedBy = 'UNH 64895'
	
	and cast(AddedAt as date)  between '12-24-2019' and '12-27-2019'
	--and f.AccountNumber = '4168066235'
	AND ScriptDataId IN (	6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75)
group by 
	f.AccountNumber, sd
	--substring(letterdata, 20, 120)
having count(*) > 1