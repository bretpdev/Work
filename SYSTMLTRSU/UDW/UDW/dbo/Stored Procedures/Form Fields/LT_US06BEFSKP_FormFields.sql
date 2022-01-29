CREATE PROCEDURE [dbo].[LT_US06BEFSKP_FormFields]
	@BF_SSN VARCHAR(9),
	@IsCoborrower BIT = 0,
    @RN_ATY_SEQ_PRC INT
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = 'US06BEFSKP')

--Special reference only letter
	SELECT DISTINCT
		ISNULL(NULLIF(RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST), ' '),'') AS BorrName,  
		CASE WHEN ADR_DATA.BorrAddress1 != '' THEN ADR_DATA.BorrAddress1 ELSE 'No address record on file' END AS BorrAddress1, 
		ISNULL(ADR_DATA.BorrAddress2,'') AS BorrAddress2, 
		ISNULL(ADR_DATA.BorrCountry,'') AS BorrCountry, 
		ISNULL(ADR_DATA.BorrCity,'') AS BorrCity, 
		ISNULL(ADR_DATA.BorrState,'') AS BorrState, 
		ISNULL(ADR_DATA.BorrZip,'') AS BorrZip, 
		ISNULL(PHN_DATA.BorrPhone, 'No phone number on record') [BorrPhone] 
	FROM  
		PD10_PRS_NME PD10
		INNER JOIN LN10_LON LN10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN
		(
			SELECT
				LN85.BF_SSN,
				LN85.LN_SEQ
			FROM
				LN85_LON_ATY LN85
				INNER JOIN AY10_BR_LON_ATY AY10
                    ON AY10.BF_SSN = LN85.BF_SSN
                    AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
            WHERE   
                AY10.PF_REQ_ACT = @PF_REQ_ACT
                AND AY10.BF_SSN = @BF_SSN
                AND AY10.LN_ATY_SEQ = @RN_ATY_SEQ_PRC
                --Active flag ignored, as LT20 provides the exact record that is tied to this request
		)LN85
			ON LN85.BF_SSN = LN10.BF_SSN
			AND LN85.LN_SEQ = LN10.LN_SEQ 
		LEFT JOIN  
		(		 
 			SELECT 
  				PHN_RNK.DF_PRS_ID,  
  				PHN_RNK.BorrPhone,  
  				ROW_NUMBER() OVER (PARTITION BY PHN_RNK.DF_PRS_ID ORDER BY PHN_RNK.VLD, PHN_RNK.RNK) AS PHN_PRIORITY 
 				FROM  
 				(  
  				SELECT  
  					PD42.DF_PRS_ID,  
 					CASE PD42.DC_PHN   
  						WHEN 'H' THEN 1  
  						WHEN 'A' THEN 2  
  						WHEN 'W' THEN 3  
  						WHEN 'M' THEN 4  
  						ELSE 5  
  					END AS RNK,  
  					CASE PD42.DI_PHN_VLD  
  						WHEN 'Y' THEN 1  
  						WHEN 'N' THEN 2  
  						ELSE 3  
  					END AS VLD,  
  					COALESCE  
  					(  
  						CAST(NULLIF  
  						(  
  							RTRIM(PD42.DN_DOM_PHN_ARA) + '-' +   
  							RTRIM(PD42.DN_DOM_PHN_XCH) + '-' +   
  							RTRIM(PD42.DN_DOM_PHN_LCL)  
  							, '--'  
  						) AS VARCHAR(250)),     
  						NULLIF  
  						(  
  							RTRIM(PD42.DN_FGN_PHN_INL) + '-' +   
  							RTRIM(PD42.DN_FGN_PHN_CNY) + '-' +   
  							RTRIM(PD42.DN_FGN_PHN_CT) + '-' +   
  							RTRIM(PD42.DN_FGN_PHN_LCL)  
  							, '---'  
  						),  
  						'No phone number on record'  
  					) [BorrPhone]  
  				FROM 
 					PD42_PRS_PHN PD42
 				WHERE  
 					PD42.DF_PRS_ID = @BF_SSN 
 			) AS PHN_RNK  
		) AS PHN_DATA  
 			ON PHN_DATA.DF_PRS_ID = PD10.DF_PRS_ID  
 			AND PHN_DATA.PHN_PRIORITY = 1 
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
  					PD30.DF_PRS_ID,  
 					CASE PD30.DC_ADR   
  						WHEN 'L' THEN 1  
  						WHEN 'B' THEN 2  
  						WHEN 'D' THEN 3  
  						ELSE 4  
  					END AS RNK,  
  					CASE PD30.DI_VLD_ADR  
  						WHEN 'Y' THEN 1  
  						WHEN 'N' THEN 2  
  						ELSE 3  
  					END AS VLD,  
  					ISNULL(RTRIM(PD30.DX_STR_ADR_1),'') AS BorrAddress1, 
 					ISNULL(RTRIM(PD30.DX_STR_ADR_2),'') AS BorrAddress2, 
 					ISNULL(RTRIM(PD30.DM_FGN_CNY),'') AS BorrCountry, 
 					ISNULL(RTRIM(PD30.DM_CT),'') AS BorrCity, 
					CASE WHEN LEN(RTRIM(PD30.DM_FGN_ST)) > 0 
						THEN RTRIM(PD30.DM_FGN_ST) 
 						ELSE RTRIM(PD30.DC_DOM_ST) 
 					END AS BorrState,  
 					ISNULL(RTRIM(PD30.DF_ZIP_CDE),'') AS BorrZip 
  				FROM 
 					PD30_PRS_ADR PD30 
 				WHERE  
 					PD30.DF_PRS_ID = @BF_SSN 
 			) AS ADR_RNK  
		) AS ADR_DATA  
 			ON ADR_DATA.DF_PRS_ID = PD10.DF_PRS_ID  
 			AND ADR_DATA.ADR_PRIORITY = 1 
	WHERE  
		PD10.DF_PRS_ID = @BF_SSN 
END	 