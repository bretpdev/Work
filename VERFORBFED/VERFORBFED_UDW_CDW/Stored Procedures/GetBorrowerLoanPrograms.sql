﻿CREATE PROCEDURE [verforbfed].[GetBorrowerLoanPrograms]
	@Ssn CHAR(9)
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SELECT
		RTRIM(LN10.IC_LON_PGM)
	FROM
		LN10_Lon LN10
	WHERE
		LN10.BF_SSN = @Ssn


RETURN 0
