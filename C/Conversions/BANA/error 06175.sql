use EA27_BANA
go

select
	S.BorrowerSSN,
    S.HomePhoneNumber,
	S.IBRSubsidyBeginDate
from 
	Borrower_Errors err
	inner join CompassLoanMapping map
		on map.BorrowerSsn = err.br_ssn
		and map.LN_SEQ = err.ln_seq
	inner join _05SupplementalBorrowerRecord s
		on s.BorrowerSSN = map.BorrowerSsn
		and s.loan_number = map.loan_number
where 
	error_code = '06175'