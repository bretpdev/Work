﻿CREATE PROCEDURE [dbo].[GetEndorserIndexRecord]
	@Ssn CHAR(9),
	@EndorserSsn CHAR(9)
AS
SELECT DISTINCT
    LN20.LF_EDS AS Ssn, --WERE USING THE ENDORSER SSN WITH THE BORROWER INFORMATION FOR NOW
    CentralData.dbo.TRIM(PD10.DM_PRS_LST) AS LastName,
    CentralData.dbo.TRIM(PD10.DM_PRS_1) AS FirstName,
    'SRVH' AS DocType,
    FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR(3)),3) AS LoanId,
    CONVERT(VARCHAR(10), GETDATE(), 101) as DocDate,
    'DIRL' AS LoanProgramType,
    LN10.LD_LON_GTR AS GuarantyDate,
    '11/09/2020' AS SaleDate,
    MAX(CASE WHEN LN10.LC_FED_PGM_YR = 'DLO' THEN 'PSAOG' ELSE 'PSAOH' END) OVER(PARTITION BY LN10.BF_SSN) AS DealId
FROM
    CDW..PD10_PRS_NME PD10
    INNER JOIN CDW..LN10_LON LN10
        ON LN10.BF_SSN = PD10.DF_PRS_ID
    INNER JOIN CDW..LN20_EDS LN20
        ON LN10.BF_SSN = LN20.BF_SSN
        AND LN10.LN_SEQ = LN20.LN_SEQ
    INNER JOIN CDW..FS10_DL_LON FS10
        ON FS10.BF_SSN = LN10.BF_SSN
        AND FS10.LN_SEQ = LN10.LN_SEQ
WHERE
    LN10.BF_SSN = @Ssn
    AND LN20.LF_EDS = @EndorserSsn