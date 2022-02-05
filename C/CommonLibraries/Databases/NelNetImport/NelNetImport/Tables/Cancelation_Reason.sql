CREATE TABLE [dbo].[Cancelation_Reason] (
    [cancelation_reason_id] INT           IDENTITY (1, 1) NOT NULL,
    [nn_reason_code]        CHAR (2)      NULL,
    [aes_reason_code]       CHAR (4)      NULL,
    [nn_description]        VARCHAR (255) NULL,
    [aes_description]       VARCHAR (255) NULL
);

