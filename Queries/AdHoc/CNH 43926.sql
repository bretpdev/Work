
SELECT * FROM
(
SELECT
	rmXX.bf_ssn,
	SUM(rmXX.la_br_rmt) OVER(PARTITION BY RMXX.BF_SSN)AS la_br_rmt,
	ISNULL(abs(p.amount),X) as AMOUNT_FROM_FILE
	--rmXX.*
FROM
	CDW..RMXX_BR_RMT rmXX
	LEFT join
	(
		select SUBSTRING(ssn,X,X) as ssn, sum(amount) amount from cdw..cnhXXXXXPOSTING group by SUBSTRING(ssn,X,X)
	) p
		on p.ssn = rmXX.bf_ssn
WHERE
	LD_RMT_BCH_INI = 'XX/XX/XXXX'
	AND LC_RMT_BCH_SRC_IPT = 'W'
	AND LN_RMT_BCH_SEQ = X
	and lc_rmt_sta = 'A'
) P
WHERE  cast(abs(p.AMOUNT_FROM_FILE) as decimal(XX,X)) != cast(P.la_br_rmt as decimal(XX,X))
select * from 
(
SELECT
	RMXX.bf_ssn,
	sum(RMXX.LA_BR_RMT_PST) over (partition by rmXX.bf_ssn) as LA_BR_RMT_PST,
	ISNULL(abs(p.amount),X) as amount
	--RMXX.*
FROM
	CDW..RMXX_BR_RMT_PST RMXX
	LEFT join
	(
		select SUBSTRING(ssn,X,X) as ssn, sum(amount) amount from cdw..CNHXXXXXSUSPENSE group by SUBSTRING(ssn,X,X)
	) p
		on p.ssn = RMXX.bf_ssn
WHERE
	LD_RMT_BCH_INI = 'XX/XX/XXXX'
	AND LC_RMT_BCH_SRC_IPT = 'W'
	AND LN_RMT_BCH_SEQ = X
	

	and LC_RMT_STA_PST not in ('I','c')
) p
WHERE
	cast(abs(p.amount) as decimal(XX,X)) != cast(P.LA_BR_RMT_PST as decimal(XX,X))
	--XXXX
	--XXXX

