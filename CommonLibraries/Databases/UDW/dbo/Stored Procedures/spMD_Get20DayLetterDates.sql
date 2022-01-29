﻿
CREATE PROCEDURE [dbo].[spMD_Get20DayLetterDates]
	@AccountNumber		VARCHAR(10) 
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		CONVERT(VARCHAR(10), AY10.LD_ATY_REQ_RCV, 101) AS SentDate
	FROM
		AY10_BR_LON_ATY AY10
		INNER JOIN PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = AY10.BF_SSN
	WHERE
		AY10.PF_REQ_ACT = 'DL200'
		AND PD10.DF_SPE_ACC_ID = @AccountNumber
		AND AY10.LD_ATY_REQ_RCV >= DATEADD(YEAR, - 3, GETDATE())
		AND PF_RSP_ACT = 'PRNTD'

	ORDER BY
		LD_ATY_REQ_RCV DESC

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_Get20DayLetterDates] TO [UHEAA\Imaging Users]
    AS [dbo];

