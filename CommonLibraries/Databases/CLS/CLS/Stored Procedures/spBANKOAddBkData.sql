
/********************************************************
*Routine Name	: [dbo].[spBANKOAddBkData]
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		08/06/2012	Jarom Ryan		Will add BK data to table and return the RecordIndex
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spBANKOAddBkData]
	-- Add the parameters for the stored procedure here
	  @CaseNumber as Varchar(12),
	  @StateField as Char(2),
	  @Chapter as Varchar(2),
	  @FileDate as Varchar(10),
	  @StatusDate as Varchar(10),
	  @DispositionCode as Varchar(2),
	  @AttorneyName as Varchar(35),
	  @AttorneyAddress as Varchar(32),
	  @AttorneyCity as Varchar(25),
	  @AttorneyState as Varchar(2),
	  @AttorneyZip as Varchar(10),
	  @AttorneyPhone as Varchar(10),
	  @CourtDistrict as Varchar(30),
	  @CourtAddress1 as Varchar(35),
	  @CourtAddress2 as Varchar(35),
	  @CourtMailingCity as Varchar(20),
	  @CourtZip as Varchar(5),
	  @CourtPhone as Varchar(10),
	  @ProofOfClaimBarDate as Varchar(10)  
	  

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;
	
	insert into dbo.BankoReceiveResponseOutput(CaseNumber,StateField,Chapter,FileDate,StatusDate,DispositionCode,AttorneyName,
	AttorneyAddress,AttorneyCity,AttorneyState,AttorneyZip,AttorneyPhone,CourtDistrict,CourtAddress1,CourtAddress2,
	CourtMailingCity,CourtZip,CourtPhone,ProofOfClaimBarDate)
	Values(@CaseNumber,@StateField,@Chapter,@FileDate,@StatusDate,@DispositionCode,@AttorneyName,@AttorneyAddress,@AttorneyCity,
	@AttorneyState, @AttorneyZip,@AttorneyPhone,@CourtDistrict,@CourtAddress1,@CourtAddress2,@CourtMailingCity,
	@CourtZip,@CourtPhone,@ProofOfClaimBarDate)
	
	
	
	select RecordNumber
	from dbo.BankoReceiveResponseOutput
	where CaseNumber = @CaseNumber and FileDate = @FileDate

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spBANKOAddBkData] TO [db_executor]
    AS [dbo];



