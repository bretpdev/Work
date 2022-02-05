CREATE TABLE [dbo].[Filing_Statuses] (
    [filing_status_id]          INT NOT NULL,
    [filing_status]             TINYINT       NOT NULL,
    [filing_status_description] VARCHAR (255) NOT NULL,
	[status_for_married]			BIT NULL,
    CONSTRAINT [PK_Filing_Statuses] PRIMARY KEY CLUSTERED ([filing_status_id] ASC)
);

