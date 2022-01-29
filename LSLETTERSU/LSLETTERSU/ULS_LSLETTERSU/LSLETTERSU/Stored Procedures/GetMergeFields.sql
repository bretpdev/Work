CREATE PROCEDURE [lslettersu].[GetMergeFields]
	@LoanServicingLettersId VARCHAR(10)
AS
	SELECT
		MF.MergeField,
		FF.FormField
	FROM
		lslettersu.MergeFieldMapping MM
		LEFT JOIN lslettersu.MergeFields MF
			ON MM.MergeFieldsId = MF.MergeFieldsId
		LEFT JOIN lslettersu.FormFields FF
			ON MM.FormFieldsId = FF.FormFieldsId
		LEFT JOIN lslettersu.LoanServicingLetters LL
			ON MM.LoanServicingLettersId = LL.LoanServicingLettersId
	WHERE
		LL.LoanServicingLettersId = @LoanServicingLettersId
RETURN 0