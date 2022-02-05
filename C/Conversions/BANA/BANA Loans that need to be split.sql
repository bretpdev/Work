use EA27_BANA
go

select distinct
	err.major_batch,
	err.minor_batch,
	d.*
from
	_07_08DisbClaimEnrollRecord d
	left join Borrower_Errors err
		on d.BorrowerSSN = err.br_ssn
inner join
(

select
	BorrowerSSN,
	loan_number,
	count(distinct isnull(CommonLineUniqueID, '!@#')) as c
from
	_07_08DisbClaimEnrollRecord
group by 
	BorrowerSSN,
	loan_number
having 
	count(distinct isnull(CommonLineUniqueID, '!@#')) > 1
) pop
	on pop.BorrowerSSN = d.BorrowerSSN
	and pop.loan_number = d.loan_number

select
	p.*
from
	_07_08DisbClaimEnrollRecord d
	inner join _03PaymentDataRecord p
		on p.BorrowerSSN = d.BorrowerSSN
		and p.loan_number = d.loan_number
inner join
(

select
	BorrowerSSN,
	loan_number,
	count(distinct isnull(CommonLineUniqueID, '!@#')) as c
from
	_07_08DisbClaimEnrollRecord
group by 
	BorrowerSSN,
	loan_number
having 
	count(distinct isnull(CommonLineUniqueID, '!@#')) > 1
) pop
	on pop.BorrowerSSN = d.BorrowerSSN
	and pop.loan_number = d.loan_number



