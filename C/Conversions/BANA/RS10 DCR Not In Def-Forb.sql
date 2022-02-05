use EA27_BANA
go

SELECT DISTINCT
	rs10.BF_SSN,
    rs10.LN_RPS_SEQ,
	CONVERT(VARCHAR,p.NextPaymentDueDate,101)AS NextPayDue,
	CONVERT(VARCHAR,RS10.LD_RPS_1_PAY_DU,101)AS LD_RPS_1_PAY_DU,
	b.LoanStatusCode
  FROM RS10
  inner join LN65
	on ln65.BF_SSN = rs10.BF_SSN
	and ln65.LN_RPS_SEQ = rs10.LN_RPS_SEQ
inner join CompassLoanMapping map
	on map.BorrowerSsn = ln65.BF_SSN
	and map.LN_SEQ = ln65.LN_SEQ
inner join _03PaymentDataRecord p
	on p.borrowerssn = map.borrowerssn
	and p.loan_number = map.loan_number
inner join _01BorrowerRecord b
	on b.borrowerssn = p.BorrowerSSN
	and b.loan_number = p.loan_number
where 
	b.LoanStatusCode in ('30','33')
		