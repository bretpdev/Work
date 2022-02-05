CREATE TABLE [dbo].[BE_Data_History] (
    [be_data_history_id]           INT          IDENTITY (1, 1) NOT NULL,
    [application_id]               INT          NOT NULL,
    [award_id]                     CHAR (21)    NULL,
    [repayment_plan_type]          INT          NULL,
    [requested_by_borrower]        BIT          NULL,
    [substatus_mapping_id]         INT          NULL,
    [repayment_application_status] DATE         NULL,
    [total_income]                 INT          NULL,
    [created_at]                   DATETIME     CONSTRAINT [DF_BE_created_at] DEFAULT (getdate()) NOT NULL,
    [created_by]                   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_BE_Data_History] PRIMARY KEY CLUSTERED ([be_data_history_id] ASC)
);



