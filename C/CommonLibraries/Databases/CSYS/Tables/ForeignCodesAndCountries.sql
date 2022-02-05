CREATE TABLE [dbo].[ForeignCodesAndCountries] (
    [ForeignCodeId]  INT            IDENTITY (1, 1) NOT NULL,
    [ForeignCode]    CHAR (2)       NOT NULL,
    [ForeignCountry] NVARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([ForeignCodeId] ASC)
);

