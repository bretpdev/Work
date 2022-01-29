CREATE PROCEDURE [dbo].[GetUnprocessedLettersByLetterId]
	@LetterId varchar(10)
AS
BEGIN

SELECT 
	letter.LT20_LETTER_REQUEST_ID as LetterRequestId,
	letter.DF_SPE_ACC_ID as AccountNumber,
	letter.RM_DSC_LTR_PRC as LetterId,
	email.DI_CNC_ELT_OPI as Ecorr,
	RN_SEQ_LTR_CRT_PRC AS LetterSequence,
	RF_SBJ_PRC as Ssn
FROM 
	[CDW].[dbo].[LT20_LetterRequests] letter
LEFT JOIN
	CDW.dbo.PH05_ContactEmail email on email.DF_SPE_ACC_ID = letter.DF_SPE_ACC_ID
WHERE 
	    [PrintedAt] IS NULL 
	AND [InactivatedAt] IS NULL
	AND [RM_DSC_LTR_PRC] = @LetterId

END