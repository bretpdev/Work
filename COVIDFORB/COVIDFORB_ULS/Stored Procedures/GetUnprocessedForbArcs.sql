CREATE PROCEDURE [covidforb].[GetUnprocessedForbArcs]
AS
	SELECT
	FP.ForbearanceProcessingId,
	FP.AccountNumber,
	FP.ForbCode,
	FP.DateRequested,
	FP.ForbearanceType,
	FP.StartDate,
	FP.EndDate,
	FP.DateCertified,
	FP.CoMakerEligibility,
	FP.AuthorizedToExceedMax,
	FP.ForbToClearDelq,
	FP.CapitalizeInterest,
	FP.PaymentAmount,
	FP.SignatureOfBorrower,
	FP.SelectAllLoans,
	FP.BusinessUnitId,
	FP.SubType,
	FP.SchoolCode,
	FP.SchoolEnrollment,
	FP.ReservistNationalGuard,
	FP.DodForm,
	FP.SignatureOfOfficial,
	FP.PhysiciansCertification,
	FP.MedicalInternship,
	FP.StateLicensingCertificationProvided,
	FP.ArcComment,
	FP.Failure
FROM
	[covidforb].ForbearanceProcessing FP
WHERE
	FP.ForbearanceProcessedOn IS NOT NULL
	AND FP.ArcAddProcessingId IS NULL
	AND FP.DeletedAt IS NULL
	AND CAST(FP.ProcessOn AS DATE) <= CAST(GETDATE() AS DATE)