CREATE TABLE [dbo].[GENR_LST_BillPayPayee] (
    [Payee]     NVARCHAR (50) NOT NULL,
    [Address1]  NVARCHAR (50) NOT NULL,
    [Address2]  NVARCHAR (50) NULL,
    [City]      NVARCHAR (50) NOT NULL,
    [State]     NVARCHAR (50) NOT NULL,
    [Zip]       NVARCHAR (50) NOT NULL,
    [Country]   NVARCHAR (50) NULL,
    [PayeeCode] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_GENR_LST_BillPayPayee] PRIMARY KEY CLUSTERED ([Payee] ASC)
);

