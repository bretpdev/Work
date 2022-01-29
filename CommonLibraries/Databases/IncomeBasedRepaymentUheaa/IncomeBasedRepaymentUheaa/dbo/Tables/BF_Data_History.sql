CREATE TABLE [dbo].[BF_Data_History] (
    [bf_data_history_id] INT          IDENTITY (1, 1) NOT NULL,
    [application_id]     INT          NOT NULL,
    [award_id]           CHAR (21)    NULL,
    [disclosure_date]    DATE         NULL,
    [created_at]         DATETIME     CONSTRAINT [DF_BF_created_at] DEFAULT (getdate()) NOT NULL,
    [created_by]         VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_BF_Data_History] PRIMARY KEY CLUSTERED ([bf_data_history_id] ASC)
);



