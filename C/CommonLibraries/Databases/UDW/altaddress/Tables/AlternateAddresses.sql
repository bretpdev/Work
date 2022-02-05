CREATE TABLE [altaddress].[AlternateAddresses] (
    [AlternateAddressId] INT             IDENTITY (1, 1) NOT NULL,
    [AccountNumber]      CHAR (10)       NOT NULL,
    [Address1]           NVARCHAR (1000) NOT NULL,
    [Address2]           NVARCHAR (1000) NULL,
    [City]               NVARCHAR (100)  NOT NULL,
    [State]              NVARCHAR (100)  NOT NULL,
    [Zip]                NVARCHAR (100)  NOT NULL,
    [Country]            NVARCHAR (100)  NULL,
    PRIMARY KEY CLUSTERED ([AlternateAddressId] ASC)
);

