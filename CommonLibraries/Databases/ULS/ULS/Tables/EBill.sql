CREATE TABLE [dbo].[EBill] (
	EbillId int not null IDENTITY,
    [SSN]               CHAR (9) NOT NULL,
    [LoanSequence]      INT      NOT NULL,
    [BillingPreference] CHAR (1) NOT NULL,
	[EmailAddress]		varchar(300) NOT NULL,
    [UpdateSucceeded]   BIT      CONSTRAINT [DF_EBill_UpdateSucceeded] DEFAULT ((0)) NOT NULL,
    [UpdatedAt] DATETIME NULL, 
	[ArcAdded] bit null DEFAULT 0,
	ArcAddedAt datetime null ,
	HadError bit null DEFAULT 0,
	ErroredAt DateTime null,
	[AddedAt] DATETIME NOT NULL DEFAULT getdate(), 
    CONSTRAINT [PK_EBill] PRIMARY KEY CLUSTERED ([EbillId])
);

