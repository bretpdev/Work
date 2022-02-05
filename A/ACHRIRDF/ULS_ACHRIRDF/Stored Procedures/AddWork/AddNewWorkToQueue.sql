USE [ULS]
GO
/****** Object:  StoredProcedure [achrirdf].[AddNewWorkToQueue]    Script Date: 1/23/2019 10:24:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [achrirdf].[AddNewWorkToQueue]
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED


--Create temp table that will be loaded into the processing table
IF OBJECT_ID('tempdb..#DusterData') IS NOT NULL
    DROP TABLE #DusterData

CREATE TABLE #DusterData
(
	Report VARCHAR(3),
	Ssn CHAR(9),
	AccountNumber CHAR(10),
	LoanSequence VARCHAR(3),
	OwnerCode VARCHAR(8),
	DefermentOrForbearanceOriginalBeginDate DATE,
	DefermentOrForbearanceOriginalEndDate DATE,
	DefermentOrForbearanceBeginDate DATE,
	DefermentOrForbearanceEndDate DATE,
	UpdatedAt DATETIME	
)

--Create the table that will house the contiguous deferment/forbearance periods
DECLARE @DF_PERIOD TABLE
(
	BF_SSN VARCHAR(9),
	LN_SEQ SMALLINT,
	Begin_Date DATE,
	End_Date DATE
)

--Population to limit the deferment/forbearance periods and ach periods by
DECLARE @SPECIAL_PAY_POP [achrirdf].[Population]

--Date to filter LN50/LN60 records by for deferment/forbearance periods
DECLARE @LASTRUN VARCHAR(30) = 
(
	SELECT 
		ISNULL(CONVERT(DATE,MAX(UpdatedAt),101), '2017-02-04') --USE FOR LIVE
		--ISNULL(CONVERT(DATE,MAX(UpdatedAt),101), '2016-9-28')  --USE FOR TEST
	FROM 
		ULS.[achrirdf].[ProcessQueue]
)

INSERT INTO @SPECIAL_PAY_POP(BF_SSN)
SELECT DISTINCT
	LN10.BF_SSN
FROM
	UDW..LN10_LON LN10
	INNER JOIN UDW..LN72_INT_RTE_HST LN72
		ON LN10.BF_SSN = LN72.BF_SSN
		AND LN10.LN_SEQ = LN72.LN_SEQ
	INNER JOIN UDW..LN83_EFT_TO_LON LN83
		ON LN10.BF_SSN = LN83.BF_SSN
		AND LN10.LN_SEQ = LN83.LN_SEQ
WHERE 
	LN72.LC_STA_LON72 = 'A'
	AND LN72.LC_INT_RDC_PGM = 'S'
	AND LN10.LC_STA_LON10 ='R'
	AND LN10.LA_CUR_PRI > 0.00
	AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
	AND LN83.LC_STA_LN83 = 'A'
	--AND LN72.LF_LST_DTS_LN72 > @LASTRUN

--Get Deferment/Forbearance Periods
INSERT INTO
	@DF_PERIOD
EXEC
	[achrirdf].GetDefForPeriods @SPECIAL_PAY_POP, @LASTRUN;



END