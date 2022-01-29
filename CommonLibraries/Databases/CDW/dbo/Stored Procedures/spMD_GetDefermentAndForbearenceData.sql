CREATE PROCEDURE [dbo].[spMD_GetDefermentAndForbearenceData]
	@AccountNumber					VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		
	SELECT TypeCode,
			CONVERT(VARCHAR(10), BeginDateDate, 101) as BeginDate,
			EndDate,
			[Type],
			CertDate,
			CapInd,
			MonthsUsed,
			OneOfTheLoanSequenceNumbersAssociatedWithDeferOrForb,
			BeginDateDate
	FROM 
	(
		SELECT DISTINCT 'D' + LC_DFR_TYP as TypeCode,
						CAST(LD_DFR_BEG as DateTime) as BeginDateDate,
						LD_DFR_END as EndDate,
						DFR_TYP as [Type],
						LD_DFR_INF_CER as CertDate,
						LC_LON_LEV_DFR_CAP as CapInd,
						MONTHS_USED as MonthsUsed,
						MAX(LN_SEQ) as OneOfTheLoanSequenceNumbersAssociatedWithDeferOrForb
		FROM dbo.DF10_Deferment as Def1
		WHERE DF_SPE_ACC_ID = @AccountNumber
		GROUP BY LC_DFR_TYP, LD_DFR_BEG, LD_DFR_END, DFR_TYP, LD_DFR_INF_CER, LC_LON_LEV_DFR_CAP, MONTHS_USED
		
		UNION

		SELECT DISTINCT 'F' + LC_FOR_TYP as TypeCode,
						CAST(LD_FOR_BEG as DateTime) as BeginDateDate,
						LD_FOR_END as EndDate,
						FOR_TYP as [Type],
						LD_FOR_INF_CER as CertDate,
						LI_CAP_FOR_INT_REQ as CapInd,
						MONTHS_USED as MonthsUsed,
						MAX(LN_SEQ) as OneOfTheLoanSequenceNumbersAssociatedWithDeferOrForb
		FROM dbo.FB10_Forbearance as For1
		WHERE DF_SPE_ACC_ID = @AccountNumber
		GROUP BY LC_FOR_TYP, LD_FOR_BEG, LD_FOR_END, FOR_TYP, LD_FOR_INF_CER, LI_CAP_FOR_INT_REQ, MONTHS_USED
	) as CompleteTable
	ORDER BY BeginDateDate
	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetDefermentAndForbearenceData] TO [Imaging Users]
    AS [dbo];

