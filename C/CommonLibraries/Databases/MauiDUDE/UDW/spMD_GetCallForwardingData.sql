﻿use [UDW]
go
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('spMD_GetCallForwardingData') AND TYPE IN ('P', 'PC'))
DROP PROCEDURE spMD_GetCallForwardingData
GO
CREATE PROCEDURE [dbo].[spMD_GetCallForwardingData]
	@AccountNumber VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT cf.FORWARDING, l.IF_GTR
	FROM dbo.GA10_CallForwarding cf
    left join dbo.LN10_Loan l on cf.DF_SPE_ACC_ID = l.DF_SPE_ACC_ID and cf.LN_SEQ = l.LN_SEQ
	WHERE cf.DF_SPE_ACC_ID = @AccountNumber
END