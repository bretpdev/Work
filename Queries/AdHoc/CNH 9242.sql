use ECorrFed
go

select distinct
	count(*)
from
	DocumentDetails dd
where 
	LetterId = XXXX
	and CreateDate between 'XX/XX/XXXX' and 'XX/XX/XXXX'
	and CorrMethod = 'Printed'

select distinct
	count(*)
from
	DocumentDetails dd
where 
	LetterId = XXXX
	and CreateDate between 'XX/XX/XXXX' and 'XX/XX/XXXX'
	and CorrMethod = 'Email Notify'