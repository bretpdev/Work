CREATE TABLE [dbo].[DocTypes]
(
	[DocTypeId] TINYINT NOT NULL PRIMARY KEY IDENTITY, 
    [DocTypeValue] CHAR(4) NOT NULL, 
    [AddedBy] NVARCHAR(50) NOT NULL DEFAULT SYSTEM_USER,
	[RemovedBy] NVARCHAR(50) NULL
)

GO

CREATE TRIGGER [dbo].[Trigger_DocTypes_RemovedBy]
    ON [dbo].[DocTypes]
    FOR UPDATE
    AS
    BEGIN
        update di set di.RemovedBy = dt.RemovedBy
		  from DocIds di
		  join DocTypes dt on dt.DocTypeId = di.DocTypeId
		  join inserted i on i.DocTypeId = dt.DocTypeId
		 where i.RemovedBy is not null
    END
GO