USE CDW
GO

MERGE
	dbo.BORR_BILL ExistingData
USING
	(
		SELECT
			BILL.DF_SPE_ACC_ID,
			BIL_MTD = STUFF(
								(
									SELECT 
										( ', ' + BIL_MTD  ) AS [text()]
									FROM 
									(
										SELECT DISTINCT
											LB.DF_SPE_ACC_ID,
											LB.BIL_MTD
										FROM
											LOAN_Bill LB
									) SUB
									WHERE 
										BILL.DF_SPE_ACC_ID = SUB.DF_SPE_ACC_ID
									ORDER BY 
										DF_SPE_ACC_ID,
										BIL_MTD
									FOR XML PATH( '' )
								),1,2,''
							),
			MULTI_TYP.MULT_BIL_MTD
		FROM 
			LOAN_Bill BILL
			LEFT JOIN 
			(
				SELECT 
					BILL.DF_SPE_ACC_ID,
					CASE WHEN COUNT(DISTINCT BILL.BIL_MTD) > 1 THEN 'Y' ELSE 'N' END AS MULT_BIL_MTD
				FROM
					LOAN_Bill BILL
				GROUP BY 
					BILL.DF_SPE_ACC_ID
			) MULTI_TYP
				ON BILL.DF_SPE_ACC_ID = MULTI_TYP.DF_SPE_ACC_ID
		GROUP BY 
			BILL.DF_SPE_ACC_ID, 
			MULTI_TYP.MULT_BIL_MTD
	) NewData
		ON NewData.DF_SPE_ACC_ID = ExistingData.DF_SPE_ACC_ID
WHEN MATCHED THEN UPDATE SET
	ExistingData.BIL_MTD = NewData.BIL_MTD,
	ExistingData.MULT_BIL_MTD = NewData.MULT_BIL_MTD
WHEN NOT MATCHED THEN
	INSERT
	(
		DF_SPE_ACC_ID,
		BIL_MTD,
		MULT_BIL_MTD
	)
	VALUES
	(
		NewData.DF_SPE_ACC_ID,
		NewData.BIL_MTD,
		NewData.MULT_BIL_MTD
	)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;