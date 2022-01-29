USE [CLS]
GO
/****** Object:  StoredProcedure [fp].[GetBulkLoadCount]    Script Date: X/XX/XXXX X:XX:XX AM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [fp].[GetBulkLoadCount]
AS
	SELECT COUNT(*) FROM [fp]._BulkLoad
RETURN X
