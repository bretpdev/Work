DECLARE 
       @StartDate DATE = 'XXXX-X-X',
       @EndDate DATE = 'XXXX-X-XX'

DECLARE @BorrowerCount FLOAT = 
(
	SELECT 
		COUNT(DISTINCT B.account_number) 
	FROM 
		Income_Driven_Repayment..Applications A 
		INNER JOIN Income_Driven_Repayment..Loans L ON A.application_id = L.application_id
		INNER JOIN Income_Driven_Repayment..Borrowers B ON B.borrower_id = L.borrower_id
	WHERE 
		A.created_at BETWEEN @StartDate AND @EndDate
)


--X.   What % of borrowers apply for IDR online? 
SELECT
    'Question X' [Question],
    COUNT(DISTINCT B.account_number) / @BorrowerCount * XXX [%]
FROM 
    Income_Driven_Repayment..Applications A
    INNER JOIN Income_Driven_Repayment..Loans L ON A.application_id = L.application_id
    INNER JOIN Income_Driven_Repayment..Borrowers B ON B.borrower_id = L.borrower_id
WHERE
    A.application_source_id = X
    AND A.created_at BETWEEN @StartDate AND @EndDate


--X.   What % of borrower�s recertify IDR plans online? 
SELECT
    'Question X' [Question],
    COUNT(DISTINCT B.account_number) / @BorrowerCount * XXX [%]
FROM 
    Income_Driven_Repayment..Applications A
    INNER JOIN Income_Driven_Repayment..Loans L ON A.application_id = L.application_id
    INNER JOIN Income_Driven_Repayment..Borrowers B ON B.borrower_id = L.borrower_id
WHERE
    A.repayment_plan_reason_id = X
    AND A.application_source_id = X
    AND A.created_at BETWEEN @StartDate AND @EndDate


--X.   What % of borrowers use the IRS DRT? 
SELECT
    'Question X' [Question],
    COUNT(DISTINCT B.account_number) / @BorrowerCount * XXX [%]
FROM 
    Income_Driven_Repayment..Applications A
    INNER JOIN Income_Driven_Repayment..Loans L ON A.application_id = L.application_id
    INNER JOIN Income_Driven_Repayment..Borrowers B ON B.borrower_id = L.borrower_id
WHERE
    A.application_source_id = X
    AND A.taxes_filed_flag = X
    AND A.tax_year IS NOT NULL
    AND A.created_at BETWEEN @StartDate AND @EndDate


--X.   What % of borrowers submit a paper copy of their Tax Return? 
SELECT
    'Question X' [Question],
    COUNT(DISTINCT B.account_number) / @BorrowerCount * XXX [%]
FROM 
    Income_Driven_Repayment..Applications A
    INNER JOIN Income_Driven_Repayment..Loans L ON A.application_id = L.application_id
    INNER JOIN Income_Driven_Repayment..Borrowers B ON B.borrower_id = L.borrower_id
    INNER JOIN CDW..AYXX_BR_LON_ATY AYXX ON B.SSN = AYXX.BF_SSN
WHERE 
    A.agi_reflects_current_income = X
    AND A.taxes_filed_flag = X
    AND AYXX.PF_REQ_ACT = 'DITAX'
    AND AYXX.LD_ATY_RSP BETWEEN @StartDate AND @EndDate
    AND A.created_at BETWEEN @StartDate AND @EndDate


--X.   What % of borrowers provide ADOI? 
SELECT
    'Question X' [Question],
    COUNT(DISTINCT B.account_number) / @BorrowerCount * XXX [%]
FROM 
    Income_Driven_Repayment..Applications A
    INNER JOIN Income_Driven_Repayment..Loans L ON A.application_id = L.application_id
    INNER JOIN Income_Driven_Repayment..Borrowers B ON B.borrower_id = L.borrower_id
	INNER JOIN CDW..AYXX_BR_LON_ATY AYXX ON B.SSN = AYXX.BF_SSN
WHERE
    A.agi_reflects_current_income = X
	AND AYXX.PF_REQ_ACT IN('DIINC','DIBNK')
	AND AYXX.LD_ATY_RSP BETWEEN @StartDate AND @EndDate
    AND A.created_at BETWEEN @StartDate AND @EndDate


--X.   What % of borrowers certify they have $X.XX or non-taxable income whey they apply or recertify?
SELECT 
    'Question X' [Question],
    COUNT(DISTINCT B.account_number) / @BorrowerCount * XXX [%]
FROM 
    Income_Driven_Repayment..Applications A
    INNER JOIN Income_Driven_Repayment..Loans L ON A.application_id = L.application_id
    INNER JOIN Income_Driven_Repayment..Borrowers B ON B.borrower_id = L.borrower_id
WHERE
    A.adjusted_grose_income = X.XX
    AND A.total_income = X.XX
	AND A.repayment_plan_status_id = X
    AND A.created_at BETWEEN @StartDate AND @EndDate


--X.   Would you say that having $X.XX or non-taxable income is more common on some plans than others? If so, which plans and why?
SELECT
    'Question X' [Question],
    COUNT(DISTINCT B.account_number) [BorrowerCount],
	RT.repayment_type_status
FROM 
    Income_Driven_Repayment..Applications A
    INNER JOIN Income_Driven_Repayment..Loans L ON A.application_id = L.application_id
    INNER JOIN Income_Driven_Repayment..Borrowers B ON B.borrower_id = L.borrower_id
	INNER JOIN Income_Driven_Repayment..Repayment_Plan_Selected RP ON RP.application_id = A.application_id
	INNER JOIN Income_Driven_Repayment..Repayment_Type RT ON RT.repayment_type_id = RP.repayment_type_id
WHERE
    A.adjusted_grose_income = X.XX
    AND A.total_income = X.XX
	AND A.repayment_plan_status_id = X
    AND A.created_at BETWEEN @StartDate AND @EndDate
GROUP BY
    RT.repayment_type_status
