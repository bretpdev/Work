USE [UDW]
GO
/****** Object:  StoredProcedure [dbo].[TruncateTable]    Script Date: 10/30/2020 10:15:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[TruncateTable]  @tableName varchar(50)
AS
DECLARE @SQL VARCHAR(2000)
SET @SQL='TRUNCATE TABLE ' + @tableName
EXEC (@SQL)