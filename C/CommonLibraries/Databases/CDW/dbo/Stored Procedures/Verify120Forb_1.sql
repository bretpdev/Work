﻿
CREATE PROCEDURE [dbo].[Verify120Forb]

AS
	IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'LI_FOR_VRB_DFL_RUL' AND Object_ID = Object_ID(N'LN60_BR_FOR_APV'))
BEGIN
    SELECT
		1
END
RETURN 0