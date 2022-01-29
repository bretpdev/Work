
CREATE PROCEDURE [dbo].[spMD_GetReferenceActivityHistory]
	@AccountNumber		varchar(10),
	@RecipientId		varchar(9)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT	LN_ATY_SEQ AS SequenceNumber,
			PF_REQ_ACT AS RequestCode,
			PF_RSP_ACT AS ResponseCode,
			PX_ACT_DSC_REQ AS RequestDescription,
			LD_ATY_REQ_RCV AS RequestDate,
			LD_ATY_RSP AS ResponseDate,
			LF_USR_REQ_ATY AS Requestor,
			LT_ATY_RSP AS PerformedDate,
			LX_ATY AS CommentText
	FROM	dbo.AY10_ReferenceHistory
	WHERE	DF_SPE_ACC_ID = @AccountNumber
			AND LF_ATY_RCP = @RecipientId
			
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetReferenceActivityHistory] TO [Imaging Users]
    AS [dbo];

