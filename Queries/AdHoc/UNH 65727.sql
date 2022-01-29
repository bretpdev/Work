use udw
go

SELECT
	PD10.DF_SPE_ACC_ID,
	COMBINED_COMMENT.LX_ATY,
	COMBINED_COMMENT.LD_ATY_REQ_RCV
FROM
	PD10_PRS_NME PD10
	INNER JOIN
	(
		SELECT DISTINCT
			AY20.BF_SSN,
			ay10.LD_ATY_REQ_RCV,
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
			left JOIN PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = AY20.BF_SSN
		WHERE
			AY10.LD_ATY_REQ_RCV between '12/08/2019' and '12/16/2019'
			and ay10.LF_USR_REQ_ATY = 'UT02350 '
	) COMBINED_COMMENT
		ON COMBINED_COMMENT.BF_SSN = PD10.DF_PRS_ID
