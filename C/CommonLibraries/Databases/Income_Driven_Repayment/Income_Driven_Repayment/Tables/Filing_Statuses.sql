CREATE TABLE [dbo].[Filing_Statuses] (
    [filing_status_id]          INT           IDENTITY (1, 1) NOT NULL,
    [filing_status]             TINYINT       NOT NULL,
    [filing_status_description] VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Filing_Statuses] PRIMARY KEY CLUSTERED ([filing_status_id] ASC)
);

