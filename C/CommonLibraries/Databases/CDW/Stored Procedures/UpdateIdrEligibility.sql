CREATE PROCEDURE [dbo].[UpdateIdrEligibility]
	@DF_PRS_ID char(9),
	@LF_FED_AWD varchar(18),
	@LN_FED_AWD_SEQ int,
	@IdrInEligible bit = NULL,
	@ICRInELigible bit = NULL
AS
	
	UPDATE
		GRSP_NDS_LON_RSP
	SET
		IdrIneligible = @IdrInEligible,
		IcrIneligible = @ICRInELigible
	WHERE
		DF_PRS_ID = @DF_PRS_ID
		AND LF_FED_AWD = @LF_FED_AWD
		AND LN_FED_AWD_SEQ = @LN_FED_AWD_SEQ


RETURN 0
