-- =============================================
-- Author:		Eric Lynes
-- Create date: 2/06/2012
-- Description:	Merge Staging.Billing and BL10_Billing
-- =============================================
CREATE PROCEDURE [dbo].[spSSIS_MergeStagingBilling_BL10_Billing] 	
AS
BEGIN	
	SET NOCOUNT ON;

    MERGE [dbo].[BL10_Bill] as bl10
	USING Staging.Billing as b
	ON bl10.DF_SPE_ACC_ID = b.DF_SPE_ACC_ID
		AND bl10.LN_SEQ = b.LN_SEQ
		AND bl10.LD_BIL_CRT = b.LD_BIL_CRT
		AND bl10.LN_SEQ_BIL_WI_DTE = b.LN_SEQ_BIL_WI_DTE
		/*first, delete*/
	WHEN MATCHED AND (b.LC_STA_BIL10 <> 'A' AND b.LC_STA_LON80 <> 'A') THEN DELETE
	/*then update*/
	WHEN MATCHED THEN UPDATE 
		SET bl10.[LD_BIL_DU_LON] = b.[LD_BIL_DU_LON]
		  ,bl10.[LC_STA_LON80] = b.[LC_STA_LON80]
		  ,bl10.[LA_BIL_CUR_DU] = b.[LA_BIL_CUR_DU]
		  ,bl10.[LA_BIL_PAS_DU] = b.[LA_BIL_PAS_DU]
		  ,bl10.[LC_BIL_MTD] = b.[LC_BIL_MTD]
		  ,bl10.[LC_IND_BIL_SNT] = b.[LC_IND_BIL_SNT]
		  ,bl10.[LC_STA_BIL10] = b.[LC_STA_BIL10]
		  ,bl10.[LA_TOT_BIL_STS] = b.[LA_TOT_BIL_STS]
		  ,bl10.[LD_BIL_STS_RIR_TOL] = b.[LD_BIL_STS_RIR_TOL]
		  ,bl10.[PAID_AHEAD] = b.[PAID_AHEAD]
		  ,bl10.[BIL_SAT] = b.[BIL_SAT]
		  ,bl10.[BIL_MTD] = b.[BIL_MTD]
	WHEN NOT MATCHED AND (b.LC_STA_BIL10 = 'A' AND b.LC_STA_LON80 = 'A') THEN
	/*finally, insert*/
		INSERT([DF_SPE_ACC_ID]
		  ,[LN_SEQ]
		  ,[LD_BIL_CRT]
		  ,[LN_SEQ_BIL_WI_DTE]
		  ,[LD_BIL_DU_LON]
		  ,[LC_STA_LON80]
		  ,[LA_BIL_CUR_DU]
		  ,[LA_BIL_PAS_DU]
		  ,[LC_BIL_MTD]
		  ,[LC_IND_BIL_SNT]
		  ,[LC_STA_BIL10]
		  ,[LA_TOT_BIL_STS]
		  ,[LD_BIL_STS_RIR_TOL]
		  ,[PAID_AHEAD]
		  ,[BIL_SAT]
		  ,[BIL_MTD])
	   VALUES(
	   b.[DF_SPE_ACC_ID]
		  ,b.[LN_SEQ]
		  ,b.[LD_BIL_CRT]
		  ,b.[LN_SEQ_BIL_WI_DTE]
		  ,b.[LD_BIL_DU_LON]
		  ,b.[LC_STA_LON80]
		  ,b.[LA_BIL_CUR_DU]
		  ,b.[LA_BIL_PAS_DU]
		  ,b.[LC_BIL_MTD]
		  ,b.[LC_IND_BIL_SNT]
		  ,b.[LC_STA_BIL10]
		  ,b.[LA_TOT_BIL_STS]
		  ,b.[LD_BIL_STS_RIR_TOL]
		  ,b.[PAID_AHEAD]
		  ,b.[BIL_SAT]
		  ,b.[BIL_MTD]);
END