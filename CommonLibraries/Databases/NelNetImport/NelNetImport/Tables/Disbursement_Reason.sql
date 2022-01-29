CREATE TABLE [dbo].[Disbursement_Reason] (
    [disbursement_reason_id] INT           IDENTITY (1, 1) NOT NULL,
    [reason_code]            CHAR (2)      NULL,
    [reason]                 VARCHAR (255) NULL
);

