USE [UDW]
GO
/****** Object:  StoredProcedure [dbo].[spGetAccountNumberFromSSN]    Script Date: 5/26/2020 1:59:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Jarom Ryan
-- Create date: 11/27/2012
-- Description:	Gets the borrowers account number for the given SSN
-- =============================================
ALTER PROCEDURE [dbo].[spGetAccountNumberFromSSN] 
	
	@Ssn AS Varchar(9)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	
	DECLARE @RowCount INT = 0
	
	SELECT DF_SPE_ACC_ID FROM dbo.PD10_PRS_NME WHERE DF_PRS_ID = @Ssn
	SET @RowCount = @@ROWCOUNT
	
	IF @RowCount != 1
	BEGIN
	
		SELECT
			DF_SPE_ACC_ID
		FROM
			ODW..PD01_PDM_INF
		WHERE
			DF_PRS_ID = @Ssn
		  
		  SET @RowCount = @@ROWCOUNT
	END
	 
	IF @RowCount = 0
	BEGIN
		RAISERROR('spAccountNumberFromSSN returned %i record(s) for SSN: %s', 16, 1, @RowCount, @Ssn) 
	END
END




