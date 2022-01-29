USE CDW
GO

SELECT DISTINCT
	D.Borrower_SSN [Borrower SSN],
	D.Borrower_Name [Borrower Name],
	CASE
		WHEN D.[Status] = X
			THEN 'ALLEGED'
		WHEN D.[Status] = X
			THEN 'VERIFIED'
		WHEN D.[Status] = X
			THEN 'DISPROVED'
		ELSE
			ISNULL(D.[Status], '')
	END [Status]
FROM
	OPENQUERY
		(LEGEND,
			'
				SELECT DISTINCT
					AYXX.BF_SSN AS Borrower_SSN,
					RTRIM(PDXX.DM_PRS_X) || '' '' || PDXX.DM_PRS_LST AS Borrower_Name,
					PDXX.DC_DTH_STA AS Status
				FROM
					PKUB.AYXX_BR_LON_ATY AYXX
					JOIN PKUB.PDXX_PRS_NME PDXX ON AYXX.BF_SSN = PDXX.DF_PRS_ID
					JOIN PKUB.PDXX_GTR_DTH PDXX ON AYXX.BF_SSN = PDXX.DF_PRS_ID
				WHERE
					AYXX.PF_REQ_ACT IN (''DIDED'',''DIDEM'',''DIDTH'')
					AND
					AYXX.LD_ATY_REQ_RCV >= ''XX/XX/XXXX''

				UNION

				SELECT
					PDXX.DF_PRS_ID AS Borrower_SSN,
					RTRIM(PDXX.DM_PRS_X) || '' '' ||PDXX.DM_PRS_LST AS Borrower_Name,
					CASE
						WHEN DWXX.WC_DW_LON_STA = ''XX''
							THEN ''X''
							ELSE ''X''
					END AS Status
				FROM
					PKUB.PDXX_PRS_NME PDXX
					JOIN PKUB.DWXX_DW_CLC_CLU DWXX ON PDXX.DF_PRS_ID = DWXX.BF_SSN
					JOIN PKUB.LNXX_LON LNXX ON DWXX.BF_SSN = LNXX.BF_SSN AND DWXX.LN_SEQ = LNXX.LN_SEQ
				WHERE
					DWXX.WC_DW_LON_STA = ''XX''
					OR
					(DWXX.WC_DW_LON_STA = XX AND LNXX.LD_PIF_RPT >= ''XX/XX/XXXX'')
			'
		) D