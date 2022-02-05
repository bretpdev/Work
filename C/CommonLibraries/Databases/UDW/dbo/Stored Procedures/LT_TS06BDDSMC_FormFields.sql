﻿
 CREATE PROCEDURE [dbo].[LT_TS06BDDSMC_FormFields]
	@AccountNumber VARCHAR(10)
AS
	
    SELECT DISTINCT  
	CONVERT(VARCHAR(10), MAX(LN80.LD_BIL_DU_LON) OVER (PARTITION BY LN80.BF_SSN), 101) AS 'DueDate',  
    SUM(LN80.LA_BIL_CUR_DU) OVER (PARTITION BY LN80.BF_SSN)  AS 'InstallmentAmnt',
    COALESCE(BR30.BA_EFT_ADD_WDR, 0.00) AS 'AddtlACHAmnt',  
    (SUM(LN80.LA_BIL_CUR_DU) OVER (PARTITION BY LN80.BF_SSN) + (COALESCE(BR30.BA_EFT_ADD_WDR, 0.00))) AS 'TotalWithdrawlAmnt'    
FROM
    [dbo].LN80_LON_BIL_CRF LN80
	INNER JOIN PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = LN80.BF_SSN
    INNER JOIN  [dbo].BR30_BR_EFT BR30    
        ON  BR30.BF_SSN = LN80.BF_SSN 
		AND BR30.BC_EFT_STA = 'A'  
    WHERE   
        PD10.DF_SPE_ACC_ID = @AccountNumber  
        AND LN80.LC_STA_LON80 = 'A'     
        AND LN80.LC_LON_STA_BIL = 1  
        AND LN80.LD_BIL_DU_LON  BETWEEN DATEADD(DAY, -1, GETDATE()) AND DATEADD(DAY, -1, DATEADD(MONTH, 1, GETDATE()))


IF @@ROWCOUNT = 0  
 	BEGIN 
 		DECLARE @SprocName VARCHAR(MAX) = OBJECT_NAME(@@PROCID) 
 		RAISERROR('No data returned for Sproc %s.',11 ,2 ,@SprocName) 
 	END