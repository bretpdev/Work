
-- =============================================
-- Author:		Eric Lynes
-- Create date: 2/09/2012
-- Description:	Merge Staging.Endorser and BR03_Reference
-- =============================================
CREATE PROCEDURE [dbo].[spSSIS_MergeStagingEndorser_LN20_Endorser] 	
AS
BEGIN	
	SET NOCOUNT ON;

    MERGE [dbo].[LN20_Endorser] as e
	USING Staging.Endorser as staging
	ON e.DF_SPE_ACC_ID = staging.DF_SPE_ACC_ID
		AND e.LN_SEQ = staging.LN_SEQ
		/*first, delete*/
	WHEN MATCHED AND (staging.LC_STA_LON20 <> 'A') THEN DELETE
	/*then update*/
	WHEN MATCHED THEN UPDATE
		SET e.[LC_STA_LON20] = staging.[LC_STA_LON20]
	WHEN NOT MATCHED AND (staging.LC_STA_LON20 = 'A') THEN
	/*finally, insert*/
		INSERT([DF_SPE_ACC_ID]
      ,[LN_SEQ]
      ,[LC_STA_LON20])
	   VALUES(
	   staging.[DF_SPE_ACC_ID]
      ,staging.[LN_SEQ]
      ,staging.[LC_STA_LON20]);
END

