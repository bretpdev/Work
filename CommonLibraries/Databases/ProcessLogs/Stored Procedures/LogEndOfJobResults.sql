CREATE PROCEDURE [dbo].[LogEndOfJobResults]
    @ProcessLogId int,
    @ResultHeader VARCHAR(256),
    @ResultsValue VARBINARY(256) 
AS
    INSERT INTO [dbo].[EndOfJobResults](ProcessLogId, ResultHeader, ResultsValue)
    VALUES(@ProcessLogId, @ResultHeader, @ResultsValue)
RETURN 0
