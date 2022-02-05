CREATE PROCEDURE [barcodefed].GetRecords
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		ReturnMailId,
		RecipientId,
		LetterId,
		CreateDate,
		Address1,
		Address2,
		City,
		[State],
		Zip,
		Country,
		BorrowerSsn,
		PersonType
	FROM
		[barcodefed].ReturnMail
	WHERE
		ProcessedAt IS NULL
		AND
		DeletedAt IS NULL
END