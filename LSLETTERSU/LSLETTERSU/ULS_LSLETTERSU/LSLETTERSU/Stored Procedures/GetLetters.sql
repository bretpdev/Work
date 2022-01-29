CREATE PROCEDURE [lslettersu].[GetLetters]
AS
	SELECT
		L.LoanServicingLettersId,
		L.LetterType,
		L.LetterOptions,
		REPLACE(L.LetterChoices, '"', '') AS [LetterChoices],
		L.LetterId,
		S.StoredProcedureName,
		L.Arc,
		L.DischargeAmount,
		L.SchoolName,
		L.LastDateAttendance,
		L.SchoolClosureDate,
		L.DefForbType,
		L.DefForbEndDate,
		L.LoanTermEndDate,
		L.SchoolYear,
		L.AdditionalReason,
		L.DeathLetter
	FROM
		lslettersu.LoanServicingLetters L
		LEFT JOIN lslettersu.StoredProcedures S
			ON L.StoredProceduresId = S.StoredProceduresId
RETURN 0