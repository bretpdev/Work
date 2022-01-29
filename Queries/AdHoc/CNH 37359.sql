USE CDW
GO


SELECT DISTINCT
	RS.BF_SSN
FROM
	calc.RepaymentSchedules RS
	INNER JOIN CDW..BRXX_BR_EFT BRXX
		ON BRXX.BF_SSN = RS.BF_SSN
WHERE 
	LC_TYP_SCH_DIS IN ('CA', 'CP', 'CQ', 'CX', 'CX', 'CX', 'IA', 'IB', 'IL', 'IP', 'IS', 'IX', 'IX')
	AND CurrentGradation = X
	AND BRXX.BC_EFT_STA = 'A'
GROUP BY 
	RS.BF_SSN
HAVING 
	SUM(LA_RPS_ISL) = X.XX