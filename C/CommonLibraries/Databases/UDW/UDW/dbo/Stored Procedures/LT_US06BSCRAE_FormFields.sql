﻿
CREATE PROCEDURE [dbo].[LT_US06BSCRAE_FormFields]
	@BF_SSN CHAR(9)
AS

DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = 'US06BSCRAE')


SELECT
	CONVERT(VARCHAR(10), MIN(LN72.LD_ITR_EFF_BEG),101) AS EftDate
FROM
	UDW..LN72_INT_RTE_HST LN72
		INNER JOIN
	(
		SELECT

			LN85.BF_SSN,
			LN85.LN_SEQ
		FROM
			UDW..LN85_LON_ATY LN85
			INNER JOIN --GETS THE MOST RECENT ARC LEFT ON THE BORROWERS ACCOUNT TO GET THE LOAN SEQ'S THE LETTER APPLIES TO
			(
				SELECT
					AY10.BF_SSN,
					MAX(AY10.LN_ATY_SEQ) AS LN_ATY_SEQ,
					MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
				FROM 
					UDW..AY10_BR_LON_ATY AY10
				WHERE
					AY10.PF_REQ_ACT = @PF_REQ_ACT
				GROUP BY
					AY10.BF_SSN
			)AY10
				ON AY10.BF_SSN = LN85.BF_SSN
				AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
	)LN85
		ON LN85.BF_SSN = LN72.BF_SSN
		AND LN85.LN_SEQ = LN72.LN_SEQ
WHERE
	LD_ITR_EFF_BEG > GETDATE() 
	AND LC_STA_LON72 = 'A'
	AND LC_INT_RDC_PGM != 'M'
	AND LN72.BF_SSN = @BF_SSN






RETURN 0