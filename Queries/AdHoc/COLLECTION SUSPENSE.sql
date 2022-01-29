
SELECT DISTINCT APP.*, F.* FROM 
(
SELECT DISTINCT
							AY20.BF_SSN,
							PD10.DF_SPE_ACC_ID,
							AY10.LD_ATY_REQ_RCV,
							AY10.LN_ATY_SEQ,
							STUFF(
							(
								SELECT 
										' ' + SUB.LX_ATY AS [text()]
								FROM 
									AY20_ATY_TXT SUB
								WHERE
									SUB.BF_SSN = AY20.BF_SSN
									AND SUB.LN_ATY_SEQ = AY20.LN_ATY_SEQ
							FOR XML PATH('')
							)
							,1,1, '') AS LX_ATY
			
						FROM	
							AY10_BR_LON_ATY AY10
							INNER JOIN AY20_ATY_TXT AY20
								ON AY10.BF_SSN = AY20.BF_SSN
								AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
							INNER JOIN PD10_PRS_NME PD10
								ON PD10.DF_PRS_ID = AY20.BF_SSN
						WHERE
							AY10.PF_REQ_ACT = 'XFORB'
							AND AY20.LX_ATY like '%Borrower requests Collection Suspension%'
							AND AY10.LD_ATY_REQ_RCV > '08/27/2019'
) F
INNER JOIN CLS..ArcAddProcessing APP
	ON APP.AccountNumber = F.DF_SPE_ACC_ID
	AND APP.LN_ATY_SEQ = F.LN_ATY_SEQ
INNER JOIN CDW..DW01_DW_CLC_CLU DW01
	ON DW01.BF_SSN = F.BF_SSN
	AND DW01.WC_DW_LON_STA = '05'

ORDER BY CreatedAt DESC