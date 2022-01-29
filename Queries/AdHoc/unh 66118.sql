select distinct
	Script,
	id,
	cast(Description as varchar(max)) as [Description]
from 
	BSYS..SCKR_DAT_Scripts s
	inner join [BSYS].[dbo].[SCKR_REF_Unit] u
		on s.Script = u.Program
where
	[Status] = 'Active'
	and u.Unit in 
	(
		'LGP Accounting',
		'Loan Management',
		'Claims Review'
	)

select distinct
	Job,
	id,
	cast(Description as varchar(max)) as [Description]
	
from 
	BSYS..SCKR_DAT_SAS s
	inner join [BSYS].[dbo].[SCKR_REF_UnitSAS] u
		on s.Job = u.Program
where
	[Status] = 'Active'
	and u.Unit in 
	(
		'LGP Accounting',
		'Loan Management',
		'Claims Review'
	)