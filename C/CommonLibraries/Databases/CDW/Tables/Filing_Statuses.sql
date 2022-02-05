CREATE TABLE [dbo].[Filing_Statuses] (
    [filing_status_id]          INT           IDENTITY (1, 1) NOT NULL,
    [filing_status]             TINYINT       NULL,
    [filing_status_description] VARCHAR (255) NULL
);

