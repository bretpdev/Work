CREATE PROCEDURE [rhbrwinpc].[GetAvailableLetterData]
AS
MERGE OLS.[rhbrwinpc].[Letters] AS TARGET USING
	(
		SELECT DISTINCT
			RTRIM(PD01.DF_SPE_ACC_ID) [AccountNumber],
			RTRIM(PD01.DM_PRS_1) [FirstName],
			RTRIM(PD01.DM_PRS_LST) [LastName],
			RTRIM(PD01.DX_STR_ADR_1) [Address1],
			RTRIM(PD01.DX_STR_ADR_2) [Address2],
			RTRIM(PD01.DM_CT) [City],
			CASE 
				WHEN PD01.DC_DOM_ST = 'FC' THEN ''
				ELSE RTRIM(PD01.DC_DOM_ST)
			END AS [State],
			RTRIM(PD01.DF_ZIP) [Zip],
			RTRIM(PD01.DM_FGN_CNY) [Country]
		FROM
			ODW..DC01_LON_CLM_INF DC01
			INNER JOIN
			(--Get the borrower account where the status code = 10 and matches the status update date
				SELECT
					AUXSTA.BF_SSN,
					PCDT.LD_AUX_STA_UPD
				FROM
					ODW..DC01_LON_CLM_INF AUXSTA --Aux Status
					INNER JOIN
					(--Get the max updated status date regardless of the status code
						SELECT
							BF_SSN,
							MAX(LD_AUX_STA_UPD) AS LD_AUX_STA_UPD
						FROM
							ODW..DC01_LON_CLM_INF 
						WHERE
							LD_AUX_STA_UPD IS NOT NULL
						GROUP BY
							BF_SSN
					) PCDT -- Pre-Claim Date
						ON AUXSTA.BF_SSN = PCDT.BF_SSN
						AND AUXSTA.LD_AUX_STA_UPD = PCDT.LD_AUX_STA_UPD
				WHERE
					AUXSTA.LC_AUX_STA = '10'
			) PCAS --Pre-Claim Account Status
				ON DC01.BF_SSN = PCAS.BF_SSN
			INNER JOIN
			(--Get the borrower account where claim status = 1, PCL_REA IN DB, DF, DQ and they have a matching status update date
				SELECT
					RSN.BF_SSN,
					PCDT.LD_STA_UPD_DC10
				FROM
					ODW..DC01_LON_CLM_INF RSN --Pre-Claim Reason
					INNER JOIN
					(--Gets the max preclaim status date regardless of the status code
						SELECT
							BF_SSN,
							MAX(LD_STA_UPD_DC10) AS LD_STA_UPD_DC10
						FROM
							ODW..DC01_LON_CLM_INF
						GROUP BY
							BF_SSN
					) PCDT --Pre-Claim Date
						ON RSN.BF_SSN = PCDT.BF_SSN
						AND RSN.LD_STA_UPD_DC10 = PCDT.LD_STA_UPD_DC10
				WHERE
					RSN.LC_STA_DC10 = '01'
					AND RSN.LC_PCL_REA IN ('DB','DF','DQ')
			) PCRSN --Pre-Claim Reason
				ON DC01.BF_SSN = PCRSN.BF_SSN
			INNER JOIN ODW..PD01_PDM_INF PD01
				ON DC01.BF_SSN = PD01.DF_PRS_ID
			LEFT JOIN	ODW..AY01_BR_ATY AY01
				ON DC01.BF_SSN = AY01.DF_PRS_ID	
				AND AY01.PF_ACT = 'ALSBR'
			WHERE
				PCAS.LD_AUX_STA_UPD < PCRSN.LD_STA_UPD_DC10
				AND CAST(DC01.LD_PCL_SUP_LST_ATT AS DATE) < CAST((GETDATE() - 3) AS DATE)
				AND CAST(DC01.LD_PCL_SUP_LST_CNC AS DATE) < CAST((GETDATE() - 5) AS DATE)
					AND PD01.DI_VLD_ADR = 'Y'
					AND AY01.DF_PRS_ID IS NULL
		) AS SOURCE
			ON
				(
					SOURCE.AccountNumber = TARGET.AccountNumber
					AND (TARGET.ArcAddProcessingId IS NULL OR TARGET.PrintedAt IS NULL)
					AND TARGET.DeletedAt IS NULL
				)
				OR
				(
					SOURCE.AccountNumber = TARGET.AccountNumber
					AND CAST(TARGET.AddedAt AS DATE) = CAST(GETDATE() AS DATE)
					AND TARGET.DeletedAt IS NULL
				)
WHEN NOT MATCHED THEN
	INSERT (AccountNumber,FirstName,LastName,Address1,Address2,City,[State],Zip,Country)
	VALUES(SOURCE.AccountNumber,SOURCE.FirstName,SOURCE.LastName,SOURCE.Address1,SOURCE.Address2,SOURCE.City,SOURCE.[State],SOURCE.Zip,SOURCE.Country)
WHEN NOT MATCHED BY SOURCE AND (TARGET.ArcAddProcessingId IS NULL AND TARGET.PrintedAt IS NULL AND TARGET.DeletedAt IS NULL AND TARGET.AddedAt < DATEADD(DAY, -3, GETDATE())) THEN
	UPDATE
	SET
		DeletedAt = GETDATE(),
		DeletedBy = SUSER_SNAME()
WHEN MATCHED AND (SOURCE.AccountNumber = TARGET.AccountNumber	AND (TARGET.ArcAddProcessingId IS NULL OR TARGET.PrintedAt IS NULL) AND TARGET.DeletedAt IS NULL) THEN
	UPDATE
	SET
		AccountNumber = SOURCE.AccountNumber,
		FirstName = SOURCE.FirstName,
		LastName = SOURCE.LastName,
		Address1 = SOURCE.Address1,
		Address2 = SOURCE.Address2,
		City = SOURCE.City,
		[State] = SOURCE.[State],
		Zip = SOURCE.Zip,
		Country = SOURCE.Country;