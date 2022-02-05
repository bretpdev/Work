CREATE TABLE [dbo].[Graduated_Payment_Flags] (
    [graduated_paymnet_flags_id] INT           IDENTITY (1, 1) NOT NULL,
    [nn_flag]                    CHAR (1)      NULL,
    [compass_flag]               VARCHAR (2)   NULL,
    [description]                VARCHAR (100) NULL
);

