﻿CREATE PROCEDURE [idruserpro].[GetLoansForCoBorrower]
	@AccountNumber VARCHAR(10)
AS

BEGIN

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	SELECT
		LN20.LN_SEQ
	FROM
		PD10_PRS_NME PD10
		INNER JOIN LN20_EDS LN20
			ON PD10.DF_PRS_ID = LN20.LF_EDS
			AND LN20.LC_EDS_TYP = 'M'
			AND LN20.LC_STA_LON20 = 'A'
	WHERE
		@AccountNumber IN (PD10.DF_PRS_ID, PD10.DF_SPE_ACC_ID)

END