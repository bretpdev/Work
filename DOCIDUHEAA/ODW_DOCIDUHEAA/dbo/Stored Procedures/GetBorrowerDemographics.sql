
CREATE PROCEDURE [dbo].[GetBorrowerDemographics]
	@AccountIdentifier VARCHAR(10)
AS

DECLARE @Borrower varchar(10) = 
	(
		SELECT DISTINCT
			GA01.DF_PRS_ID_BR
		FROM
			GA01_APP GA01
			INNER JOIN PD01_PDM_INF PD01
				ON GA01.DF_PRS_ID_BR = PD01.DF_PRS_ID
		WHERE
			@AccountIdentifier IN (PD01.DF_PRS_ID, PD01.DF_SPE_ACC_ID)
	)

DECLARE @Student varchar(10)
IF @Borrower = '' OR @Borrower IS NULL
	SET @Student = 
		(
			SELECT DISTINCT
				GA01.DF_PRS_ID_STU
			FROM
				GA01_APP GA01
				INNER JOIN PD01_PDM_INF PD01
					ON GA01.DF_PRS_ID_STU = PD01.DF_PRS_ID
			WHERE
				@AccountIdentifier IN (PD01.DF_PRS_ID, PD01.DF_SPE_ACC_ID)
		)

IF LEN(@Student) > 0
	SET @AccountIdentifier = 
		(
			SELECT DISTINCT
				DF_PRS_ID_BR
			FROM
				GA01_APP
			WHERE
				DF_PRS_ID_STU = @Student
		)

DECLARE @SSN VARCHAR(9) = (SELECT DF_PRS_ID FROM PD01_PDM_INF WHERE @AccountIdentifier IN (DF_PRS_ID, DF_SPE_ACC_ID))

SELECT 
	PD01.DF_PRS_ID [AccountIdentifier],
	PD01.DF_SPE_ACC_ID [AccountNumber],
	PD01.DF_PRS_ID [Ssn],
 	ISNULL(NULLIF(RTRIM(PD01.DM_PRS_1) + ' ' + RTRIM(PD01.DM_PRS_MID) +  ' ' + RTRIM(PD01.DM_PRS_LST), ' '),'') AS [Name],  
 	CASE WHEN ADR_DATA.BorrAddress1 != '' THEN ADR_DATA.BorrAddress1 ELSE 'No address record on file' END AS [Address1], 
 	ISNULL(ADR_DATA.BorrAddress2,'') AS [Address2],
 	ISNULL(ADR_DATA.BorrCity,'') AS [City], 
 	ISNULL(ADR_DATA.BorrState,'') AS [State], 
 	ISNULL(ADR_DATA.BorrZip,'') AS [Zip],
	1 [IsOneLink]
FROM  
 	PD01_PDM_INF PD01
 	LEFT JOIN  
 	( 
 		SELECT 
  			ADR_RNK.DF_PRS_ID,  
  			ADR_RNK.BorrAddress1,  
 			ADR_RNK.BorrAddress2, 
 			ADR_RNK.BorrCountry, 
 			ADR_RNK.BorrCity, 
 			ADR_RNK.BorrState, 
 			ADR_RNK.BorrZip, 
  			ROW_NUMBER() OVER (PARTITION BY ADR_RNK.DF_PRS_ID ORDER BY ADR_RNK.VLD, ADR_RNK.RNK) AS ADR_PRIORITY 
 			FROM  
 			(  
  				SELECT  
  					PD03.DF_PRS_ID,  
 					CASE PD03.DC_ADR   
  						WHEN 'L' THEN 1  
  						WHEN 'B' THEN 2  
  						WHEN 'D' THEN 3  
  						ELSE 4  
  					END AS RNK,  
  					CASE PD03.DI_VLD_ADR  
  						WHEN 'Y' THEN 1  
  						WHEN 'N' THEN 2  
  						ELSE 3  
  					END AS VLD,  
  					ISNULL(RTRIM(PD03.DX_STR_ADR_1),'') AS BorrAddress1, 
 					ISNULL(RTRIM(PD03.DX_STR_ADR_2),'') AS BorrAddress2, 
 					ISNULL(RTRIM(PD03.DM_FGN_CNY),'') AS BorrCountry, 
 					ISNULL(RTRIM(PD03.DM_CT),'') AS BorrCity, 
 					ISNULL(RTRIM(PD03.DC_DOM_ST),'') AS BorrState, 
 					ISNULL(RTRIM(PD03.DF_ZIP),'') AS BorrZip 
  				FROM 
 					PD03_PRS_ADR_PHN PD03
 				WHERE  
 					PD03.DF_PRS_ID = @SSN 
 		) AS ADR_RNK  
 	) AS ADR_DATA  
 		ON ADR_DATA.DF_PRS_ID = PD01.DF_PRS_ID  
 		AND ADR_DATA.ADR_PRIORITY = 1 
WHERE  
 	@AccountIdentifier IN (PD01.DF_PRS_ID, PD01.DF_SPE_ACC_ID)

UNION

SELECT
	BR03.DF_PRS_ID_RFR [AccountIdentifier],
	'' [AccountNumber],
	'' [Ssn],
	RTRIM(BR03.BM_RFR_1) + ' ' + RTRIM(BR03.BM_RFR_MID) +  ' ' + RTRIM(BR03.BM_RFR_LST) [Name],  
 	CASE WHEN BR03.BX_RFR_STR_ADR_1 != '' THEN BR03.BX_RFR_STR_ADR_1 ELSE 'No address record on file' END AS [Address1], 
 	ISNULL(RTRIM(BR03.BX_RFR_STR_ADR_2),'') AS [Address2],
 	ISNULL(RTRIM(BR03.BM_RFR_CT),'') AS [City], 
 	ISNULL(RTRIM(BR03.BC_RFR_ST),'') AS [State], 
 	ISNULL(RTRIM(BR03.BF_RFR_ZIP),'') AS [Zip],
	1 [IsOneLink]
FROM
	BR03_BR_REF BR03
WHERE
 	BR03.DF_PRS_ID_RFR = @AccountIdentifier
	AND BR03.BI_VLD_ADR = 'Y'