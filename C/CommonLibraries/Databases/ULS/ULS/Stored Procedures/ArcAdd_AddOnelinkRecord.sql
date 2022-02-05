CREATE PROCEDURE [dbo].[ArcAdd_AddOnelinkRecord]
	@AccountIdentifier VARCHAR(10),
	@AssociatedPersonID VARCHAR(10) = NULL,
	@ActionCode VARCHAR(5),
	@ActivityType CHAR(2),
	@ActivityContactType CHAR(2),
	@ScriptId VARCHAR(10),
	@DocumentID VARCHAR(4) = NULL,
	@ActivityDateTime DATETIME = NULL,
	@ActivityCloseDate DATETIME = NULL,
	@UniqueID VARCHAR(17) = NULL,
	@InstitutionID VARCHAR(8) = NULL,
	@UserID varchar(8) = NULL,
	@ClaimPackageCreateDate DATETIME = NULL,
	@UserIDClaimPackage VARCHAR(8) = NULL,
	@Comment VARCHAR(589) = NULL

AS
	INSERT INTO ArcAddProcessingOnelink(AccountIdentifer, AssociatedPersonID, ActionCode, ActivityType, ActivityContactType, ScriptID, DocumentID, ActivityDateTime, ActivityCloseDate, UniqueID, InstitutionID, UserID, ClaimPackageCreateDate, UserIDClaimPackage, Comment)
	VALUES(@AccountIdentifier, @AssociatedPersonID, @ActionCode, @ActivityType, @ActivityContactType, @ScriptId, @DocumentID, @ActivityDateTime, @ActivityCloseDate, @UniqueID, @InstitutionID, @UserID, @ClaimPackageCreateDate, @UserIDClaimPackage, @Comment)
RETURN 0
