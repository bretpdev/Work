CREATE TABLE [dbo].[DocumentDetails]
(
    [DocumentDetailsId] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [LetterId] INT NOT NULL,
    [Path] VARCHAR(MAX) NOT NULL, 
    [Ssn] CHAR(9) NOT NULL, 
    [DocDate] DATE NOT NULL, 
    [ADDR_ACCT_NUM] VARCHAR(10) NOT NULL, 
    [RequestUser] VARCHAR(8) NOT NULL, 
    [CorrMethod] VARCHAR(20) NOT NULL, 
    [LoadTime] DATETIME NOT NULL, 
    [AddresseeEmail] VARCHAR(254) NOT NULL, 
    [CreateDate] DATE NOT NULL, 
    [DueDate] DATETIME NULL, 
    [TotalDue] VARCHAR(15) NULL, 
    [BillSeq] CHAR(4) NULL, 
    [Printed] DATETIME NULL, 
    [AesDateTime] NCHAR(10) NULL, 
    [EmailSent] DATETIME NULL, 
	[ZipFileName] VARCHAR(300) NULL,
	[CorrespondenceFormatId] int not null DEFAULT 1,
	[CorrespondenceFormatSentDate] DATETIME NULL,
    [Active] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_DocumentDetails_Letters] FOREIGN KEY ([LetterId]) REFERENCES Letters(LetterId), 
	CONSTRAINT [CK_DocumentDetails_CorrMethod] CHECK (CorrMethod COLLATE latin1_general_cs_as IN ('Printed' ,'Direct Debit', 'Email Notify')), -- These are the only valid values
    CONSTRAINT [FK_DocumentDetails_CorrespondenceFormats] FOREIGN KEY (CorrespondenceFormatId) REFERENCES CorrespondenceFormats(CorrespondenceFormatId)	
)
