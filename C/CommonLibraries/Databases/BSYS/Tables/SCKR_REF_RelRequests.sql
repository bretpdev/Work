CREATE TABLE [dbo].[SCKR_REF_RelRequests] (
    [Request]  INT           NOT NULL,
    [Class]    NVARCHAR (50) NOT NULL,
    [RRequest] INT           NOT NULL,
    [RClass]   NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_refRelRequests] PRIMARY KEY CLUSTERED ([Request] ASC, [Class] ASC, [RRequest] ASC, [RClass] ASC) WITH (FILLFACTOR = 90)
);

