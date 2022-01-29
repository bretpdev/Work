INSERT INTO CDW..CS_Transfer_Exclusions
SELECT DISTINCT
	P.BF_SSN,
	P.S
FROM
(
select distinct bf_ssn, 'BRENDA TICKET XXXXX BATCH XX/XX/XXXXSXXX' AS S  from cdw..RMXX_BR_RMT where ld_rmt_bch_ini = 'XX/XX/XXXX' and LC_RMT_BCH_SRC_IPT = 's' and LN_RMT_BCH_SEQ in (X)
union
select distinct bf_ssn, 'BRENDA TICKET XXXXX BATCH XX/XX/XXXXSXXX' AS S  from cdw..RMXX_BR_RMT where ld_rmt_bch_ini = 'XX/XX/XXXX' and LC_RMT_BCH_SRC_IPT = 's' and LN_RMT_BCH_SEQ in (X)
UNION
select distinct bf_ssn, 'BRENDA TICKET XXXXX BATCH XX/XX/XXXXSXXX' AS S   from cdw..RMXX_BR_RMT where ld_rmt_bch_ini = 'XX/XX/XXXX' and LC_RMT_BCH_SRC_IPT = 's' and LN_RMT_BCH_SEQ in (X)
union
select distinct bf_ssn, 'BRENDA TICKET XXXXX BATCH XX/XX/XXXXSXXX' AS S  from cdw..RMXX_BR_RMT where ld_rmt_bch_ini = 'XX/XX/XXXX' and LC_RMT_BCH_SRC_IPT = 's' and LN_RMT_BCH_SEQ in (X)
UNION
select distinct bf_ssn, 'BRENDA TICKET XXXXX BATCH XX/XX/XXXXSXXX' AS S from cdw..RMXX_BR_RMT where ld_rmt_bch_ini = 'XX/XX/XXXX' and LC_RMT_BCH_SRC_IPT = 's' and LN_RMT_BCH_SEQ in (X)
union
select distinct bf_ssn, 'BRENDA TICKET XXXXX BATCH XX/XX/XXXXSXXX' AS S from cdw..RMXX_BR_RMT where ld_rmt_bch_ini = 'XX/XX/XXXX' and LC_RMT_BCH_SRC_IPT = 's' and LN_RMT_BCH_SEQ in (X)
UNION
select distinct bf_ssn, 'BRENDA TICKET XXXXX BATCH XX/XX/XXXXSXXX' AS S from cdw..RMXX_BR_RMT where ld_rmt_bch_ini = 'XX/XX/XXXX' and LC_RMT_BCH_SRC_IPT = 's' and LN_RMT_BCH_SEQ in (X)
) P
LEFT JOIN CDW..CS_Transfer_Exclusions EX
	ON EX.BF_SSN = P.BF_SSN
WHERE
	EX.BF_SSN IS NULL

