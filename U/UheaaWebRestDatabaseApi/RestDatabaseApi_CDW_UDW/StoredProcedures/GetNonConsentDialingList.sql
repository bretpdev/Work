CREATE PROCEDURE [NobleController].[GetNonConsentDialingList]

@Agent varchar(100),
@Campaign varchar(500)
AS

BEGIN TRANSACTION
IF (DB_NAME() = 'CDW')
BEGIN


DELETE 
	PC 
FROM 
	[NobleController].[PendingCalls] PC
	LEFT JOIN OPENQUERY(UHEAAAPP1,
	'
		SELECT 
			"< 45 count", 
			"State", 
			"Days Delinq", 
			"Amount Due", 
			SSN1, 
			"Primary", 
			"Alternate"
		FROM
		(
			SELECT 
				Count_Last_45 AS "< 45 count",
				dc_dom_st AS "State",
				ln_dlq_max AS "Days Delinq",
				CONCAT
				(
					''$'',
					(LEFT(CAST(amt_due AS VARCHAR(100)),LENGTH(CAST(amt_due AS VARCHAR(100)))-2)), 
					''.'',
					(RIGHT(CAST(amt_due AS VARCHAR(100)),2))
				) AS "Amount Due",
				cornerstone_1.ssn1,
				phn_h AS "Primary",
				phn_a AS "Alternate",
				Max_CallDate,
				ROW_NUMBER() OVER(PARTITION BY cornerstone_1.SSN1 ORDER BY ln_dlq_max DESC) AS Dupe_Filter --Used to filter out duplicates
			FROM Cornerstone_1
			LEFT JOIN 
			(
				SELECT
					CONCAT
					(
						CASE WHEN filler_0 = ''A'' THEN ''0''
							 WHEN filler_0 = ''B'' THEN ''1''
							 WHEN filler_0 = ''C'' THEN ''2''
							 WHEN filler_0 = ''D'' THEN ''3''
							 WHEN filler_0 = ''E'' THEN ''4''
							 WHEN filler_0 = ''F'' THEN ''5''
							 WHEN filler_0 = ''G'' THEN ''6''
							 WHEN filler_0 = ''H'' THEN ''7''
							 WHEN filler_0 = ''I'' THEN ''8''
							 WHEN filler_0 = ''J'' THEN ''9''
							 ELSE filler_0 
						END,
						Filler_1,
						Filler_2
					) AS SSN1,
					COUNT(CONCAT(filler_0,filler_1,filler_2)) AS Count_Last_45,
					MAX(CallDate) AS Max_CallDate --Used to filter out accounts w/ calls same day
				FROM 
					CS_GRID
				WHERE 
					(CURRENT_DATE - CallDate) < 45
				GROUP BY 
					filler_0,
					filler_1,
					filler_2
			) AS Grid
				ON cornerstone_1.ssn1 = grid.ssn1
			WHERE
				geo_phn_ind1 = ''N''
				AND (Max_CallDate <> CURRENT_DATE OR Max_CallDate IS NULL) --Filters out calls made TODAY
				AND 
				(
					ln_dlq_max BETWEEN ''39'' AND ''44''
					OR ln_dlq_max BETWEEN ''84'' AND ''89''
					OR ln_dlq_max BETWEEN ''129'' AND ''134''
					OR ln_dlq_max BETWEEN ''174'' AND ''179''
					OR ln_dlq_max BETWEEN ''219'' AND ''224''
					OR ln_dlq_max BETWEEN ''264'' AND ''269''
					OR ln_dlq_max BETWEEN ''309'' AND ''314''
					OR ln_dlq_max BETWEEN ''354'' AND ''359''
				)
				AND COALESCE(Count_Last_45,0) < 2 
		) AS FINALTABLE
		WHERE 
			Dupe_Filter = 1 -- REMOVE DUPLICATES
		ORDER BY 
			COALESCE("< 45 count",0)+1,-- Orders the list by blank first then lowest call attempt 
			"Days Delinq" DESC
	') Fed
		ON FED.SSN1 = PC.[AccountNumber]
WHERE 
	FED.SSN1 IS NULL
	
	INSERT INTO [NobleController].[PendingCalls]
	SELECT TOP 5 
		RTRIM(PD10.DM_PRS_LST) + ', ' + RTRIM(PD10.DM_PRS_1) AS [Name], 
		COALESCE(Fed.[< 45 Count],0) AS CountLast45,
		Fed.[State] AS [State],
		COALESCE(Fed.[Days Delinq],0) AS [DaysDelinquent],
		Fed.[Amount Due] AS AmountDue,
		Fed.SSN1 AS AccountNumber,
		RTRIM(Fed.[Primary]) AS [Primary],
		RTRIM(Fed.[Alternate]) AS [Alternate],
		@Agent as Agent,
		@Campaign as Campaign
	FROM OPENQUERY(UHEAAAPP1,
	'
		SELECT 
			"< 45 count", 
			"State", 
			"Days Delinq", 
			"Amount Due", 
			SSN1, 
			"Primary", 
			"Alternate"
		FROM
		(
			SELECT 
				Count_Last_45 AS "< 45 count",
				dc_dom_st AS "State",
				ln_dlq_max AS "Days Delinq",
				CONCAT
				(
					''$'',
					(LEFT(CAST(amt_due AS VARCHAR(100)),LENGTH(CAST(amt_due AS VARCHAR(100)))-2)), 
					''.'',
					(RIGHT(CAST(amt_due AS VARCHAR(100)),2))
				) AS "Amount Due",
				cornerstone_1.ssn1,
				phn_h AS "Primary",
				phn_a AS "Alternate",
				Max_CallDate,
				ROW_NUMBER() OVER(PARTITION BY cornerstone_1.SSN1 ORDER BY ln_dlq_max DESC) AS Dupe_Filter --Used to filter out duplicates
			FROM Cornerstone_1
			LEFT JOIN 
			(
				SELECT
					CONCAT
					(
						CASE WHEN filler_0 = ''A'' THEN ''0''
							 WHEN filler_0 = ''B'' THEN ''1''
							 WHEN filler_0 = ''C'' THEN ''2''
							 WHEN filler_0 = ''D'' THEN ''3''
							 WHEN filler_0 = ''E'' THEN ''4''
							 WHEN filler_0 = ''F'' THEN ''5''
							 WHEN filler_0 = ''G'' THEN ''6''
							 WHEN filler_0 = ''H'' THEN ''7''
							 WHEN filler_0 = ''I'' THEN ''8''
							 WHEN filler_0 = ''J'' THEN ''9''
							 ELSE filler_0 
						END,
						Filler_1,
						Filler_2
					) AS SSN1,
					COUNT(CONCAT(filler_0,filler_1,filler_2)) AS Count_Last_45,
					MAX(CallDate) AS Max_CallDate --Used to filter out accounts w/ calls same day
				FROM 
					CS_GRID
				WHERE 
					(CURRENT_DATE - CallDate) < 45
				GROUP BY 
					filler_0,
					filler_1,
					filler_2
			) AS Grid
				ON cornerstone_1.ssn1 = grid.ssn1
			WHERE
				geo_phn_ind1 = ''N''
				AND (Max_CallDate <> CURRENT_DATE OR Max_CallDate IS NULL) --Filters out calls made TODAY
				AND 
				(
					ln_dlq_max BETWEEN ''39'' AND ''44''
					OR ln_dlq_max BETWEEN ''84'' AND ''89''
					OR ln_dlq_max BETWEEN ''129'' AND ''134''
					OR ln_dlq_max BETWEEN ''174'' AND ''179''
					OR ln_dlq_max BETWEEN ''219'' AND ''224''
					OR ln_dlq_max BETWEEN ''264'' AND ''269''
					OR ln_dlq_max BETWEEN ''309'' AND ''314''
					OR ln_dlq_max BETWEEN ''354'' AND ''359''
				)
				AND COALESCE(Count_Last_45,0) < 2 
		) AS FINALTABLE
		WHERE 
			Dupe_Filter = 1 -- REMOVE DUPLICATES
		ORDER BY 
			COALESCE("< 45 count",0)+1,-- Orders the list by blank first then lowest call attempt 
			"Days Delinq" DESC
	') Fed
	LEFT JOIN PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = Fed.SSN1
		--ON PD10.DF_SPE_ACC_ID = Fed.SSN1 --TODO: Use this once the account number change happens
END
IF (DB_NAME() = 'UDW')
BEGIN

DELETE 
	PC 
FROM 
	[NobleController].[PendingCalls] PC
	LEFT JOIN OPENQUERY(UHEAAAPP1,
	'
		SELECT 
			"< 45 count", 
			"State", 
			"Days Delinq", 
			"Amount Due", 
			SSN1, 
			"Primary", 
			"Alternate"
		FROM
		(
			SELECT 
				Count_Last_45 AS "< 45 count",
				dc_dom_st AS "State",
				ln_dlq_max AS "Days Delinq",
				CONCAT
				(
					''$'',
					(LEFT(CAST(amt_due AS VARCHAR(100)),LENGTH(CAST(amt_due AS VARCHAR(100)))-2)), 
					''.'',
					(RIGHT(CAST(amt_due AS VARCHAR(100)),2))
				) AS "Amount Due",
				UHEAA_1_Master.ssn1,
				phn_h AS "Primary",
				phn_a AS "Alternate",
				Max_CallDate,
				ROW_NUMBER() OVER(PARTITION BY UHEAA_1_Master.SSN1 ORDER BY ln_dlq_max DESC) AS Dupe_Filter --Used to filter out duplicates
			FROM 
				UHEAA_1_Master
				LEFT JOIN 
				(
					SELECT
						CONCAT
						(
							CASE WHEN filler_0 = ''A'' THEN ''0''
								 WHEN filler_0 = ''B'' THEN ''1''
								 WHEN filler_0 = ''C'' THEN ''2''
								 WHEN filler_0 = ''D'' THEN ''3''
								 WHEN filler_0 = ''E'' THEN ''4''
								 WHEN filler_0 = ''F'' THEN ''5''
								 WHEN filler_0 = ''G'' THEN ''6''
								 WHEN filler_0 = ''H'' THEN ''7''
								 WHEN filler_0 = ''I'' THEN ''8''
								 WHEN filler_0 = ''J'' THEN ''9''
								 ELSE filler_0 
							END,
							Filler_1,
							Filler_2
						) AS SSN1,
						COUNT(CONCAT(filler_0,filler_1,filler_2)) AS Count_Last_45,
						MAX(CallDate) AS Max_CallDate --Used to filter out accounts w/ calls same day
					FROM 
						UH_GRID
					WHERE 
						(CURRENT_DATE - CallDate) < 45
					GROUP BY 
						filler_0,
						filler_1,
						filler_2
				) AS Grid
					ON UHEAA_1_Master.ssn1 = Grid.ssn1
			WHERE
				geo_phn_ind1 = ''N'' 
				AND (Max_CallDate <> CURRENT_DATE OR Max_CallDate IS NULL) --Filters out calls made TODAY
				AND
				(
					ln_dlq_max BETWEEN ''39'' AND ''44''
					OR ln_dlq_max BETWEEN ''84'' AND ''89''
					OR ln_dlq_max BETWEEN ''129'' AND ''134''
					OR ln_dlq_max BETWEEN ''174'' AND ''179''
					OR ln_dlq_max BETWEEN ''219'' AND ''224''
					OR ln_dlq_max BETWEEN ''264'' AND ''269''
					OR ln_dlq_max BETWEEN ''309'' AND ''314''
					OR ln_dlq_max BETWEEN ''354'' AND ''359''
				)
				AND COALESCE(Count_Last_45,0) < 2
		) AS FINALTABLE
		WHERE
			Dupe_Filter = 1 -- REMOVE DUPLICATES
		ORDER BY 
			COALESCE("< 45 count",0)+1,-- Orders the list by blank first then lowest call attempt 
			"Days Delinq" DESC
	') Uheaa
		ON Uheaa.SSN1 = PC.[AccountNumber]
WHERE 
	Uheaa.SSN1 IS NULL

	INSERT INTO [NobleController].[PendingCalls]
	SELECT TOP 5 
		RTRIM(PD10.DM_PRS_LST) + ', ' + RTRIM(PD10.DM_PRS_1) AS [Name], 
		COALESCE(Uheaa.[< 45 Count],0) AS CountLast45,
		Uheaa.[State] AS [State],
		COALESCE(Uheaa.[Days Delinq],0) AS [DaysDelinquent],
		Uheaa.[Amount Due] AS AmountDue,
		Uheaa.SSN1 AS AccountNumber,
		RTRIM(Uheaa.[Primary]) AS [Primary],
		RTRIM(Uheaa.[Alternate]) AS [Alternate],
		@Agent as Agent,
		@Campaign as Campaign
	FROM OPENQUERY(UHEAAAPP1,
	'
		SELECT 
			"< 45 count", 
			"State", 
			"Days Delinq", 
			"Amount Due", 
			SSN1, 
			"Primary", 
			"Alternate"
		FROM
		(
			SELECT 
				Count_Last_45 AS "< 45 count",
				dc_dom_st AS "State",
				ln_dlq_max AS "Days Delinq",
				CONCAT
				(
					''$'',
					(LEFT(CAST(amt_due AS VARCHAR(100)),LENGTH(CAST(amt_due AS VARCHAR(100)))-2)), 
					''.'',
					(RIGHT(CAST(amt_due AS VARCHAR(100)),2))
				) AS "Amount Due",
				UHEAA_1_Master.ssn1,
				phn_h AS "Primary",
				phn_a AS "Alternate",
				Max_CallDate,
				ROW_NUMBER() OVER(PARTITION BY UHEAA_1_Master.SSN1 ORDER BY ln_dlq_max DESC) AS Dupe_Filter --Used to filter out duplicates
			FROM 
				UHEAA_1_Master
				LEFT JOIN 
				(
					SELECT
						CONCAT
						(
							CASE WHEN filler_0 = ''A'' THEN ''0''
								 WHEN filler_0 = ''B'' THEN ''1''
								 WHEN filler_0 = ''C'' THEN ''2''
								 WHEN filler_0 = ''D'' THEN ''3''
								 WHEN filler_0 = ''E'' THEN ''4''
								 WHEN filler_0 = ''F'' THEN ''5''
								 WHEN filler_0 = ''G'' THEN ''6''
								 WHEN filler_0 = ''H'' THEN ''7''
								 WHEN filler_0 = ''I'' THEN ''8''
								 WHEN filler_0 = ''J'' THEN ''9''
								 ELSE filler_0 
							END,
							Filler_1,
							Filler_2
						) AS SSN1,
						COUNT(CONCAT(filler_0,filler_1,filler_2)) AS Count_Last_45,
						MAX(CallDate) AS Max_CallDate --Used to filter out accounts w/ calls same day
					FROM 
						UH_GRID
					WHERE 
						(CURRENT_DATE - CallDate) < 45
					GROUP BY 
						filler_0,
						filler_1,
						filler_2
				) AS Grid
					ON UHEAA_1_Master.ssn1 = Grid.ssn1
			WHERE
				geo_phn_ind1 = ''N'' 
				AND (Max_CallDate <> CURRENT_DATE OR Max_CallDate IS NULL) --Filters out calls made TODAY
				AND
				(
					ln_dlq_max BETWEEN ''39'' AND ''44''
					OR ln_dlq_max BETWEEN ''84'' AND ''89''
					OR ln_dlq_max BETWEEN ''129'' AND ''134''
					OR ln_dlq_max BETWEEN ''174'' AND ''179''
					OR ln_dlq_max BETWEEN ''219'' AND ''224''
					OR ln_dlq_max BETWEEN ''264'' AND ''269''
					OR ln_dlq_max BETWEEN ''309'' AND ''314''
					OR ln_dlq_max BETWEEN ''354'' AND ''359''
				)
				AND COALESCE(Count_Last_45,0) < 2
		) AS FINALTABLE
		WHERE
			Dupe_Filter = 1 -- REMOVE DUPLICATES
		ORDER BY 
			COALESCE("< 45 count",0)+1,-- Orders the list by blank first then lowest call attempt 
			"Days Delinq" DESC
	') Uheaa
	LEFT JOIN PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = Uheaa.SSN1
		--ON PD10.DF_SPE_ACC_ID = Uheaa.SSN1 --TODO: Use this once the account number change happens
END

SELECT
	[Name]
    ,[CountLast45]
    ,[State]
    ,[DaysDelinquent]
    ,[AmountDue]
    ,[AccountNumber]
    ,[Primary]
    ,[Alternate]
FROM
	[NobleController].[PendingCalls]
WHERE
	Agent = @Agent
COMMIT