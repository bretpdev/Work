-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[testpdftable] 
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		LN10.IC_LON_PGM,
		LN10.LD_LON_1_DSB,
		LN10.LA_CUR_PRI,
		LN72.LR_ITR
	FROM
		dbo.LN10_Loan LN10
		JOIN LN72_InterestRate LN72
			ON LN10.DF_SPE_ACC_ID = LN72.DF_SPE_ACC_ID
	WHERE 
		LN10.DF_SPE_ACC_ID = '0015135373'
END