USE CDW
GO

MERGE
	dbo.BORR_Repayment ExistingData
USING
	(
		SELECT DISTINCT 
			COALESCE (LN65Base.DF_SPE_ACC_ID, FB10Base.DF_SPE_ACC_ID) AS DF_SPE_ACC_ID,
			COALESCE (LN65Base.LD_CRT_LON65, ' ') AS LD_CRT_LON65, 
			DUE_DAY = 
			CAST(
				COALESCE(FB10Base.DUE_DAY, 
					SUBSTRING(
						(
							SELECT
								(', ' + D.DUE_DAY)
							FROM
							(
								SELECT DISTINCT 
									LN65Day.DF_SPE_ACC_ID, 
									LN65Day.DUE_DAY
								FROM 
									dbo.LN65_REPAYMENTSCHED LN65Day
								GROUP BY 
									LN65Day.DF_SPE_ACC_ID, 
									LN65Day.DUE_DAY
							) D
							WHERE
								LN65Base.DF_SPE_ACC_ID = D.DF_SPE_ACC_ID
							ORDER BY 
								CAST(D.DUE_DAY AS INT) FOR XML PATH('')
						), 3, 50
					)
				) 
			AS VARCHAR), 
			MONTH_AMT = 
			CAST(
				COALESCE(FB10Base.MONTH_AMT,
					SUBSTRING(
						(
							SELECT
								(', $' + CONVERT(VARCHAR, LA_RPS_ISL, 1))
							FROM
							(
								SELECT
									LN65Amount.DF_SPE_ACC_ID,
									LN65Amount.DUE_DAY,
									SUM(LN65Amount.LA_RPS_ISL) AS LA_RPS_ISL
								FROM 
									dbo.LN65_REPAYMENTSCHED LN65Amount
								GROUP BY
									LN65Amount.DF_SPE_ACC_ID,
									LN65Amount.DUE_DAY
							) D
							WHERE
								LN65Base.DF_SPE_ACC_ID = D.DF_SPE_ACC_ID
							ORDER BY 
								CAST(D.DUE_DAY AS INT) FOR XML PATH('')
						), 3, 100
					)
				)
			AS VARCHAR), 
			COALESCE(FB10Base.MULT_DUE_DT, LN65Base.MULT_DUE_DT) AS MULT_DUE_DT
		FROM
			(
				SELECT
					DF_SPE_ACC_ID, 
					CASE WHEN COUNT(DISTINCT DUE_DAY) > 1 
						THEN 'Y' 
						ELSE 'N' 
					END AS MULT_DUE_DT, 
					CONVERT(VARCHAR, MAX(CAST(LD_CRT_LON65 AS DATETIME)), 101) AS LD_CRT_LON65
				FROM
					dbo.LN65_REPAYMENTSCHED
				GROUP BY 
					DF_SPE_ACC_ID
			) LN65Base 
			FULL OUTER JOIN
			(
				SELECT DISTINCT 
					DF_SPE_ACC_ID, 
					'RPF=$' + CONVERT(VARCHAR, SUM(LA_REQ_RDC_PAY), 1) AS MONTH_AMT, 
					CAST(DAY(DATEADD(s, - 1, DATEADD(mm, DATEDIFF(m, 0, GETDATE()) + 1, 0))) AS VARCHAR) AS DUE_DAY,
					'N' AS MULT_DUE_DT
				FROM
					dbo.FB10_FORBEARANCE
				WHERE
					CAST(LD_FOR_BEG AS DATETIME) <= GETDATE() 
					AND CAST(LD_FOR_END AS DATETIME) >= GETDATE() 
					AND LA_REQ_RDC_PAY > 0
				GROUP BY 
					DF_SPE_ACC_ID
			) FB10Base 
				ON LN65Base.DF_SPE_ACC_ID = FB10Base.DF_SPE_ACC_ID
	) NewData
		ON NewData.DF_SPE_ACC_ID = ExistingData.DF_SPE_ACC_ID
WHEN MATCHED THEN UPDATE SET
	ExistingData.LD_CRT_LON65 = NewData.LD_CRT_LON65,
	ExistingData.DUE_DAY = NewData.DUE_DAY,
	ExistingData.MONTH_AMT = NewData.MONTH_AMT,
	ExistingData.MULT_DUE_DT = NewData.MULT_DUE_DT
WHEN NOT MATCHED THEN
	INSERT
	(
		DF_SPE_ACC_ID,
		LD_CRT_LON65,
		DUE_DAY,
		MONTH_AMT,
		MULT_DUE_DT
	)
	VALUES
	(
		NewData.DF_SPE_ACC_ID,
		NewData.LD_CRT_LON65,
		NewData.DUE_DAY,
		NewData.MONTH_AMT,
		NewData.MULT_DUE_DT
	)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;