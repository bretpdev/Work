CREATE PROCEDURE [dbo].[spCentralizedPrintingArcOrQueue]
	@BusinessUnit Varchar(50),
	@WhatClass Varchar(50)
AS
	SELECT ArcOrQueue FROM PRNT_REF_BUsXArcsAndQueues WHERE BU = @BusinessUnit AND Class = @WhatClass
RETURN 0
