USE [ULS]
GO
/****** Object:  StoredProcedure [print].[GetBulkLoadCount]    Script Date: 2/25/2016 10:44:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [print].[GetBulkLoadCount]
AS
	SELECT COUNT(*) FROM [print]._BulkLoad
RETURN 0
