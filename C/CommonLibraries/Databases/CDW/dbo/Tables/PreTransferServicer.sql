CREATE TABLE [dbo].[PreTransferServicer] (
    [PreTransferServicerId] INT           IDENTITY (1, 1) NOT NULL,
    [RegionDeconversion]    CHAR (3)      NOT NULL,
    [ServicerName]          VARCHAR (50)  NOT NULL,
    [Website]               VARCHAR (100) NULL,
    [Phone]                 VARCHAR (12)  NULL,
    [PaymentAddress]        VARCHAR (200) NULL,
    [CorrespondenceAddress] VARCHAR (200) NULL,
    CONSTRAINT [PK_PreTransferServicer] PRIMARY KEY CLUSTERED ([PreTransferServicerId] ASC)
);

