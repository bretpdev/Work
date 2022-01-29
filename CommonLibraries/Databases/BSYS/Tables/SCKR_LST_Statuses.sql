CREATE TABLE [dbo].[SCKR_LST_Statuses] (
    [Status]      NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (100) NULL,
    [Order]       INT            NULL,
    CONSTRAINT [PK_lstStatuses] PRIMARY KEY CLUSTERED ([Status] ASC) WITH (FILLFACTOR = 90)
);

