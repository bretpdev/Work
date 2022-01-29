/*
	X. Put document in X:\PADD\CenralizedPrinting\ecorr
	X. Insert record into ecorruheaa.dbo.documentdetails
	X. Insert a record into [EcorrUheaa].[dbo].[Letters]
*/
USE EcorrUheaa
GO
DECLARE @idnty INT

INSERT INTO
	[ECorruheaa]..[Letters]
(
	Letter, 
	LetterTypeId, 
	DocId, 
	Viewable, 
	ReportDescription, 
	ReportName, 
	Viewed, 
	MainframeRegion, 
	SubjectLine, 
	DocSource, 
	DocComment, 
	WorkFlow, 
	DocDelete 
)
VALUES
(
	'CUSTMXOFF',
	X,
	'XELT', -- docId
	'Y',-- viewable
	'Description',--ReportDescription, 
	'Name',--ReportName, 
	'N',--Viewed, 
	'PUT',--MainframeRegion, 
	'An Update Regarding Your Inquery',--SubjectLine, 
	'IMPORT',--DocSource, 
	'An Update Regarding Your Inquery',--DocComment, 
	'N',--WorkFlow, 
	'N'--DocDelete, 
)

SET @idnty = SCOPE_IDENTITY()

SELECT * FROM Letters WHERE LetterId = @idnty

INSERT INTO
	EcorrUheaa..DocumentDetails
(
	LetterId, 
	[Path], 
	Ssn,
	DocDate,
	ADDR_ACCT_NUM, 
	RequestUser, 
	CorrMethod, 
	LoadTime, 
	AddresseeEmail,
	CreateDate
)
VALUES
(
	@idnty,
	'\bulk\FILENET_UHEAA_UT\InboundRequest\{X}\CUSTMXOFF_SuXXccUykXeYxvlvqdXXXXX.pdf',
	'XXXXXXXXX',
	CAST(GETDATE() AS DATE), -- docdate
	'XXXXXXXXXX', --ADDR_ACCT_NUM
	'UTXXXXX', -- requestuser
	 'EmailNotify',--CorrMethod
	 GETDATE(),
	 'LUCAS.GASCO@GMAIL.COM',
	 GETDATE()
)

SELECT
	*
FROM
	EcorrUheaa..DocumentDetails DD
WHERE
	DD.LetterId = @idnty
