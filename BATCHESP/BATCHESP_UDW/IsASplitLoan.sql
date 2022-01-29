CREATE PROCEDURE [batchesp].[IsASplitLoan]
	@BorrowerSsn CHAR(9),
	@LoanSeq SMALLINT
AS

DECLARE @IsSplit BIT = 0
	IF EXISTS
	(
		SELECT
			PD10.DF_SPE_ACC_ID
		FROM
			PD10_PRS_NME PD10
			INNER JOIN LN10_LON LN10_A
				ON LN10_A.BF_SSN = PD10.DF_PRS_ID
				AND LN10_A.IC_LON_PGM IN ('DLSCNS', 'DLUCNS', 'SUBCNS', 'UNCNS') --Has to be a consol type
				AND LN10_A.LN_SEQ = @LoanSeq
				AND PD10.DF_PRS_ID = @BorrowerSsn
				AND LN10_A.LA_CUR_PRI > 0 -- Filter out PIF loans since they are not selectable in Session, and thus won't encounter 
			INNER JOIN LN10_LON LN10_B
				ON LN10_B.BF_SSN = LN10_A.BF_SSN
				AND LN10_B.IC_LON_PGM IN ('DLSCNS', 'DLUCNS', 'SUBCNS', 'UNCNS')
				AND LN10_B.LN_SEQ != LN10_A.LN_SEQ --Is not the same loan as the first
				AND LN10_B.LD_LON_1_DSB = LN10_A.LD_LON_1_DSB --Disbursed on the same date
				AND LN10_B.LA_CUR_PRI > 0 -- Both consol splits have to be non-PIF, otherwise their selection is not stacked in the Session
	)
	SET @IsSplit = 1

SELECT
	@IsSplit

RETURN 0
