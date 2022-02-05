DECLARE @TODAY DATE = GETDATE();

SELECT
	LD_RMT_BCH_INI, --date batch initiated
	LC_RMT_BCH_SRC_IPT, --batch source code
	LN_RMT_BCH_SEQ, --batch seq number
	LC_RMT_BCH, --batch type
	LC_RMT_BCH_STA, --batch status
	LC_BCH_TRX_TYP, --transaction type
	LN_RMT_BCH_ITM_TOT, --total number of batch items
	LA_RMT_BCH_TOT, --amount of batch
	LF_RMT_BCH_USR_ASN, --user ID assigned to batch
	LD_RMT_BCH_PAY_EFF --batch payment effective date
FROM
	CDW..RM10_RMT_BCH
WHERE 
	LC_RMT_BCH_STA != 'A' --specifically looking for inactive flag, not active
	AND	(
			(
				LF_RMT_BCH_USR_ASN != 'CANQUE'
				AND (
						LD_RMT_BCH_PAY_EFF < @TODAY
						OR LD_RMT_BCH_PAY_EFF IS NULL
					)
			)
			OR
			( 
				LF_RMT_BCH_USR_ASN = 'CANQUE'
				AND (
						LD_RMT_BCH_PAY_EFF < DATEADD(DAY, -3, @TODAY)
						OR LD_RMT_BCH_PAY_EFF IS NULL
					)
			)
		)
;