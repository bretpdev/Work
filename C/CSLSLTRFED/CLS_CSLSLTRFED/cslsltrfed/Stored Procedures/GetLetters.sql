CREATE PROCEDURE [cslsltrfed].[GetLetters]
AS
	SELECT
		L.LoanServicingLettersId,
		L.LetterType,
		L.LetterOptions,
		L.LetterChoices,
		L.CheckForCoBorrower,
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
		cslsltrfed.LoanServicingLetters L
		LEFT JOIN cslsltrfed.StoredProcedures S
			ON L.StoredProceduresId = S.StoredProceduresId
RETURN 0