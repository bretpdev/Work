SELECT * FROM OPENQUERY (DUSTER,'
	SELECT		
		 PD01.DF_SPE_ACC_ID AS Borrower_Acct_Nu
		,GA10.AC_LON_TYP	AS Guaranty_Date
		,GA10.AD_PRC		AS Guaranty_Amount
		,GA10.AA_GTE_LON_AMT AS Loan_Status
	FROM
		OLWHRM1.PD01_PDM_INF PD01
		INNER JOIN OLWHRM1.AY01_BR_ATY AY01
			ON PD01.DF_PRS_ID = AY01.DF_PRS_ID
		INNER JOIN OLWHRM1.GA01_APP GA01
			ON AY01.AF_APL_ID = GA01.AF_APL_ID
		INNER JOIN OLWHRM1.GA10_LON_APP GA10
			ON GA01.AF_APL_ID = GA10.AF_APL_ID
	WHERE
		GA10.AC_PRC_STA = ''A''
		AND GA01.AF_APL_OPS_SCL IN 
		(
			 ''01045200''
			,''02067500''
			,''02105200''
			,''02112100''
			,''02112400''
			,''02115000''
			,''02160400''
			,''02160700''
			,''02165600''
		)
');

SELECT * FROM OPENQUERY (DUSTER,'
	SELECT		
		GA01.AF_APL_OPS_SCL
	FROM
		OLWHRM1.GA01_APP GA01
	WHERE
		GA01.AF_APL_OPS_SCL IN 
		(
			 ''01045200''
			,''02067500''
			,''02105200''
			,''02112100''
			,''02112400''
			,''02115000''
			,''02160400''
			,''02160700''
			,''02165600''
		)
');
