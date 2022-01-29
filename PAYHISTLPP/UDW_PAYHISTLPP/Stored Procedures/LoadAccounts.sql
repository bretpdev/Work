CREATE PROCEDURE [payhistlpp].[LoadAccounts]
	@UserAccessId INT,
	@RunId INT,
	@Lender VARCHAR(6),
	@Count INT,
	@IsTilp BIT
AS
	
	INSERT INTO ULS.payhistlpp.Accounts(Ssn, Lender, UserAccessId, RunId)
	SELECT DISTINCT TOP (@Count)
		LN10.BF_SSN,
		LEFT(LN10.LF_LON_CUR_OWN, 6),
		@UserAccessId,
		@RunId
	FROM
		UDW..LN90_FIN_ATY LN90
		INNER JOIN UDW..LN10_LON LN10
			ON LN10.BF_SSN = LN90.BF_SSN
			AND LN10.LN_SEQ = LN90.LN_SEQ
		LEFT JOIN ULS.payhistlpp.Accounts ACCT
			ON ACCT.Ssn = LN10.BF_SSN
			AND ACCT.DeletedAt IS NULL
	WHERE
		LEFT(LN10.LF_LON_CUR_OWN, 6) = @Lender
		AND LN10.LA_CUR_PRI > 0.00
		AND LN10.LC_STA_LON10 = 'R'
		AND LN90.LC_STA_LON90 = 'A'
		AND ISNULL(LN90.LC_FAT_REV_REA,'') = ''
		AND LN90.LA_FAT_CUR_PRI > 0.00
		AND ACCT.Ssn IS NULL
		AND ((@IsTilp = 1 AND LN10.IC_LON_PGM = 'TILP') OR (@IsTilp = 0 AND LN10.IC_LON_PGM != 'TILP'))