
-- =============================================
-- Author:		Eric Lynes
-- Create date: 2/08/2012
-- Description:	Merge Staging.Reference and BR03_Reference
-- =============================================
CREATE PROCEDURE [dbo].[spSSIS_MergeStagingReference_BR03_Reference] 	
AS
BEGIN	
	SET NOCOUNT ON;

    MERGE [dbo].[BR03_Reference] as br03
	USING Staging.Reference as r
	ON br03.DF_SPE_ACC_ID = r.DF_SPE_ACC_ID
		AND br03.DF_PRS_ID_RFR = r.DF_PRS_ID_RFR
		/*first, delete*/
	WHEN MATCHED AND (r.BC_STA_BR03 <> 'A') THEN DELETE
	/*then update*/
	WHEN MATCHED THEN UPDATE
		SET br03.[BC_STA_BR03] = r.[BC_STA_BR03]
		  , br03.[BI_ATH_3_PTY] = r.[BI_ATH_3_PTY]
		  , br03.[BC_RFR_REL_BR] = r.[BC_RFR_REL_BR]
		  , br03.[BM_RFR_1] = r.[BM_RFR_1]
		  , br03.[BM_RFR_LST] = r.[BM_RFR_LST]
		  , br03.[LST_CNC] = r.[LST_CNC]
		  , br03.[LST_ATT] = r.[LST_ATT]
		  , br03.[RFR_REL_BR] = r.[RFR_REL_BR]
	WHEN NOT MATCHED AND (r.BC_STA_BR03 = 'A') THEN
	/*finally, insert*/
		INSERT([DF_SPE_ACC_ID]
      ,[DF_PRS_ID_RFR]
      ,[BC_STA_BR03]
      ,[BI_ATH_3_PTY]
      ,[BC_RFR_REL_BR]
      ,[BM_RFR_1]
      ,[BM_RFR_LST]
      ,[LST_CNC]
      ,[LST_ATT]
      ,[RFR_REL_BR])
	   VALUES(
	   r.[DF_SPE_ACC_ID]
      ,r.[DF_PRS_ID_RFR]
      ,r.[BC_STA_BR03]
      ,r.[BI_ATH_3_PTY]
      ,r.[BC_RFR_REL_BR]
      ,r.[BM_RFR_1]
      ,r.[BM_RFR_LST]
      ,r.[LST_CNC]
      ,r.[LST_ATT]
      ,r.[RFR_REL_BR]);
END

