
-- =============================================
-- Author:		Eric Lynes
-- Create date: 02/08/2012
-- Description:	Merge Staging.FinancialTran and LN90_FinancialTran
-- =============================================
CREATE PROCEDURE [dbo].[spSSIS_MergeStagingFinTran_LN90_FinTran] 	
AS
BEGIN	
	SET NOCOUNT ON;

    MERGE [dbo].[LN90_FinancialTran] as ln90
	USING [Staging].[FinancialTran] as ft
	ON ln90.DF_SPE_ACC_ID = ft.DF_SPE_ACC_ID
		AND ln90.LN_SEQ = ft.LN_SEQ
		AND ln90.LN_FAT_SEQ = ft.LN_FAT_SEQ
		/*first, delete*/
	WHEN MATCHED AND (ft.LC_STA_LON90 <> 'A') THEN DELETE
	/*then update*/
	WHEN MATCHED THEN UPDATE 
		SET ln90.[LD_FAT_PST] = ft.[LD_FAT_PST]
      ,ln90.[LD_FAT_EFF] = ft.[LD_FAT_EFF]
      ,ln90.[LC_STA_LON90] = ft.[LC_STA_LON90]
      ,ln90.[LA_FAT_LTE_FEE] = ft.[LA_FAT_LTE_FEE]
      ,ln90.[LA_FAT_CUR_PRI] = ft.[LA_FAT_CUR_PRI]
      ,ln90.[PC_FAT_TYP] = ft.[PC_FAT_TYP]
      ,ln90.[PC_FAT_SUB_TYP] = ft.[PC_FAT_SUB_TYP]
      ,ln90.[LC_FAT_REV_REA] = ft.[LC_FAT_REV_REA]
      ,ln90.[LA_FAT_NSI] = ft.[LA_FAT_NSI]
      ,ln90.[FAT_REV_REA] = ft.[FAT_REV_REA]
	WHEN NOT MATCHED AND (ft.LC_STA_LON90 = 'A') THEN
	/*finally, insert*/
		INSERT([DF_SPE_ACC_ID]
      ,[LN_SEQ]
      ,[LN_FAT_SEQ]
      ,[LD_FAT_PST]
      ,[LD_FAT_EFF]
      ,[LC_STA_LON90]
      ,[LA_FAT_LTE_FEE]
      ,[LA_FAT_CUR_PRI]
      ,[PC_FAT_TYP]
      ,[PC_FAT_SUB_TYP]
      ,[LC_FAT_REV_REA]
      ,[LA_FAT_NSI]
      ,[FAT_REV_REA])
	   VALUES(
	   ft.[DF_SPE_ACC_ID]
      ,ft.[LN_SEQ]
      ,ft.[LN_FAT_SEQ]
      ,ft.[LD_FAT_PST]
      ,ft.[LD_FAT_EFF]
      ,ft.[LC_STA_LON90]
      ,ft.[LA_FAT_LTE_FEE]
      ,ft.[LA_FAT_CUR_PRI]
      ,ft.[PC_FAT_TYP]
      ,ft.[PC_FAT_SUB_TYP]
      ,ft.[LC_FAT_REV_REA]
      ,ft.[LA_FAT_NSI]
      ,ft.[FAT_REV_REA]);
END

