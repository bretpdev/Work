USE [ULS]
GO

/****** Object:  StoredProcedure [portcount].[InsertCounts]    Script Date: 3/1/2021 1:58:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [portcount].[InsertCounts]
AS

MERGE 
	portcount.PortfolioCounts PC
USING
	(
		SELECT
			COUNT(DISTINCT LN10.BF_SSN) AS UheaaBorrowerCount,
			COUNT(*) AS UheaaLoanCount,
			CAST(GETDATE() AS DATE) AS CreatedAt,
			SUSER_SNAME() AS CreatedBy
		FROM
			UDW..LN10_LON LN10
			INNER JOIN UDW..DW01_DW_CLC_CLU DW01
				ON DW01.BF_SSN = LN10.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
		WHERE
			LN10.LC_STA_LON10 = 'R'
			AND ABS(ISNULL(LN10.LA_CUR_PRI, 0.00)) + ABS(ISNULL(LN10.LA_NSI_OTS, 0.00)) > 0.00
	) NewData 
		ON NewData.CreatedAt = PC.CreatedAt
WHEN MATCHED THEN 
	UPDATE SET 
		PC.UheaaBorrowerCount = NewData.UheaaBorrowerCount,
		PC.UheaaLoanCount = NewData.UheaaLoanCount
WHEN NOT MATCHED THEN
	INSERT 
	(
		UheaaLoanCount,
		UheaaBorrowerCount,
		CreatedAt,
		CreatedBy
	)
	VALUES 
	(
		NewData.UheaaLoanCount,
		NewData.UheaaBorrowerCount,
		NewData.CreatedAt,
		NewData.CreatedBy
	)
;

RETURN 0
GO


