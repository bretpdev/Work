

/********************************************************
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		09/17/2012	Jarom Ryan		Will get data for the given record number
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spBANKOGetResponseData]
	@recordNUmber as BigInt

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		RecordNumber,
		CaseNumber,
		StateField,
		Chapter,
		FileDate,
		StatusDate,
		DispositionCode,
		AttorneyName,
		AttorneyAddress,
		AttorneyCity,
		AttorneyState,
		AttorneyZip,
		AttorneyPhone,
		CourtDistrict,
		CourtAddress1,
		CourtAddress2,
		CourtMailingCity,
		CourtZip,
		CourtPhone,
		ProofOfClaimBarDate
	FROM BankoReceiveResponseOutput
	Where RecordNumber = @recordNUmber

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spBANKOGetResponseData] TO [db_executor]
    AS [dbo];



